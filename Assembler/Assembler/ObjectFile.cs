using System;
using System.IO;
using System.Collections.Generic;

namespace Assembler
{
    public class ObjectFile
    {

        private List<LinkingRecord> linkingRecords = new List<LinkingRecord>();

        private List<TextRecord> textRecords = new List<TextRecord>();

        private List<ModificationRecord> modificationRecords = new List<ModificationRecord>();

        private IntermediateFile input;

        private SymbolTable symb;

        private AssemblyReport report;

        public ObjectFile(ref IntermediateFile input, ref SymbolTable symb, ref AssemblyReport report)
        {
            this.input = input;
            this.symb = symb;
            this.report = report;
        }

        /**
         * Add a linking record to this object file.
         */
        private void AddRecord(LinkingRecord record)
        {
            this.linkingRecords.Add(record);
        }

        /**
         * Add a text record to this object file.
         */
        private void AddRecord(TextRecord record)
        {
            this.textRecords.Add(record);
        }

        /**
         * Add a modification record to this object file.
         */
        private void AddRecord(ModificationRecord record)
        {
            this.modificationRecords.Add(record);
        }

        /**
         * Scan the symbol table for ENTRY symbols and create linking records
         * for them.
         */
        private void GenerateLinkingRecords()
        {
            // iterate the symbol table
            foreach (string symbName in this.symb.SortedSymbols())
            {
                // we only care about ENTRY symbols
                Symbol symb = this.symb.GetSymbol(symbName);
                if (symb.usage == Usage.ENTRY)
                {
                    // generate a linking record
                    var record = new LinkingRecord(this.symb.ProgramName);
                    record.EntryName = symb.rlabel;
                    record.ProgramLocation = symb.lc;
                    this.AddRecord(record);
                }
            }
        }

        /**
         * Scan and the source (pass 2) and generate text and modification
         * records as apporpriate.
         */
        private void GenerateSourceRecords()
        {
            foreach (IntermediateLine line in this.input)
            {
                // only scan lines that will actually be in the output
                if (line.ProgramCounter == null)
                {
                    // list of errors
                    List<Errors.Error> errors = line.GetThreeErrors();

                    this.report.Add(null, null, ' ',
                        line.SourceLineNumber, line.SourceLine, errors);
                    continue;
                }

                // create the text record
                TextRecord rec = new TextRecord(this.symb.ProgramName);
                rec.ProgramLocation = line.ProgramCounter;
                rec.StatusFlag = 'A';
                rec.Adjustments = "0";
                string bin = line.Bytecode;

                // do we have an instruction?
                if (line.OpCategory != null)
                {
                    // equated symbols may need to be relocated
                    if (line.Equated != null)
                    {
                        Symbol symb = this.symb.GetSymbol(line.Equated);
                        ModificationRecord mod = new ModificationRecord(this.symb.ProgramName);
                        mod.ProgramLocation = line.ProgramCounter;
                        mod.Word = Convert.ToString(Convert.ToInt32(line.Bytecode, 2), 16);
                        for (int i = 0; i < symb.relocations; i++)
                        {
                            mod.AddAdjustment(true, this.symb.ProgramName);
                        }
                        this.AddRecord(mod);
                        rec.StatusFlag = 'M';
                        rec.Adjustments = Convert.ToString(symb.relocations, 16);
                    }
                    // or a plain label
                    else if (line.OpLitOperand == OperandParser.Literal.NONE)
                    {
                        // get the symbol that is being referenced
                        if (this.symb.ContainsSymbol(line.OpOperand))
                        {
                            Symbol symb = this.symb.GetSymbol(line.OpOperand);
                            // external labels are processed in the linker
                            if (symb.usage == Usage.EXTERNAL) {
                                // create a modification record
                                ModificationRecord mod = new ModificationRecord(this.symb.ProgramName);
                                mod.ProgramLocation = line.ProgramCounter;
                                mod.Word = Convert.ToString(Convert.ToInt32(line.Bytecode, 2), 16);
                                mod.AddAdjustment(true, symb.rlabel);
                                this.AddRecord(mod);
                                // set the status to 1 modify
                                rec.StatusFlag = 'M';
                                rec.Adjustments = "1";
                            }
                            // otherwise we can resolve the symbol
                            else
                            {
                                // get the binary encoding padded to 10 bits
                                bin = BinaryHelper.BinaryString(symb.lc).PadLeft(10, '0');
                                // prefix it with the original instruction bytecode
                                bin = line.Bytecode.Substring(0, 6) + bin;
                                // relocatable label
                                rec.StatusFlag = 'R';
                                rec.Adjustments = "0";
                            }
                        }
                        // error, otherwise
                        else
                        {
                            // TODO
                            //throw new NotImplementedException("ERROR");
                        }
                    }
                    // otherwise if it is (was) an expression
                    else if (line.OpLitOperand == OperandParser.Literal.EXPRESSION)
                    {
                        // then is is relocatable
                        
                        // get the expression to be evaluated
                        string expr = line.OpOperand;

                        // create the modification record, this will have at least one
                        // adjustment because these expressions are required to have a *
                        ModificationRecord mod = new ModificationRecord(symb.ProgramName);

                        // this is a garbage variable
                        int rel;

                        // evaluate the expression
                        OperandParser.ParseExpression(ref expr, OperandParser.Expressions.Operand, 
                                                      line, ref symb, mod, out rel);
                        rec.StatusFlag = 'M';
                        rec.Adjustments = Convert.ToString(mod.Adjustments, 16);
                        int bytecode = Convert.ToInt32(bin, 2);
                        bytecode += Convert.ToInt32(expr, 16);
                        bin = Convert.ToString(bytecode, 2);
                        mod.Word = Convert.ToString(bytecode, 16);
                        mod.ProgramLocation = line.ProgramCounter;
                        this.AddRecord(mod);
                    }
                    // otherwise, it was a literal
                    else
                    {
                        rec.StatusFlag = 'A';
                        rec.Adjustments = "0";
                    }
                }
                // or a DAT directive?
                else if (line.Directive == "DAT")
                {
                    // DAT fields shouldn't need to be modified
                    rec.StatusFlag = 'A';
                    rec.Adjustments = "0";
                }
                // or an ADC directive?
                else if (line.Directive == "ADC" || line.Directive == "ADCE")
                {
                    ModificationRecord mod = new ModificationRecord(symb.ProgramName);
                    mod.AddAdjustment(true, symb.ProgramName);
                    bool worked = true;
                    string expr = line.DirectiveOperand;
                    int rel = 0;
                    int maxOp = 1;
                    if (line.Directive.EndsWith("E"))
                    {
                        maxOp = 3;
                    }
                    if (line.DirectiveLitOperand == OperandParser.Literal.EXPRESSION)
                    {
                        worked = OperandParser.ParseExpression(ref expr, OperandParser.Expressions.ADC, line,
                                                      ref symb, mod, out rel, maxOp);
                        if (worked)
                            bin = Convert.ToString(Convert.ToInt32(expr, 16), 2).PadLeft(16, '0');
                    }
                    else if (line.DirectiveLitOperand == OperandParser.Literal.NUMBER)
                    {
                        rel = 1;
                        bin = Convert.ToString(Convert.ToInt32(expr, 16), 2);
                    }
                    if (mod.Adjustments > 0 && rel > 0 && worked)
                    {
                        rec.StatusFlag = 'M';
                        rec.Adjustments = Convert.ToString(mod.Adjustments, 16);
                        mod.Word = Convert.ToString(Convert.ToInt32(bin, 2), 16);
                        mod.ProgramLocation = line.ProgramCounter;
                        this.AddRecord(mod);
                    }
                }

                // anything else with a LC shouldn't be here...
                else
                {
                    // this code *should* be unreachable
                    throw new NotImplementedException();
                }

                // convert bytecode to hex and add the record
                rec.HexCode = Convert.ToString(Convert.ToInt32(bin, 2), 16).ToUpper();
                this.AddRecord(rec);

                // list of errors
                List<Errors.Error> errorlist = line.GetThreeErrors();

                // add a line to the assembly report
                this.report.Add(line.ProgramCounter, rec.HexCode, rec.StatusFlag,
                        line.SourceLineNumber, line.SourceLine, errorlist);
            }
        }

        /**
         * Render the object file into the file given. If no filename, will
         * print to stdout.
         */
        public void Render(string filename)
        {
            // generate records
            this.GenerateLinkingRecords();
            this.GenerateSourceRecords();

            // write out the file
            var file = new StreamWriter(filename);

            // generate and write the header record
            var header = new HeaderRecord(this.symb.ProgramName);
            header.LoadAddress = input.Line(1).DirectiveOperand;
            header.ModuleLength = input.ModuleLength;
            header.ExecutionStart = header.LoadAddress;
            header.TotalLinking = Convert.ToString(this.linkingRecords.Count, 16);
            header.TotalText = Convert.ToString(this.textRecords.Count, 16);
            header.TotalModification = Convert.ToString(this.modificationRecords.Count, 16);
            header.TotalRecords = Convert.ToString(this.linkingRecords.Count +
                                                   this.textRecords.Count +
                                                   this.modificationRecords.Count, 16);
            file.WriteLine(header);

            // write linking records
            foreach (var record in this.linkingRecords)
            {
                file.WriteLine(record);
            }

            // write text records
            foreach (var record in this.textRecords)
            {
                file.WriteLine(record);
            }

            // write modification records
            foreach (var record in this.modificationRecords)
            {
                file.WriteLine(record);
            }

            // write the end record
            var end = new EndRecord(this.symb.ProgramName);
            file.WriteLine(end);

            file.Close();
        }
    }
}

