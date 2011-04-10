using System;
using System.Diagnostics;

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
            get { return this.LC == null ? null : this.LC; }
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
            get { return this.lineLabel == null ? null : this.lineLabel; ; }
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
            get { return this.category == null ? null : this.category; }
            set { this.category = value; }
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
            get { return this.function == null ? null : this.function; }
            set { this.function = value; }
        }

        /**
         * Allows access for getting the bytecode for this line.
         */
        public string Bytecode
        {
            get
            {
                try
                {
                    return Instructions.GetInstance().GetBytecodeString(
                        this.category == null ? "" : this.category,
                        this.function == null ? "" : this.function
                    );
                }
                catch (InstructionException)
                {
                    return null;
                }
            }
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
            get { return this.operand == null ? null : this.operand; }
            set { this.operand = value; }
        }


        /**
         * The function's literal operand type.
         */
        private string operandLit;

        /**
         * Allows access for getting and setting the literal operand type of the
         * function in this line of source.
         */
        public string OpLitOperand
        {
            get { return this.operandLit == null ? null : this.operandLit; }
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
            get { return this.directive == null ? null : this.directive; }
            set { this.directive = value; }
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
            get { return this.dirOperand == null ? null : this.dirOperand; }
            set { this.dirOperand = value; }
        }


        /**
         * The directive's literal operand type.
         */
        private string dirLitOperand;

        /**
         * Allows access for getting and setting the directive literal operand
         * type in this line of source.
         */
        public string DirectiveLitOperand
        {
            get { return this.dirLitOperand == null ? null : this.dirLitOperand; }
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
            get { return this.comment == null ? null : this.comment; }
            set { this.comment = value; }
        }


        /**
         * Sets the source line and line number, and puts all other values to 
         * their initial values, awaiting being set if needed.
         */
        public IntermediateLine(string source, short lineNum)
        {
            Trace.WriteLine("Initializing line " + lineNum, "IntermediateLine");
            this.source = source;
            this.line = lineNum.ToString();
            this.LC = null;
            this.lineLabel = null;
            this.category = null;
            this.function = null;
            this.operand = null;
            this.operandLit = null;
            this.directive = null;
            this.dirOperand = null;
            this.dirLitOperand = null;
            this.comment = null;     
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
            Trace.WriteLine("Printing line " + this.line, "IntermediateLine");

            string format = String.Join("\n    ", new string[] {
                "{0}",
                "Line: {1}",
                "LC: {2}",
                "Label: {3}",
                "Category: {4}",
                "Function: {5}",
                "Partial Bytecode: {6}",
                "Operand: {7}",
                "Literal Operand: {8}",
                "Directive: {9}",
                "Directive Operand: {10}",
                "Directive Literal Operand: {11}",
                "Comment: {12}",
                "  --\n\n"
            });
            return String.Format(format,
                this.SourceLine,
                this.SourceLineNumber,
                this.ProgramCounter == null ? "N/A" : this.ProgramCounter,
                this.Label == null ? "N/A" : this.Label,
                this.OpCategory == null ? "N/A" : this.OpCategory,
                this.OpFunction == null ? "N/A" : this.OpFunction,
                this.Bytecode == null ? "N/A" : this.Bytecode,
                this.OpOperand == null ? "N/A" : this.OpOperand,
                this.OpLitOperand == null ? "N/A" : this.OpLitOperand,
                this.Directive == null ? "N/A" : this.Directive,
                this.DirectiveOperand == null ? "N/A" : this.DirectiveOperand,
                this.DirectiveLitOperand == null ? "N/A" : this.DirectiveLitOperand,
                this.Comment == null ? "N/A" : this.Comment
            );
        }
    }
}
