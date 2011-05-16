============
User's Guide
============

Thank for evaluating the test edition of the Final Four Accelerated Assembler. Here you will find usage notes, build instructions, and general documentation about the assembler. A more detailed description of how to get the Assembler up and running can be found in our `How To <how_to.html>`_ section.

.. contents::
   :backlinks: none

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

    Assembler <source> [output]

Output will be printed to the screen in this test edition.

The format of the ``source`` file is documented in the FFA machine `language specifications <language_spec.html>`_. The format of the file is one instruction per line, and comments are accepted as defined in the language specification. Specific deviations from specification may have been made; for specifics, please see Limitations_.

Output
------

Pass 1 of the FFA Assembler produces intermediate code that cannot be executed. Certain references may not be defined during the first pass, and so code produced cannot be directly used. This will be implemented in the second pass of the assembler.

However, the intermediate code produced can provide insight into syntactic or semantic errors, and can be used as an initial check to see if code is semi-correct. The list of possible errors is desceribed below:

Errors
``````
The following is a list of all errors currently caught by the Assembler.  There are three kinds of errors.

* Fatal - denoted by EF.xx - Cannot be recovered from. Causes the Assembler to stop.
* Serious - denoted by ES.xx - Can be recovered from but will likely cause erratic program behavior. Lines with serious errors are usually replaced with NOP or ignored completely.
* Warning - denoted by EW.xx - Can be recovered from but may lead to erratic program behavior.

Each error will give a brief description of what the issue is and what will be done by the assembler.

.. include:: ../tmp/errorlist.rst

Errors are currently *not* printed for the following issues, since they will be processed in pass 2:

* Malformed expressions in ADCE instructions
* Undefined label usage
* Undefined externals (linker)

Example
-------

An example source file, ``sample.txt``, could contain::

    PGC  Start 0
         STACK PUSH,AB    :place AB on stack  0  2805   r
         STACK PUSH,CD    :place CD on stack  1  2806   r
         SOPER WRITEN,2    :print out top     2  B802   a  
         MOPER WRITEN,RES  :output RES        3  F807   r  
         CNTL  HALT,0     :halt program       4  0000   a
    AB   CNTL  HALT,10    :halt program       5  000A   a
    CD   CNTL  HALT,110   :halt program       6  006E   a
    RES  CNTL  HALT,0     :halt program       7  0000   a
         END   PGC        :end of program 

This would be processed with the following command::

    Assembler.exe sample.txt sample.obj

An assembly report will be printed to the screen, containing, for each line:

* Location counter
* Object Code
* Linker flag
* Source line number
* Original source line

At the end of the output, the symbol table will be printed, containing a list of all found symbols in the source file, along with their locations and types.

In addition, an object file will be output to ``sample.obj``, which follows the format specified in the `Object File <object_file.html>`_ section.

Output
``````
The assembly report and symbol table for the above input is::

    LOC   OBJCODE   FLAG   LINE   SOURCE
                           1      PGC  Start 0
    0     2805      R      2           STACK PUSH,AB    :place AB on stack  0  2005   r
    1     2806      R      3           STACK PUSH,CD    :place CD on stack  1  2006   r
    2     B802      A      4           SOPER WRITEN,2    :print out top      2  3C02   a  0111 1100 0000 0010
    3     F807      R      5           MOPER WRITEN,RES  :output RES         3  FC07   r  1111 1100 0000 0111
    4     0000      A      6           CNTL  HALT,0     :halt program       4  0000   a
    5     000A      A      7      AB   CNTL  HALT,10    :halt program       5  000A   a
    6     006E      A      8      CD   CNTL  HALT,110   :halt program       6  006E   a
    7     0000      A      9      RES  CNTL  HALT,0     :halt program       7  0000   a
                           10          END   PGC        :end of program

    ---- SYMBOL TABLE ----
                                  AB: 0x5      LABEL             
                                  CD: 0x6      LABEL             
                                 PGC:          PRGMNAME          
                                 RES: 0x7      LABEL             

The ``sample.obj`` file for the above example would contain::

    H:PGC:0000:0008:0000:2011134,21:19:47:9001:0008:0000:0008:0000:FFA-ASM:PGC
    T:0000:2805:R:0:PGC
    T:0001:2806:R:0:PGC
    T:0002:B802:A:0:PGC
    T:0003:F807:R:0:PGC
    T:0004:0000:A:0:PGC
    T:0005:000A:A:0:PGC
    T:0006:006E:A:0:PGC
    T:0007:0000:A:0:PGC
    E:PGC

Following the format above, this output contains each individual line and details, followed by the symbol table containing 4 symbols. No errors were found.

Limitations
===========

This assembler does not have any known limitations. Programs assembled and source input files are only limited by the capabilities of the host operating system. However, keep in mind requirements in the target machine specification and language, such as the limit of 1024 words per program (after linking).
