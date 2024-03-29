﻿using System;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;

namespace Assembler
{
    /**
     * The value of each line of the source code when it has been converted into the
     * intermediate values between pass 1 and pass 2.
     */
    public class IntermediateLine
    {
        /**
         * The full source line.
         */
        private string source;

        /**
         * Allows access for getting the full source line in this object.
         */
        public string SourceLine
        {
            get { return this.source; }
        }


        /**
         * The line number of this line of source.
         */
        private short line;

        /**
         * Allows access for getting the line number of this source line.
         */
        public short SourceLineNumber
        {
            get { return this.line; }
        }


        /**
         * The location counter this line will be assigned.
         */
        private string LC;

        /**
         * Allows access for getting and setting the program counter value
         * associated with this source line.
         */
        public string ProgramCounter
        {
            get { return this.LC; }
            set { this.LC = value; }
        }


        /**
         * The label, if any, assigned to this line.
         */
        private string lineLabel;

        /**
         * Allows access for getting and setting the label assigned
         * to this source line.
         */
        public string Label
        {
            get { return this.lineLabel; }
            set { this.lineLabel = value; }
        }


        /**
         * The category of the instruction, if an instruction is on this line.
         */
        private string category;

        /**
         * Allows access for getting and setting the category of the function
         * in this line of source.
         */
        public string OpCategory
        {
            get { return this.category; }
            set { this.category = value.ToUpper(); }
        }


        /**
         * The function name on this line, if it exists.
         */
        private string function;

        /**
         * Allows access for getting and setting the function name in this line
         * of source.
         */
        public string OpFunction
        {
            get { return this.function; }
            set { this.function = value.ToUpper(); }
        }


        /**
         * The function's operand.
         */
        private string operand;

        /**
         * Allows access for getting and setting the operand of the function
         * in this line of source.
         */
        public string OpOperand
        {
            get { return this.operand; }
            set { this.operand = value; }
        }


        /**
         * The function's literal operand type.
         */
        private OperandParser.Literal operandLit;

        /**
         * Allows access for getting and setting the literal operand type of the
         * function in this line of source.
         */
        public OperandParser.Literal OpLitOperand
        {
            get { return this.operandLit; }
            set { this.operandLit = value; }
        }


        /**
         * The directive name on this line, if it exists.
         */
        private string directive;

        /**
         * Allows access for getting and setting the directive name in this
         * line of source.
         */
        public string Directive
        {
            get { return this.directive; }
            set { this.directive = value.ToUpper(); }
        }

        
        /**
         * The directive's operand.
         */
        private string dirOperand;

        /**
         * Allows access for getting and setting the directive operand in this
         * line of source.
         */
        public string DirectiveOperand
        {
            get { return this.dirOperand; }
            set { this.dirOperand = value; }
        }


        /**
         * The directive's literal operand type.
         */
        private OperandParser.Literal dirLitOperand;

        /**
         * Allows access for getting and setting the directive literal operand
         * type in this line of source.
         */
        public OperandParser.Literal DirectiveLitOperand
        {
            get { return this.dirLitOperand; }
            set { this.dirLitOperand = value; }
        }


        /**
         * Any comment at the end of the line.
         */
        private string comment;

        /**
         * Allows access for getting and setting the comment at the end of this
         * line of source.
         */
        public string Comment
        {
            get { return this.comment; }
            set { this.comment = value; }
        }

        /**
         * The bytecode of the line
         */
        private string bytecode;

        /**
         * Access to the generated bytecode (read-only)
         */
        public string Bytecode
        {
            get { return this.bytecode; }
            set { this.bytecode = value; }
        }

        /**
         * The original equated label if used
         */
        public string Equated { get; private set; }

        /**
         * Whether this line has any fatal errors on it.
         */
        public bool Fatal { get; private set; }

        /**
         * List of errors that are flagged on this line.
         */
        private List<Errors.Error> errors;


        /**
         * Sets the source line and line number, and puts all other values to 
         * their initial values, awaiting being set if needed.
         */
        public IntermediateLine(string source, short lineNum)
        {
            Logger.Log("Initializing line " + lineNum, "IntermediateLine");
            this.source = source;
            this.line = lineNum;
            this.LC = null;
            this.lineLabel = null;
            this.category = null;
            this.function = null;
            this.operand = null;
            this.operandLit = OperandParser.Literal.NONE;
            this.directive = null;
            this.dirOperand = null;
            this.dirLitOperand = OperandParser.Literal.NONE;
            this.comment = null;
            this.bytecode = null;
            this.Equated = null;
            this.Fatal = false;
            this.errors = new List<Errors.Error>();
        }

        /**
         * Process a line, generating partial bytecode and appropriate flags.
         * Errors may be set on lines (and NOP'd) as they occur.
         *
         * @param symb symbol table reference
         *
         * @refcode OP
         * @errtest
         *  N/A
         * @errmsg
         *  ES.3, ES.4, ES.9, ES.11, ES.12, ES.16, EW.3
         * @author Jacob Peddicord
         * @creation April 10, 2011
         * @modlog
         *  - April 15, 2011 - Jacob - Converted to a property; begin generating full bytecode
         *  - April 15, 2011 - Jacob - Changed to ProcessLine; we'll do more general things here
         *  - April 16, 2011 - Jacob - Fix padding on generated instructions.
         *  - April 17, 2011 - Jacob - Handle error conditions.
         * @codestandard Mark Mathis
         * @teststandard Andrew Buelow
         */
        public void ProcessLine(ref SymbolTable symb)
        {
            Logger.Log("Processing line \"" + this.source + "\"", "IntermediateLine");
            // get the first 5 bits
            StringBuilder code = new StringBuilder();
            try
            {
                code.Append(Instructions.GetInstance().GetBytecodeString(
                    this.category == null ? "" : this.category,
                    this.function == null ? "" : this.function
                ));
            }
            catch (InstructionException)
            {
                this.AddError(Errors.Category.Serious, 3);
                this.NOPificate();
                return;
            }

            // fetch equated symbols
            if (this.OpLitOperand == OperandParser.Literal.NONE &&
                this.OpOperand != null &&
                symb.ContainsSymbol(this.OpOperand) &&
                symb.GetSymbol(this.OpOperand).usage == Usage.EQUATED)
            {
                Logger.Log("Evaluating equated symbol " + this.OpOperand, "IntermediateLine");
                this.Equated = this.OpOperand;
                this.OpOperand = symb.GetSymbol(this.OpOperand).val;
            }

            // from here on, everything is instruction-dependent
            switch (this.category)
            {
                case "CNTL": {
                    // unused bit
                    code.Append("0");
                    // validation
                    if (this.function == "HALT")
                    {
                        // we want a number
                        if (this.OpLitOperand == OperandParser.Literal.NUMBER)
                        {
                            int val = BinaryHelper.HexToInt(this.OpOperand, 16);
                            // out of bounds
                            if (val < 0 || val > 1023)
                            {
                                this.AddError(Errors.Category.Serious, 12);
                                this.NOPificate();
                                return;
                            }
                        }
                        // wrong operand type
                        else
                        {
                            this.AddError(Errors.Category.Serious, 12);
                            this.NOPificate();
                            return;
                        }
                    }
                    else if (this.function == "DUMP")
                    {
                        // not a 1, 2, or 3
                        if (this.OpOperand != "1" && this.OpOperand != "2" && this.OpOperand != "3")
                        {
                            this.AddError(Errors.Category.Serious, 11);
                            this.NOPificate();
                            return;
                        }
                    }
                    else if (this.function == "CLRD" || this.function == "CLRT")
                    {
                        // no operand for CLRD/CLRT
                        if (this.OpOperand != "" && this.OpOperand != null)
                        {
                            this.AddError(Errors.Category.Warning, 3);
                            this.NOPificate();
                            return;
                        }
                    }
                    else if (this.function == "GOTO")
                    {
                        // make sure there's a label or expression
                        if ((this.OpLitOperand != OperandParser.Literal.EXPRESSION &&
                            this.OpLitOperand != OperandParser.Literal.NONE) ||
                            this.OpOperand == "" || this.OpOperand == null)
                        {
                            this.AddError(Errors.Category.Serious, 9);
                            this.NOPificate();
                            return;
                        }
                    }
                    // actual processing!
                    // numeric operand
                    if (this.OpLitOperand == OperandParser.Literal.NUMBER)
                    {
                        code.Append(BinaryHelper.BinaryString(this.OpOperand).PadLeft(10, '0'));
                    }
                    // otherwise pad with zeros (labels & expressions in pass 2)
                    else
                    {
                        code.Append("0000000000");
                    }
                } break;
                case "STACK": {
                    // ensure there is an operand
                    if (this.OpOperand.Length == 0)
                    {
                        this.AddError(Errors.Category.Serious, 16);
                        this.NOPificate();
                        return;
                    }
                    // literal operand
                    if (this.OpLitOperand != OperandParser.Literal.NONE &&
                        this.OpLitOperand != OperandParser.Literal.EXPRESSION)
                    {
                        // bounds-check
                        int val = Convert.ToInt32(this.OpOperand, 16);
                        if (val < 0 || val > 1023) // actually (val < -512 || val > 511) in 10 bits
                        {
                            this.AddError(Errors.Category.Serious, 4);
                            this.NOPificate();
                            return;
                        }
                        // literal flag
                        code.Append("1");
                        // and the value
                        code.Append(BinaryHelper.BinaryString(this.OpOperand).PadLeft(10, '0'));
                    }
                    // expression
                    else if (this.OpLitOperand == OperandParser.Literal.EXPRESSION)
                    {
                        // we'll have literal data once the expression is evaluated.
                        code.Append("1");
                        code.Append("0000000000");
                    }
                    // label
                    else
                    {
                        // non-literal (reference) bit
                        code.Append("0");
                        // and the reference itself, which will be filled in pass 2
                        code.Append("0000000000");
                    }
                } break;
                case "JUMP": {
                    // ensure there is an operand
                    if (this.OpOperand.Length == 0)
                    {
                        this.AddError(Errors.Category.Serious, 16);
                        this.NOPificate();
                        return;
                    }
                    // unused bit
                    code.Append("0");
                    // ensure that JUMP is taking a label
                    if (this.OpLitOperand == OperandParser.Literal.NONE)
                    {
                        // reference label, filled in pass 2
                        code.Append("0000000000");
                    }
                    // or a number
                    else if (this.OpLitOperand == OperandParser.Literal.NUMBER)
                    {
                        int val = Convert.ToInt32(this.OpOperand, 16);
                        if (val < 0 || val > 1023)
                        {
                            this.AddError(Errors.Category.Serious, 12);
                            this.NOPificate();
                            return;
                        }
                        code.Append(BinaryHelper.BinaryString(this.OpOperand).PadLeft(10, '0'));
                    }
                    // or an expression (processed later)
                    else if (this.OpLitOperand == OperandParser.Literal.EXPRESSION)
                    {
                        code.Append("0000000000");
                    }
                    // otherwise, error
                    else
                    {
                        this.AddError(Errors.Category.Serious, 9);
                        this.NOPificate();
                        return;
                    }
                } break;
                case "SOPER": {
                    // ensure there is an operand of type Number
                    if (this.OpLitOperand != OperandParser.Literal.NUMBER)
                    {
                        this.AddError(Errors.Category.Serious, 17);
                        this.NOPificate();
                        return;
                    }
                    // bounds-check
                    int val = Convert.ToInt32(this.OpOperand, 16);
                    if (val < 0 || val > 255) // will catch 2's complement negatives as well
                    {
                        this.AddError(Errors.Category.Serious, 17);
                        this.NOPificate();
                        return;
                    }
                    // the write flag is only set for character operations
                    if (this.OpFunction == "READC" || this.OpFunction == "WRITEC")
                    {
                        code.Append("1");
                    }
                    // for everything else, it's a zero
                    else
                    {
                        code.Append("0");
                    }
                    // unused 2 bits
                    code.Append("00");
                    // operand (assumed literal)
                    code.Append(BinaryHelper.BinaryString(this.OpOperand).PadLeft(8, '0'));
                } break;
                case "MOPER": {
                    // ensure there is an operand
                    if (this.OpOperand.Length == 0)
                    {
                        this.AddError(Errors.Category.Serious, 16);
                        this.NOPificate();
                        return;
                    }
                    // the write flag is only set for character operations
                    if (this.OpFunction == "READC" || this.OpFunction == "WRITEC")
                    {
                        code.Append("1");
                    }
                    // for everything else, it's a zero
                    else
                    {
                        code.Append("0");
                    }
                    // ensure that MOPER is taking a label
                    if (this.OpLitOperand == OperandParser.Literal.NONE)
                    {
                        // reference label, filled pass 2
                        code.Append("0000000000");
                    }
                    // or a number
                    else if (this.OpLitOperand == OperandParser.Literal.NUMBER)
                    {
                        int val = Convert.ToInt32(this.OpOperand, 16);
                        if (val < 0 || val > 1023)
                        {
                            this.AddError(Errors.Category.Serious, 4);
                            this.NOPificate();
                            return;
                        }
                        code.Append(BinaryHelper.BinaryString(this.OpOperand).PadLeft(10, '0'));
                    }
                    // or an expression (pass 2)
                    else if (this.OpLitOperand == OperandParser.Literal.EXPRESSION)
                    {
                        code.Append("0000000000");
                    }
                    // otherwise, it's invalid
                    else
                    {
                        this.AddError(Errors.Category.Serious, 9);
                        this.NOPificate();
                        return;
                    }
                } break;
            }

            // set the bytecode
            this.bytecode = code.ToString();
        }

        /**
         * Add an error to this line.
         *
         * @param level the severity of the error
         * @param code code of the error to store
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Jacob
         * @creation April 15, 2011
         * @modlog
         *  - April 16, 2011 - Jacob - Changed to use an Error struct.
         *  - April 17, 2011 - Jacob - Look up an error code instead of a message.
         *  - April 17, 2011 - Andrew - Added logging to this procedure.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public void AddError(Errors.Category level, int code)
        {
            Logger.Log("Found " + level.ToString() + " error number " + code + ". Adding to error list.", "IntermediateLine");

            Parser.TotalErrors++;

            Errors inst = Errors.GetInstance();

            // Get the correct error based on category and add to the list of errors for this line.
            if (level == Errors.Category.Warning)
            {
                this.errors.Add(inst.GetWarningError(code));
            }
            else if (level == Errors.Category.Serious)
            {
                this.errors.Add(inst.GetSeriousError(code));
            }
            else if (level == Errors.Category.Fatal)
            {
                this.errors.Add(inst.GetFatalError(code));
                this.Fatal = true;
            }
        }

        /**
         * Get the first three errors on this line. If there are more, they
         * are ignored.
         *
         * @return the first three errors associated with this line
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Jacob
         * @creation April 15, 2011
         * @modlog
         *  - April 16, 2011 - Jacob - Changed to use an Error struct.
         *  - April 17, 2011 - Andrew - Made the loop condition better defined to avoid using breaks.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public List<Errors.Error> GetThreeErrors()
        {
            List<Errors.Error> result = new List<Errors.Error>();
            for (int i = 0; i < Math.Min(this.errors.Count, 3); i++)
            {
                result.Add(this.errors[i]);
            }
            return result;
        }

        /**
         * Converts the current line to a NOP (SOPER ADD,0). Used when an error
         * is throws of severity level serious or higher and the line is
         * invalidated. The corresponding bytecode for a NOP is
         * 1000000000000000, which is equivalent to that of a SOPER ADD,0.
         * Labels and comments will still be preserved, but instructions and
         * directives will not.
         *
         * @refcode D10
         * @errtest
         * @errmsg
         * @author Jacob
         * @creation April 17, 2011
         * @modlog
         *  - April 17, 2011 - Andrew - Removed the last line which also cleared the list of errors.
         *      This caused error output to be blank even though there may have been errors present.
         * @teststandard Andrew
         * @codestandard Mark
         */
        public void NOPificate()
        {
            Logger.Log("Invalidating line " + this.line, "IntermediateLine");
            this.bytecode = "1000000000000000";
            this.category = "SOPER";
            this.function = "ADD";
            this.operand = "0";
            this.operandLit = OperandParser.Literal.NUMBER;
            this.directive = null;
            this.dirOperand = null;
            this.dirLitOperand = OperandParser.Literal.NONE;
            this.Equated = null;
        }

        /**
         * Displays the line of source and its individual parts. Used in
         * pretty-printing, and isn't useful for internal processing.
         *
         * @return the pretty-printed representation of this line
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
         * @creation April 8, 2011
         * @modlog
         *  - April 8, 2011 -  Mark - ToString prints out IntermediateLine.
         *  - April 8, 2011 -  Mark - Changed separators between values from tabs to newlines.
         *  - April 9, 2011 -  Mark - Added a spot for literal directive operands in the output.
         *  - April 9, 2011 -  Mark - Added a newline to the end of the output.
         *  - April 9, 2011 - Jacob - Made the code pretty.
         *  - April 10, 2011 - Jacob - Output partial bytecode.
         *  - April 17, 2011 - Andrew - Added errors to the output.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public override string ToString()
        {
            Logger.Log("Printing line " + this.line, "IntermediateLine");

            string output = String.Format("{0}" +
                                          "\n\tLine: {1,-29} LC: {2}" +
                                          "\n\tLabel: {3}" +
                                          "\n\tPartial Bytecode: {4}",
                this.SourceLine, this.SourceLineNumber, this.ProgramCounter, this.Label, this.Bytecode);
            // append instruction if applicable
            if (this.OpCategory != null)
            {
                output += String.Format("\n\tCategory: {0,-25} Function: {1}" +
                                        "\n\tOperand: {2,-26} (Literal: {3})",
                                        this.OpCategory, this.OpFunction, this.OpOperand, this.OpLitOperand);
            }
            // or a directive
            else if (this.Directive != null)
            {
                output += String.Format("\n\tDirective: {0}" +
                                        "\n\tDirective Operand: {1,-16} (Literal: {2})",
                                        this.Directive, this.DirectiveOperand, this.DirectiveLitOperand);
            }
            // show a comment if present
            if (this.Comment != null)
            {
                output += String.Format("\n\tComment: {0}", this.Comment);
            }
            // display any errors that may have been found in this line
            List<Errors.Error> lineErrors = this.GetThreeErrors();

            if (this.errors.Count > 0)
            {
                output += "\n\tErrors:\n";

                foreach (Errors.Error i in lineErrors)
                {
                    output += String.Format("\t {0}\n", i.ToString());
                }
            }

            return output + "\n";
        }
    }
}
