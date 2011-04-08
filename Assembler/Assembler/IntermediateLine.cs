using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
         * The line number of this line of source.
         */
        private string line;

        /**
         * The location counter this line will be assigned.
         */
        private string LC;

        /**
         * The label, if any, assigned to this line.
         */
        private string lineLabel;

        /**
         * The category of the instruction, if an instruction is on this line.
         */
        private string category;

        /**
         * The function name on this line, if it exists.
         */
        private string function;

        /**
         * The function's operand.
         */
        private string operand;

        /**
         * The function's literal operand, if it exists.
         */
        private string operandLit;

        /**
         * The directive name on this line, if it exists.
         */
        private string directive;
        
        /**
         * The directive's operand.
         */
        private string dirOperand;

        /**
         * Any comment at the end of the line.
         */
        private string comment;

        /**
         * Sets the source line and line number, and puts all other values to 
         * their initial values, awaiting being set if needed.
         */
        public IntermediateLine(string source, short lineNum)
        {
            Trace.WriteLine(String.Format("{0} -> {1}",
                                           System.DateTime.Now, 
                                           "Initializing line " + lineNum), "IntermediateLine");
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
            this.comment = null;     
        }

        /**
         * Allows access for getting the full source line in this object.
         */
        public string SourceLine
        {
            get { return this.source; }
        }

        /**
         * Allows access for getting the line number of this source line.
         */
        public string SourceLineNumber
        {
            get { return this.line; }
        }

        /**
         * Allows access for getting and setting the program counter value
         * associated with this source line.
         */
        public string ProgramCounter
        {
            get { return this.LC == null ? "N/A" : this.LC; }
            set { this.LC = value; }
        }

        /**
         * Allows access for getting and setting the label assigned
         * to this source line.
         */
        public string Label
        {
            get { return this.lineLabel == null ? "N/A" : this.lineLabel; ; }
            set { this.lineLabel = value; }
        }

        /**
         * Allows access for getting and setting the category of the function
         * in this line of source.
         */
        public string OpCategory
        {
            get { return this.category == null ? "N/A" : this.category; }
            set { this.category = value; }
        }

        /**
         * Allows access for getting and setting the function name in this line
         * of source.
         */
        public string OpFunction
        {
            get { return this.function == null ? "N/A" : this.function; }
            set { this.function = value; }
        }

        /**
         * Allows access for getting and setting the operand of the function
         * in this line of source.
         */
        public string OpOperand
        {
            get { return this.operand == null ? "N/A" : this.operand; }
            set { this.operand = value; }
        }

        /**
         * Allows access for getting and setting the literal operand of the
         * function in this line of source.
         */
        public string OpLitOperand
        {
            get { return this.operandLit == null ? "N/A" : this.operandLit; }
            set { this.operandLit = value; }
        }

        /**
         * Allows access for getting and setting the directive name in this
         * line of source.
         */
        public string Directive
        {
            get { return this.directive == null ? "N/A" : this.directive; }
            set { this.directive = value; }
        }

        /**
         * Allows access for getting and setting the directive operand in this
         * line of source.
         */
        public string DirectiveOperand
        {
            get { return this.dirOperand == null ? "N/A" : this.dirOperand; }
            set { this.dirOperand = value; }
        }

        /**
         * Allows access for getting and setting the comment at the end of this
         * line of source.
         */
        public string Comment
        {
            get { return this.comment == null ? "N/A" : this.comment; }
            set { this.comment = value; }
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
         */
        public override string ToString()
        {
            Trace.WriteLine(String.Format("{0} -> {1}", 
                                           System.DateTime.Now, 
                                           "Printing line " + this.line), "IntermediateLine");

            string format = "{1}\nLine: {2}{0}LC: {3}{0}Label: {4}{0}Category: {5}{0}Function: " +
            "{6}{0}Operand: {7}{0}Literal Operand: {8}{0}Directive: {9}{0}Directive Operand:{10}{0}Comment: {11}";
            string seperator = "\n";
            return String.Format(format, seperator, this.SourceLine, this.SourceLineNumber, this.ProgramCounter,
                this.Label, this.OpCategory, this.OpFunction, this.OpOperand, this.OpLitOperand,
                this.Directive, this.DirectiveOperand, this.Comment);
        }
    }
}
