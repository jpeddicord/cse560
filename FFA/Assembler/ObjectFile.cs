using System;
using System.IO;
using System.Collections.Generic;

namespace Assembler
{
    /**
     * Object file representation. Can be filled with records and rendered to a file.
     */
    public class ObjectFile
    {
        /**
         * The list of linking records
         */
        private List<LinkingRecord> linkingRecords = new List<LinkingRecord>();

        /**
         * The list of text records
         */
        private List<TextRecord> textRecords = new List<TextRecord>();

        /**
         * The list of modification records
         */
        private List<ModificationRecord> modificationRecords = new List<ModificationRecord>();

        /**
         * The input from pass 1
         */
        private IntermediateFile input;

        /**
         * The symbol table from pass 1
         */
        private SymbolTable symb;

        /**
         * The assembly report to be generated
         */
        private AssemblyReport report;

        /**
         * Create an object file using the given inputs.
         *
         * @param input Input source from pass 1
         * @param symb The symbol table
         * @param report an assembly report to be generated
         * @refcode OB
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 10, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public ObjectFile(ref IntermediateFile input, ref SymbolTable symb, ref AssemblyReport report)
        {
            this.input = input;
            this.symb = symb;
            this.report = report;
        }

        /**
         * Add a linking record to this object file.
         *
         * @param record The linking record to add
         * @refcode OB
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 10, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private void AddRecord(LinkingRecord record)
        {
            this.linkingRecords.Add(record);
        }

        /**
         * Add a text record to this object file.
         *
         * @param record The text record to add
         * @refcode OB
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 10, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private void AddRecord(TextRecord record)
        {
            this.textRecords.Add(record);
        }

        /**
         * Add a modification record to this object file.
         *
         * @param record The modification record to add
         * @refcode OB
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 10, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private void AddRecord(ModificationRecord record)
        {
            this.modificationRecords.Add(record);
        }

        /**
         * Scan the symbol table for ENTRY symbols and create linking records
         * for them.
         *
         * @refcode OB
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 11, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
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
                    if (symb.lc != null)
                    {
                        // generate a linking record
                        var record = new LinkingRecord(this.symb.ProgramName);
                        record.EntryName = symb.rlabel;
                        record.ProgramLocation = symb.lc;
                        this.AddRecord(record);
                    }
                    // unused ENTRY directive
                    else
                    {
                        foreach (IntermediateLine line in this.input)
                        {
                            if (line.Directive == "ENTRY" && line.DirectiveOperand == symb.rlabel)
                            {
                                line.AddError(Errors.Category.Serious, 36);
                            }
                        }
                    }
                }
            }
        }

        /**
         * Scan and the source (pass 2) and generate text and modification
         * records as apporpriate.
         *
         * @refcode OB
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 9, 2011
         * @modlog
         *  - May 10, 2011 - Jacob - Implemented some modification record things
         *  - May 10, 2011 - Jacob - Refactored line iteration a bit
         *  - May 11, 2011 - Jacob - Fix how we determine the type and number of modifications
         *  - May 13, 2011 - Mark  - Implement expression processing
         *  - May 13, 2011 - Mark  - Fix one of the expression cases with modifications
         *  - May 14, 2011 - Mark  - Implement ADC/e!
         *  - May 14, 2011 - Mark  - Adjust error handling code
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private void GenerateSourceRecords()
        {
            foreach (IntermediateLine line in this.input)
            {
                //Console.WriteLine(this.SourceLine)
                // skip lines that won't be in the object file
                if (line.ProgramCounter == null)
                {
                    // special case: RESET directive should generate a linking record
                    if (line.Directive == "RESET" && line.GetThreeErrors().Count == 0)
                    {
                        var record = new LinkingRecord(this.symb.ProgramName);
                        record.EntryName = line.Label;
                        // set the location to the target of the RESET
                        record.ProgramLocation = line.DirectiveOperand;
                        this.AddRecord(record);
                    }

                    // store the rest in the report
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
                        if (symb.relocations == 0)
                        {
                            rec.StatusFlag = 'A';
                            rec.Adjustments = "0";
                        }
                        else if (symb.relocations == 1)
                        {
                            rec.StatusFlag = 'R';
                            rec.Adjustments = "0";
                        }
                        else
                        {
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
                    }
                    // or a plain label
                    else if (line.OpLitOperand == OperandParser.Literal.NONE)
                    {
                        // get the symbol that is being referenced
                        if (line.OpOperand != null && this.symb.ContainsSymbol(line.OpOperand))
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
                            }
                        }
                        // was it empty?
                        else if (line.OpOperand == null || line.OpOperand.Length == 0)
                        {
                            bin = line.Bytecode;
                            rec.StatusFlag = 'A';
                        }
                        // error, otherwise
                        else
                        {
                            line.AddError(Errors.Category.Serious, 34);
                            line.NOPificate();
                            bin = line.Bytecode;
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
                        bool worked;
                        worked = OperandParser.ParseExpression(ref expr, OperandParser.Expressions.Operand, 
                                                      line, ref symb, mod, out rel);
                        if (worked)
                        {
                            // adjustments and such
                            rec.StatusFlag = 'M';
                            rec.Adjustments = Convert.ToString(mod.Adjustments, 16);

                            // get the hex sorted out
                            int adj = Convert.ToInt32(expr, 16);
                            int bytecode = Convert.ToInt32(bin, 2) + adj;
                            
                            // check the range
                            int start = Convert.ToInt32(this.input.Line(1).DirectiveOperand, 16);
                            int eof = start + Convert.ToInt32(this.input.ModuleLength, 16);
                            if (Convert.ToInt32(expr, 16) > eof || Convert.ToInt32(expr, 16) < start)
                            {
                                line.AddError(Errors.Category.Serious, 38);
                                line.NOPificate();
                            }
                            else
                            {
                                bin = Convert.ToString(bytecode, 2);
                                mod.Word = Convert.ToString(bytecode, 16);
                                mod.ProgramLocation = line.ProgramCounter;
                                this.AddRecord(mod);
                            }
                        }
                        else
                        {

                        }
                    }
                    // special case: numeric values with GOTO, JUMP, MOPER
                    else if (line.OpLitOperand == OperandParser.Literal.NUMBER &&
                             (line.OpFunction == "GOTO" ||
                              line.OpCategory == "JUMP" ||
                              line.OpCategory == "MOPER"))
                    {
                        rec.StatusFlag = 'R';
                    }
                    // otherwise, it was a literal
                    else
                    {
                        rec.StatusFlag = 'A';
                    }
                }
                // or a DAT directive?
                else if (line.Directive == "DAT")
                {
                    // DAT fields shouldn't need to be modified
                    rec.StatusFlag = 'A';
                }
                // or an ADC directive?
                else if (line.Directive == "ADC" || line.Directive == "ADCE")
                {
                    // make the modification record
                    ModificationRecord mod = new ModificationRecord(symb.ProgramName);
                    bool worked = true;
                    string expr = line.DirectiveOperand;
                    int rel = 0;
                    int maxOp = 1;

                    // this differentiates between ADC and ADCe
                    if (line.Directive.EndsWith("E"))
                    {
                        maxOp = 3;
                    }
                    if (line.DirectiveLitOperand == OperandParser.Literal.EXPRESSION)
                    {
                        // wat do if expressions
                        worked = OperandParser.ParseExpression(ref expr, OperandParser.Expressions.ADC, line,
                                                      ref symb, mod, out rel, maxOp);
                        // catching errors
                        if (worked)
                        {
                            bin = Convert.ToString(Convert.ToInt32(expr, 16), 2);
                        }
                    }
                    else if (line.DirectiveLitOperand == OperandParser.Literal.NUMBER)
                    {
                        // wat do if just a number
                        mod.AddAdjustment(true, symb.ProgramName);
                        rel = 1;
                        bin = Convert.ToString(Convert.ToInt32(expr, 16), 2);
                    }

                    // check the range
                    int start = Convert.ToInt32(this.input.Line(1).DirectiveOperand, 16);
                    int eof = start + Convert.ToInt32(this.input.ModuleLength, 16);
                    if (Convert.ToInt32(expr, 16) > eof || Convert.ToInt32(expr, 16) < start)
                    {
                        line.AddError(Errors.Category.Serious, 38);
                        line.NOPificate();
                        bin = "0000000000000000";
                    }

                    // if there are modifications to be made, add them
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
         * Render the object file into the file given.
         *
         * @param filename The filename to write the object file to.
         * @refcode OB
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 10, 2011
         * @modlog
         *  - May 14, 2011 - Mark - Add in the load/execution addresses and module length
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
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

