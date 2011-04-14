==========================
FFA Language Specification
==========================

The FFA machine is a hypothetical machine that the FFA Assembler produces code for. It has a word size of 16 bits, is word-addressable, and supports 1024 words of addressable memory. It has a separate data stack and test stack. This document describes the language specification of the machine.

Instructions
============

Each instruction occupies 16 bits of memory, or one word, on the system. There are five main instructions, each of which can perform different functions. The details of each are described in their respective sections.

CNTL
----

Control codes are specified with CNTL, and can be used to halt the program, debug, branch, and clear stacks. The syntax of a control code is::

    CNTL    FUNC,op

Where FUNC is any of the following functions, and op is an operand pertaining to that function. In this instruction, there is no operand for CLRD and CLRT.

CNTL HALT
~~~~~~~~~

Halt the execution of the program. Operand is an integer from 0 to 1023 indicating an exit code. Example::

    CNTL HALT,9

CNTL DUMP
~~~~~~~~~

Dump out stacks and/or memory contents; useful for debugging purposes. Operand indicates the kind of information to dump:

* 1 - Dump both stacks and status word
* 2 - Dump full program memory
* 3 - Dump items in both 1 and 2

For example, this may be a useful debugging command::

    CNTL DUMP,3

CNTL CLRD
~~~~~~~~~

Clears the data stack. No operand accepted::

    CNTL CLRD

CNTL CLRT
~~~~~~~~~

Clears the test stack. No operand accepted::

    CNTL CLRT

CNTL GOTO
~~~~~~~~~

Unconditionally branches to the specified label. To jump to label ``foo``::

    CNTL GOTO,foo

STACK
-----

This machine contains a data stack capable of storing 256 entries. It can be directly manipulated using theses ``STACK`` functions. ``STACK`` instructions can accept labels (memory references) or literals. For more information on literals, see the Literals_ section.

STACK PUSH
~~~~~~~~~~

Push an item onto the top of the stack. Example::

    STACK PUSH,FOO

would push the data at FOO on to the stack. To push a value directly::

    STACK PUSH,42

STACK POP
~~~~~~~~~

Pop an item off of the top of the stack. As an example, this would pop the top item off of the stack and store the value at the memory location given by BAR::

	STACK POP,BAR

Again, this can also be done by providing a numeric memory reference.  Using the following code would pop the first item off of the stack and store it at memory address 27::

	STACK POP,27

STACK TEST
~~~~~~~~~~

JUMP
----

SOPER
-----

MOPER
-----

Literals
========

Integer
-------

Hexadecimal
-----------

Binary
------

Character
---------

Directives
==========

Start
-----

Format::

	Label | start | 0 - 1024
	
The start directive signifies the beginning of the program.  It must appear in the first line of the input program file.  The start directive is also used to set the starting location counter.  It must be provided a number (cannot use labels) that is within the range of memory, 0 - 1024.

Example::

	PRGRM2 start 0

RESET
-----

Format::

	Label | reset | new LC
	
Reset will alter the LC to the given value. The new LC must be larger than the LC of the reset.  For example, if the reset is called at LC 23, the new LC must be greater than 23.  The new value can be given as a number within the range of memory (0 - 1024) or a label, equated label or external reference of similar value.

Example::

	DATA reset 30     : called at LC 12 (hex), sets LC to 1E (30 in hex)
	
EQU
---

Format::

	Label | equ | 0 - 1024 or another equated label
	
Equate allows the user to set a label to the a value between 0 and 1024. If provided a label rather than a number, the label must have been previously equated.

Example::

	MUD EQU 512
	DIRT EQU MUD
	
EQUe
----

Format::

	Label | eque | expression
	
Has the same use as ``EQU`` but allows for expressions in the operand field.  The expression can be made up of constants or previously equated symbols however the resulting computation must be int he range of 0 to 1024.  External references may not be used. Star notation may be used but must be the first item in the expression. Only one star notation per expression is allowed. Up to three operators may be used, however the operators are limited to plus (+) and minus (-).

Example::

	X1 EQUe 5-2+DIRT
	
ENTRY
-----


EXTRN
-----


END
---


DAT
---


ADC
---


ADCE
----


NOP
---

