==========================
FFA Language Specification
==========================

The FFA machine is a hypothetical machine that the FFA Assembler produces code for. It has a word size of 16 bits, is word-addressable, and supports 1024 words of addressable memory (from memory address 0 to 1023). It has a separate data stack and test stack.  Data stack is limited to 256 entries (words) and is used to hold user data that has been pushed onto it. The test stack is limited to 5 entries and holds results from the ``TEST`` operation. This document describes the language specification of the machine.

.. contents::
    :depth: 2
    :backlinks: none

File Format
===========

A source file for the FFA Assembler should consist of text (ASCII) instructions, one per line. There is no set line limit, however keep in mind that there is a limit of 1024 words. So, a source file may consist of **at most** 1024 instructions and data-storage directives combined. Labels must start in column 1 of the line they are being defined on. Lines that do not include a label must have whitespace in column 1. The very first line of a file should be the start directive. The very last line must be the end directive. No comments may appear on the lines before or after these directives, however a comment may appear on the same line as the end directive. White space is allowed in the program as long as the instruction format is met, for example, whitespace may not appear between a category and operand that are seperated by a comma. Comments may appear on any line, including on a line by itself, however everything following the colon until the next line will be considered part of the comment.

Instructions
============

Each instruction occupies 16 bits of memory, or one word, on the system. There are five main instructions, each of which can perform different functions. The details of each are described in their respective sections. Instruction names and functions are case-insensitive. (This does not always apply to operands, however.)

CNTL
----

Control codes are specified with CNTL, and can be used to halt the program, debug, branch, and clear stacks. The syntax of a control code is::

    CNTL    FUNC,op

Where FUNC is any of the following functions, and op is an operand pertaining to that function. In this instruction, there is no operand for CLRD and CLRT.

CNTL HALT
~~~~~~~~~

Opcode: ``00 000``

Halt the execution of the program. Operand is an integer from 0 to 1023 indicating an exit code. If in debug mode, the simulator will print the exit code, otherwise no output will be given. Example::

    CNTL HALT,9

CNTL DUMP
~~~~~~~~~

Opcode: ``00 001``

Dump out stacks and/or memory contents; useful for debugging purposes. Operand indicates the kind of information to dump:

* 1 - Dump both stacks and status word
* 2 - Dump full program memory
* 3 - Dump items in both 1 and 2

Stack Dump Output::

    Data Stack: HHHH  Test Stack: HHHH
                HHHH              HHHH
                ....              ....

Where H is a hex digit and the values are displayed as Hex values in 16bit twos complement. The top value is the top item in the stack and the values continue downward in a column until all items in the stack have been printed.

Memory Dump Output::

    hhh  HHHH HHHH HHHH HHHH HHHH HHHH HHHH HHHH HHHH HHHH HHHH HHHH HHHH HHHH HHHH HHHH
    hhh  HHHH HHHH HHHH HHHH HHHH HHHH HHHH HHHH HHHH HHHH HHHH HHHH HHHH HHHH HHHH HHHH
    hhh  HHHH HHHH HHHH HHHH HHHH HHHH HHHH HHHH HHHH HHHH HHHH HHHH HHHH HHHH HHHH HHHH
    ...

Where both h and H are hex digits and hhh displays the LC offset starting from 0 and incrementing by 16 (decimal) each row. HHHH the hex 16bit twos comp representation of a word in memory. The address can be determined based on the offset and the column it is in. The very first word has an offset of 0 and is in column 0 so its address is 0.  If the offset is 2C and it is in column 9, the address is 35 (2C + 9).

For example, this may be a useful debugging command::

    CNTL DUMP,3

CNTL CLRD
~~~~~~~~~

Opcode: ``00 010``

Clears the data stack. No operand accepted::

    CNTL CLRD

CNTL CLRT
~~~~~~~~~

Opcode: ``00 011``

Clears the test stack. No operand accepted::

    CNTL CLRT

CNTL GOTO
~~~~~~~~~

Opcode: ``00 100``

Unconditionally branches to the specified label. To jump to label ``foo``::

    CNTL GOTO,foo

STACK
-----

This machine contains a data stack capable of storing 256 entries. It can be directly manipulated using theses ``STACK`` functions. ``STACK`` instructions can accept labels (memory references) or literals. For more information on literals, see the Literals_ section.

STACK PUSH
~~~~~~~~~~

Opcode: ``00 101``

Push an item onto the top of the stack. Example::

    STACK PUSH,FOO

would push the data at FOO on to the stack. To push a value directly::

    STACK PUSH,42

STACK POP
~~~~~~~~~

Opcode: ``00 110``

Pop an item off of the top of the stack. As an example, this would pop the top item off of the stack and store the value at the memory location given by BAR::

	STACK POP,BAR

Again, this can also be done by providing a numeric memory reference.  Using the following code would pop the first item off of the stack and store it at memory address 27::

	STACK POP,27

STACK TEST
~~~~~~~~~~

Opcode: ``00 111``

Pop a single item off of the data stack, and compare it with the given label. Depending on the results of the test, any of the following may be pushed on to the test stack:

* ``=`` - push ``0`` on the test stack
* ``<`` - push ``1`` on the test stack
* ``>`` - push ``2`` on the test stack

The results of the values pushed into the test stack are useful for branching. For more information, see the JUMP_ command. Usage example::

    STACK TEST,DIRT

Here, the top value of the stack would be compared with the memory referenced by DIRT. Alternatively, literals may also be used::

    STACK TEST,B=010010

JUMP
----

Jump to the specified location if a given condition holds, and pop the test off of the test stack. This instruction operates on data in the test stack (with the exception of ``dnull``), so to do anything useful `STACK TEST`_ should probably be used first. The available tests are:

* ``=`` - if ``0`` was on the test stack. (Opcode: ``01 000``)
* ``<`` - if ``1`` was on the test stack. (Opcode: ``01 010``)
* ``>`` - if ``2`` was on the test stack. (Opcode: ``01 011``)
* ``tnull`` - if the test stack is empty. (Opcode: ``01 110``)
* ``dnull`` - if the data stack is empty. This is the only test that doesn't use the test stack. (Opcode: ``01 111``)

SOPER
-----

``SOPER`` instructions act on items in the data stack, and don't interact with main program memory. Operations exist to add, subtract, multiply, divide, logical "or" & "and", and basic I/O. In the context of this instruction, ``nnn`` or "any number of items" means an integer from 0 to 255.

SOPER ADD
~~~~~~~~~

Opcode: ``10 000``

Pops any number of items off of the stack and adds them together. Pushes the result on the top of the stack::

   SOPER ADD,3

If the stack was [4, 6, 10], then this instruction would result in the stack being [20], since 4 + 6 + 10 = 20.

SOPER SUB
~~~~~~~~~

Opcode: ``10 001``

Pops any number of items off of the stack, subtracts them in the order they were in the stack, and pushes the result::

    SOPER ADD,2

SOPER MUL
~~~~~~~~~

Opcode: ``10 010``

Pops any number of items off of the stack, multiplies them, and pushes the result::

    SOPER MUL,4

SOPER DIV
~~~~~~~~~

Opcode: ``10 011``

Pops any number of items off of the stack, divides them in order, and pushes the result::

    SOPER DIV,7

SOPER OR
~~~~~~~~

Opcode: ``10 100``

Pops any number of items off of the stack, performs a logical ``OR`` between them, and pushes the result::

    SOPER OR,12

SOPER AND
~~~~~~~~~

Opcode: ``10 101``

Pops any number of items off of the stack, performs a logical ``AND`` between them, and pushes the result::

    SOPER AND,4

SOPER READN
~~~~~~~~~~~

Opcode: ``10 110 0``

Reads an integer from the active input ``nnn`` number of times and pushes all of them onto the stack::

    SOPER READN,25

This would read 25 integers, and push them onto the stack in the order they were received.

SOPER READC
~~~~~~~~~~~

Opcode: ``10 110 1``

Reads ``nnn`` characters from the active input and pushes them onto the stack::

    SOPER READC,210

SOPER WRITEN
~~~~~~~~~~~~

Opcode: ``10 111 0``

Pops ``nnn`` integers off of the stack and writes them to the active output (screen)::

    SOPER WRITEN,8

This would print out the top 8 items off of the stack as integers.

SOPER WRITEC
~~~~~~~~~~~~

Opcode: ``10 111 1``

Pops ``nnn`` characters off of the stack and writes them to the active output::

    SOPER WRITEC,127

MOPER
-----

``MOPER`` instructions act much like SOPER_ instructions, but act on items in memory in addition to the data stack (compared to SOPER_, which acts solely on the stack). The operand for a MOPER operation is always a label.

MOPER ADD
~~~~~~~~~

Opcode: ``11 000``

Pops the top item off of the data stack and adds it with the data at the referenced memory location. Pushes the result onto the stack::

    MOPER ADD,foo

If the top item on the stack was 5 and the data at ``foo`` was 20, then the stack would then have 25 as a result on top.

MOPER SUB
~~~~~~~~~

Opcode: ``11 001``

Pops the top item off of the stack, and subtracts the data at the referenced memory location from it. Pushes the result on the top of the stack::

    MOPER SUB,bar

MOPER MUL
~~~~~~~~~

Opcode: ``11 010``

Pops off the top item off of the stack, multiplies it with the data at the referenced memory location, and pushes the result back onto the stack::

    MOPER MUL,dirt

MOPER DIV
~~~~~~~~~

Opcode: ``11 011``

Pops the top item off of the stack, divides it by the data at the referenced memory location, and pushes the result back onto the stack::

    MOPER DIV,foo

MOPER OR
~~~~~~~~

Opcode: ``11 100``

Pops the top item off of the stack and performs a logical ``OR`` with the data at the referenced memory location, pushing the result back onto the stack::

    MOPER OR,testing

MOPER AND
~~~~~~~~~

Opcode: ``11 101``

Pops the top item off of the stack and performs a logical ``AND`` with the data at the referenced memory location, pushing the result back onto the stack::

    MOPER AND,Orange

MOPER READN
~~~~~~~~~~~

Opcode: ``11 110 0``

Reads a single integer from the active input and stores it at the referenced memory location. In addition, it pushes the integer onto the stack::

    MOPER READN,myint

MOPER READC
~~~~~~~~~~~

Opcode: ``11 110 1``

Reads a single character from the active input and stores it at the referenced memory location. In addition, it pushes the character onto the stack::

    MOPER READC,mychar

MOPER WRITEN
~~~~~~~~~~~~

Opcode: ``11 111 0``

Writes the data at the referenced memory location as an integer to the active output::

    MOPER WRITEN,saveint

MOPER WRITEC
~~~~~~~~~~~~

Opcode: ``11 111 1``

Writes the data at the referenced memory location as a character to the active output::

    MOPER WRITEC,savechar

Literals
========

Literals may be used in two situations:

* As an operand for the STACK_ instruction
* To set data values with the DAT_ directive

They may be specified as integers, hexadecimal values, in binary, or as characters. By default, if not specified, the assumed data type is an integer.

Integer
-------

Integers, when used as literals, are specified using any of the following syntax:

* ``I=123``
* ``I=+123``
* ``123``
* ``I=-123``

The first three items in the above list are the same value, just represented differently. The last item is simply a negative value. Note that if the ``I=`` prefix is not specified, an integer is assumed.

Hexadecimal
-----------

Hexadecimal values are specified with the ``X=`` prefix, for example: ``X=1F``. Hex numbers cannot be given a negative sign.  Negative numbers should be given in two's complement notation.

Binary
------

Binary values are specified with a ``B=`` prefix, as in: ``B=0101010``. As with hex, binary value should be given in two's complement notation.

Character
---------

Character values are prefixed with a ``C=`` and surrounded by single quotes. Examples:

* ``C='a'`` (for a STACK_ instruction)
* ``C='ab'`` (DAT_ directives can hold two characters in 16 bits)

Directives
==========

Directives are processed by the assembler and don't directly generate code. Like instructions, they are case insensitive.

START
-----

Format::

	Label | start | 0 - 1023

The start directive signifies the beginning of the program.  It must appear in the first line of the input program file.  The start directive is also used to set the starting location counter.  It must be provided a number (cannot use labels) that is within the range of memory, 0 - 1023.

Example::

	PRGRM2 start 0

RESET
-----

Format::

	Label | reset | new LC

Reset will alter the LC to the given value. The new LC must be larger than the LC of the reset.  For example, if the reset is called at LC 23, the new LC must be greater than 23.  The new value can be given as a number within the range of memory (0 - 1023), or an equated label.  If using an equated label, it must be defined previously in the program. Providing a label in the operand that is defined later in the program or externally will give an error.

Example::

	DATA reset 30     : called at LC 12 (hex), sets LC to 1E (30 in hex)

EQU
---

Format::

	Label | equ | 0 - 1023, another equated label, or star notation

Equate allows the user to set a label to the a value between 0 and 1023. If provided a label rather than a number, the label must have been previously equated. Accepts star notation (as do instructions) but is limited to equated symbols that have been previously defined in the source.

Example::

	MUD EQU 512
	DIRT EQU MUD

EQUE
----

Format::

	Label | eque | expression

Like EQU_, but accepts up to 3 operations (4 operands) in an expression. Again, star notation is accepted.

Example::

	X1 EQUe 5-2+DIRT

ENTRY
-----

Format::

	ENTRY | Label

Defines a shared variable name.  This defined entry label must appear somewhere in this program and can then be used as an operand by other programs. Since this directive does not start with a label, it cannot start in column 1.

Example::

	 ENTRY ReturnValue
	ReturnValue DAT X=FF

EXTRN
-----

Format::

	EXTRN | Label

Declares a symbol that receives its value from another program. The extrn label defined must not appear as a label in this program.  The label must have a matching ``ENTRY`` in another program. Since this directive does not start with a label, it cannot start in column 1.

Example::

	 EXTRN ReturnValue
	STACK PUSH,ReturnValue

END
---

Format::

	END | Label
	
End signifies to the assembler that all input has been processed.  Any lines after end will generate a warning. The label should be the program name and must match the label given in the ``START`` directive. Since this directive does not start with a label, it cannot start in column 1.

Example::

	 END PRGRM2

DAT
---

Format::

	Optional Label | DAT | Literal

Creates one word of storage (16 bits) storing the value given by the literal.

Example::

	AB DAT X=15A9
	CD DAT I=111

ADC
---

Format::

	Optional Label | ADC | label, 0-1023, equated label, or star notation

Defines an address constant. A word of storage is reserved, and is set to the address of the given label, numeric constant, or equated symbol. Accepts star notation, as long as the resulting evaluated expression is within the range of 0-1023. Example::

    test ADC *+3

ADCE
----

Format::

    Optional Label | ADCE | expression

Like ADC_, but allows up to 3 operations (4 operands). The expression must evaluate to a value in the range of 0-1023. Example::

    foo    DAT C='ab'
    orange ADCe 5+foo+3

NOP
---

Format::

	 NOP

NOP can be used to waste a machine cycle without affecting anything. A NOP is accomplished by doing a SOPER ADD,0. Invalid lines found during assembly that were meant to consume memory will be replaced with NOP in order to keep the amount of memory consumed the same but still providing working code.

Example::

	 STACK PUSH,100
	 NOP

Other
=====

Comments
--------

Comments can be used at any point on a line. A comment must begin with a colon (:) and will continue until the line ends.  All text within a comment will be ignored including possible valid code.  Code the appears on a line before a comment will still be processed.

Example::

	JUMP =,done :jump to the end when equal to 0

Star Notation
-------------

A star (*) used in the operand field refers to the current location counter. This can be used in an expression along with numbers, as long as the resulting value is within the range of the program (0 to 1023).

Example::

	CNTL GOTO,*+10

Expressions
-----------

Expressions can be used as an operand in any of the instructions or directives where an equated label or number is also accepted. Expressions are usually limited to one operator except in the case of EQUe and ADCe where up to three operators are allowed.  Operators are limited to plus (+) and minus (-). Expressions can be created using star notation (which must appear only once and at the beginning of the expression if used), numbers and labels. If given an equated label, the label will be replaced with its value. If given a regular label, it will be substituted with the location counter of that label. Negative numbers are not allowed in the expression as this would be considered an additional operator and will be considered bad expression syntax.  For example do not write 8+-4, instead use 8-4.
