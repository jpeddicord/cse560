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


