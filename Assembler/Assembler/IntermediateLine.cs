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
    class IntermediateLine
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
        private string line;

        /**
         * Allows access for getting the line number of this source line.
         */
        public string SourceLineNumber
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
            set { this.function = value; }
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
        }

        /**
         * The linker hint
         */
        private char arm;

        public char LinkerHint
        {
            get { return this.arm; }
        }

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
            Logger288.Log("Initializing line " + lineNum, "IntermediateLine");
            this.source = source;
            this.line = lineNum.ToString();
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
            this.arm = 'a';
            this.errors = new List<Errors.Error>();
        }

        /**
         * Process a line, generating partial bytecode and appropriate flags.
         * Errors may be set on lines (and NOP'd) as they occur.
         *
         * @refcode
         * @errtest
         * @errmsg
         * @author Jacob
         * @creation April 10, 2011
         * @modlog
         *  - April 15, 2011 - Jacob - Converted to a property; begin generating full bytecode
         *  - April 15, 2011 - Jacob - Changed to ProcessLine; we'll do more general things here
         * @codestandard Mark
         * @teststandard Andrew
         */
        public void ProcessLine()
        {
            Logger288.Log("Processing line \"" + this.source + "\"", "IntermediateLine");
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
                return; // TODO: set a NOP
            }

            // from here on, everything is instruction-dependent
            // TODO: process equated symbols? might do this earlier.
            switch (this.category)
            {
                case "CNTL": {
                    // TODO: rework the logic here, this is a little nasty.
                    // unused bit
                    code.Append("0");
                    // literal operand
                    if (this.OpLitOperand != OperandParser.Literal.Number)
                    {
                        code.Append(BinaryHelper.BinaryString(this.OpOperand));
                    }
                    // otherwise pad with zeros (labels will have to be looked up later)
                    else if (this.OpLitOperand != OperandParser.Literal.NONE)
                    {
                        code.Append("0000000000");
                    }
                    else
                    {
                        // TODO: error
                    }
                } break;
                case "STACK": {
                    // literal operand
                    if (this.OpLitOperand != OperandParser.Literal.NONE)
                    {
                        // literal flag
                        code.Append("1");
                        // and the value
                        code.Append(BinaryHelper.BinaryString(this.OpOperand));
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
                    // unused bit
                    code.Append("0");
                    // reference label, filled in pass 2
                    code.Append("0000000000");
                } break;
                case "SOPER": {
                    // the write flag is only set for character operations
                    if (this.OpOperand == "READC" || this.OpOperand == "WRITEC")
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
                    code.Append(BinaryHelper.BinaryString(this.OpOperand));
                } break;
                case "MOPER": {
                    // again, the write flag is only set for character operations
                    if (this.OpOperand == "READC" || this.OpOperand == "WRITEC")
                    {
                        code.Append("1");
                    }
                    // for everything else, it's a zero
                    else
                    {
                        code.Append("0");
                    }
                    // reference label, filled pass 2
                    code.Append("0000000000");
                } break;
            }

            // set the bytecode
            this.bytecode = code.ToString();
        }

        /**
         * TODO: DOCUMENT
         */
        public void AddError(Errors.Category level, string msg)
        {
            Errors.Error err;
            err.category = level;
            err.msg = msg;
            this.errors.Add(err);
        }

        /**
         * TODO: DOCUMENT
         */
        public List<Errors.Error> GetThreeErrors()
        {
            List<Errors.Error> result = new List<Errors.Error>();
            for (int i = 0; i < 3; i++)
            {
                if (this.errors.Count == i)
                {
                    break;
                }
                result.Add(this.errors[i]);
            }
            return result;
        }

        /**
         * Displays the line of source and its individual parts. The format of this is:<br />
         * [SourceLine]<br />
         * Line: [SourceLineNumber]<br />
         * LC: [ProgramCounter]<br />
         * Label: [Label]<br />
         * Category: [OpCategory]<br />
         * Function: [OpFunction]<br />
         * Operand: [OpOperand]<br />
         * Literal Operand: [OpLitOperand]<br />
         * Directive: [Directive]<br />
         * Directive Operand: [DirectiveOperand]<br />
         * Comment: [Comment]
         * 
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
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public override string ToString()
        {
            Logger288.Log("Printing line " + this.line, "IntermediateLine");

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
            return output + "\n";
        }
    }
}
