============
User's Guide
============

Thank for evaluating the test edition of the Final Four Accelerated Assembler. Here you will find usage notes, build instructions, and general documentation about the assembler.

This test edition only processes a single pass over input files, and does not generate executable code. A future release will process a second pass.

.. contents::
    :depth: 1

Language Reference
==================

For a description of the machine and language specification, please see the the `Language Specification <language_spec.html>`_ page.

Requirements
============

* A build environment (see Building_)
* .NET 4 Runtime (`Microsoft .NET Framework 4 <http://www.microsoft.com/net/>`_ **or** Mono 2.8+)

Building
========

.. note::
    Your distribution already contains a pre-compiled executable. If a build cannot be accomplished, this platform-independent binary may be used in-place.

The FFA assembler is provided in source form. To compile, users may use Visual Studio 2010 or MonoDevelop 2.4.

To build, open Assembler.sln in either IDE and build. Depending on the release type selected at build, the executable may be in bin/Debug or in bin/Release.

Alternatively, if ``xbuild`` is installed, it may be run from the Assembler directory on the command-line and will result in output to bin/Debug by default.

Usage
=====

The usage of the FFA Assembler is as follows::

    Assembler <source>

Output will be printed to the screen in this test edition.

The format of the ``source`` file is documented in the FFA machine `language specifications <language_spec.html>`_. The format of the file is one instruction per line, and comments are accepted as defined in the language specification. Specific deviations from specification may have been made; for specifics, please see Limitations_.

Output
------

Pass 1 of the FFA Assembler produces intermediate code that cannot be executed. Certain references may not be defined during the first pass, and so code produced cannot be directly used. This will be implemented in the second pass of the assembler.

However, the intermediate code produced can provide insight into syntactic or semantic errors, and can be used as an initial check to see if code is semi-correct. The list of possible errors is desceribed below:

Errors
``````

.. include:: ../tmp/errorlist.rst

Errors are currently *not* printed for the following issues, since they will be processed in pass 2:

* Malformed expressions in ADCE instructions
* Undefined label usage
* Undefined externals (linker)

Example
-------

An example source file, ``sample.txt``, could contain::

    PGC  Start 0
         STACK PUSH,AB     :place AB on stack  0  2005   r
         STACK PUSH,CD     :place CD on stack  1  2006   r
         SOPER WRITEN,2    :print out top      2  3C02   a  0111 1100 0000 0010
         MOPER WRITEN,RES  :output RES         3  FC07   r  1111 1100 0000 0111
         CNTL  HALT,0      :halt program       4  0000   a
    AB   CNTL  HALT,10     :halt program       5  000A   a
    CD   CNTL  HALT,110    :halt program       6  006E   a
    RES  CNTL  HALT,0      :halt program       7  0000   a
    END  PGC               :end of program

This would be processed with the following command::

    Assembler.exe sample.txt > output

Output is displayed line-by-line as an intermediate representation of the assembly. Each source line is displayed, followed by the dissected contents of the line and any errors. At the end of the file, a symbol table is printed with a lits of all defined and valid symbols in the source program. Each symbol is accompanied by its position in the program (location counter), its symbol type, and a defined value for equated symbols. If any errors were found in the given program, the total count will be displayed at the end.

The ``output`` file, then, would contain::

   PGC  Start 0
        Line: 1                             LC:
        Label: PGC
        Partial Bytecode:
        Directive: START
        Directive Operand: 0                (Literal: NUMBER)
    -----
         STACK PUSH,AB    :place AB on stack  0  2005   r		
        Line: 2                             LC: 0			 
        Label:								| Each line is displayed followed by a summary of how the parser
        Partial Bytecode: 0010100000000000				| decomposed the line. Partial bytecode will be created from the
        Category: STACK                     Function: PUSH		| information it is able to gather in pass 1. If any errors are
        Operand: AB                         (Literal: NONE)		| found, they will be displayed at the end of the block with an
        Comment: :place AB on stack  0  2005   r			| error reference number and description.
    -----														
         STACK PUSH,CD    :place CD on stack  1  2006   r
        Line: 3                             LC: 1
        Label:
        Partial Bytecode: 0010100000000000
        Category: STACK                     Function: PUSH
        Operand: CD                         (Literal: NONE)
        Comment: :place CD on stack  1  2006   r
    -----
         SOPER WRITEN,2    :print out top      2  3C02   a  0111 1100 0000 0010
        Line: 4                             LC: 2
        Label:
        Partial Bytecode: 1011100000000010
        Category: SOPER                     Function: WRITEN
        Operand: 2                          (Literal: NUMBER)
        Comment: :print out top      2  3C02   a  0111 1100 0000 0010
    -----
         MOPER WRITEN,RES  :output RES         3  FC07   r  1111 1100 0000 0111
        Line: 5                             LC: 3
        Label:
        Partial Bytecode: 1111100000000000
        Category: MOPER                     Function: WRITEN
        Operand: RES                        (Literal: NONE)
        Comment: :output RES         3  FC07   r  1111 1100 0000 0111
    -----
         CNTL  HALT,0     :halt program       4  0000   a
        Line: 6                             LC: 4
        Label:
        Partial Bytecode: 0000000000000000
        Category: CNTL                      Function: HALT
        Operand: 0                          (Literal: NUMBER)
        Comment: :halt program       4  0000   a
    -----
    AB   CNTL  HALT,10    :halt program       5  000A   a
        Line: 7                             LC: 5
        Label: AB
        Partial Bytecode: 0000000000001010
        Category: CNTL                      Function: HALT
        Operand: A                          (Literal: NUMBER)
        Comment: :halt program       5  000A   a
    -----
    CD   CNTL  HALT,110   :halt program       6  006E   a
        Line: 8                             LC: 6
        Label: CD
        Partial Bytecode: 0000000001101110
        Category: CNTL                      Function: HALT
        Operand: 6E                         (Literal: NUMBER)
        Comment: :halt program       6  006E   a
    -----
    RES  CNTL  HALT,0     :halt program       7  0000   a
        Line: 9                             LC: 7
        Label: RES
        Partial Bytecode: 0000000000000000
        Category: CNTL                      Function: HALT
        Operand: 0                          (Literal: NUMBER)
        Comment: :halt program       7  0000   a
    -----
         END   PGC        :end of program
        Line: 10                            LC:
        Label:
        Partial Bytecode:
        Directive: END
        Directive Operand: PGC              (Literal: NONE)
        Comment: :end of program
    -----

    ---- SYMBOL TABLE ----										
                                  AB: 0x5      LABEL			| The symbol table will be sorted and displayed at
                                  CD: 0x6      LABEL			| the bottom of the report. This contains the symbol name
                                 PGC:          PRGMNAME	 		| followed by the location counter, then the usage of the
                                 RES: 0x7      LABEL 			| symbol and last the string value if it is an equated symbol.
																

Following the format above, this output contains each individual line and details, followed by the symbol table containing 4 symbols. No errors were found.

Limitations
===========

For software limitations, please see `Errata <errata.html>`_.
