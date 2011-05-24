============
User's Guide
============

Welcome to the user's guide for the Final Four Accelerated Simulator. Here you will find usage notes, instructions on how to build the simulator, and general documentation. A more detailed description of how to get the simulator up and running can be found in our `How To <how_to.html>`_ section.

.. contents::

Language Reference
==================

For a description of the machine and language specification, please see the the `Language Specification <../language_spec.html>`_ page.

Requirements
============

* A build environment (see Building_)
* .NET 4 Runtime (`Microsoft .NET Framework 4 <http://www.microsoft.com/net/>`_ **or** Mono 2.8+)

Building
========

.. note::
    Your distribution already contains a pre-compiled executable. If a build cannot be accomplished, this platform-independent binary may be used in-place.

The FFA Simulator is provided in source form. To compile, users may use Visual Studio 2010 or MonoDevelop 2.4.

To build, open Simulator.sln in either IDE and build. Depending on the release type selected at build, the executable may be in bin/Debug or in bin/Release.

Alternatively, if ``xbuild`` is installed, it may be run from the Simulator directory on the command-line and will result in output to bin/Debug by default.

For detailed instructions, please see the `Howto Guide <how_to.html>`_.

Usage
=====

The simulator, like the linker and assembler, is run from the command-line. To run the simulator, just pass it an assembled and linked ``.ffa`` `load file <../linker/loader_file.html>`_::

    Simulator.exe program.ffa

.. warning::
    Like the Linker, the Simulator and Assembler, to save space, share some internal code. **When running the Simulator, be sure that Assembler.exe is still in the same directory**, or it may not launch.

A debugging mode is also available, which will print out the contents of the current location counter, the found function at that line, and the top of the data and test stacks. Debug output will be printed out before and after each line. To activate debug mode, pass the ``-d`` switch to the simulator::

    Simulator.exe -d program.ffa

Example
-------

Source
~~~~~~

This example program uses the following two source files, ``module1.txt`` and ``strings.txt``, as in the `linker example <../linker/user_guide.html>`_.

``module1.txt``::
    
    Module1     start 15
                extrn hello2
                extrn hello3
                cntl goto,*+2
    hello1      dat c='He'
                extrn seven
    he          moper writec,hello1     : write "He"
    ll          moper writec,hello2     :       "ll"
    lettero     moper writec,hello3     :       "o"
                stack push,seven
                stack push,I=2
                soper add,2
                stack pop,*+2
                cntl goto,*+2
                dat i=0
                moper writen,*-1
                cntl halt,0
                end Module1

``strings.txt``::
    
    ModStrings  start 9
                entry seven
                entry hello2
    hello2      dat c='ll'
    hello3      dat c='o'
                entry hello3
    seven       dat i=7
                end ModStrings

They are first individually assembled::
    
    Assembler.exe module1.txt module1.obj
    Assembler.exe strings.txt strings.obj

And then the object files are linked together::

    Linker.exe module1.obj strings.obj

Which produces an output file ``module1.ffa``::

    H:Module1:0000:0000:0010:2011143,20,35,37:0012:0010:FFA-LLM:0288:Module1
    T:0000:2002:Module1
    T:0001:4865:Module1
    T:0002:FC01:Module1
    T:0003:FC0D:Module1
    T:0004:FC0E:Module1
    T:0005:280F:Module1
    T:0006:2C02:Module1
    T:0007:8002:Module1
    T:0008:340A:Module1
    T:0009:200B:Module1
    T:000A:0000:Module1
    T:000B:F80A:Module1
    T:000C:0000:Module1
    T:000D:6C6C:Module1
    T:000E:6F00:Module1
    T:000F:0007:Module1
    E:Module1

Running
~~~~~~~

The linked ``module1.ffa`` load file can then be given to the Simulator to run::

    Simulator.exe module1.ffa

The output of this program is displayed to the screen, along with the exit code of the program::

    Hello 9
    Program exited with code: 0

Alternatively, to gain an insight on how each line is being run, you can run the simulator with the ``-d`` switch::

    Simulator.exe -d module1.ffa

The ``-d`` switch turns on debug output, so that you can see the contents of memory at the current line, the active function, and the values of stacks::

    ---Before Simulation--- ---Before Simulation--- ---Before Simulation---
     LC =    0  MEM = 2002 = 0010000000000010  Op-code = 00  Function = 100  S = 0000000010
     Category:     CNTL  Function: GOTO
     S = 2  M(S) = 64513
     Top of data stack = empty
     Top of test stack = empty
    ---Before Simulation--- ---Before Simulation--- ---Before Simulation---


    ===After Simulation===  ===After Simulation===  ===After Simulation===
     LC =    2  MEM = fc01 = 1111110000000001  Op-code = 11  Function = 111  S = 0000000001
     Category:    MOPER  Function: WRITEN
     S = 1  M(S) = 18533
     Top of data stack = empty
     Top of test stack = empty
    ===After Simulation===  ===After Simulation===  ===After Simulation===



    ---Before Simulation--- ---Before Simulation--- ---Before Simulation---
     LC =    2  MEM = fc01 = 1111110000000001  Op-code = 11  Function = 111  S = 0000000001
     Category:    MOPER  Function: WRITEN
     S = 1  M(S) = 18533
     Top of data stack = empty
     Top of test stack = empty
    ---Before Simulation--- ---Before Simulation--- ---Before Simulation---

    He
    ===After Simulation===  ===After Simulation===  ===After Simulation===
     LC =    2  MEM = fc01 = 1111110000000001  Op-code = 11  Function = 111  S = 0000000001
     Category:    MOPER  Function: WRITEN
     S = 1  M(S) = 18533
     Top of data stack = empty
     Top of test stack = empty
    ===After Simulation===  ===After Simulation===  ===After Simulation===



    ---Before Simulation--- ---Before Simulation--- ---Before Simulation---
     LC =    3  MEM = fc0d = 1111110000001101  Op-code = 11  Function = 111  S = 0000001101
     Category:    MOPER  Function: WRITEN
     S = 13  M(S) = 27756
     Top of data stack = empty
     Top of test stack = empty
    ---Before Simulation--- ---Before Simulation--- ---Before Simulation---

    ll
    ===After Simulation===  ===After Simulation===  ===After Simulation===
     LC =    3  MEM = fc0d = 1111110000001101  Op-code = 11  Function = 111  S = 0000001101
     Category:    MOPER  Function: WRITEN
     S = 13  M(S) = 27756
     Top of data stack = empty
     Top of test stack = empty
    ===After Simulation===  ===After Simulation===  ===After Simulation===



    ---Before Simulation--- ---Before Simulation--- ---Before Simulation---
     LC =    4  MEM = fc0e = 1111110000001110  Op-code = 11  Function = 111  S = 0000001110
     Category:    MOPER  Function: WRITEN
     S = 14  M(S) = 28416
     Top of data stack = empty
     Top of test stack = empty
    ---Before Simulation--- ---Before Simulation--- ---Before Simulation---

    o 
    ===After Simulation===  ===After Simulation===  ===After Simulation===
     LC =    4  MEM = fc0e = 1111110000001110  Op-code = 11  Function = 111  S = 0000001110
     Category:    MOPER  Function: WRITEN
     S = 14  M(S) = 28416
     Top of data stack = empty
     Top of test stack = empty
    ===After Simulation===  ===After Simulation===  ===After Simulation===



    ---Before Simulation--- ---Before Simulation--- ---Before Simulation---
     LC =    5  MEM = 280f = 0010100000001111  Op-code = 00  Function = 101  S = 0000001111
     Category:    STACK  Function: PUSH
     S = 15  M(S) = 7
     Top of data stack = empty
     Top of test stack = empty
    ---Before Simulation--- ---Before Simulation--- ---Before Simulation---


    ===After Simulation===  ===After Simulation===  ===After Simulation===
     LC =    5  MEM = 280f = 0010100000001111  Op-code = 00  Function = 101  S = 0000001111
     Category:    STACK  Function: PUSH
     S = 15  M(S) = 7
     Top of data stack = 7
     Top of test stack = empty
    ===After Simulation===  ===After Simulation===  ===After Simulation===



    ---Before Simulation--- ---Before Simulation--- ---Before Simulation---
     LC =    6  MEM = 2c02 = 0010110000000010  Op-code = 00  Function = 101  S = 0000000010
     Category:    STACK  Function: PUSH
     S = 2  M(S) = 64513
     Top of data stack = 7
     Top of test stack = empty
    ---Before Simulation--- ---Before Simulation--- ---Before Simulation---


    ===After Simulation===  ===After Simulation===  ===After Simulation===
     LC =    6  MEM = 2c02 = 0010110000000010  Op-code = 00  Function = 101  S = 0000000010
     Category:    STACK  Function: PUSH
     S = 2  M(S) = 64513
     Top of data stack = 2
     Top of test stack = empty
    ===After Simulation===  ===After Simulation===  ===After Simulation===



    ---Before Simulation--- ---Before Simulation--- ---Before Simulation---
     LC =    7  MEM = 8002 = 1000000000000010  Op-code = 10  Function = 000  S = 0000000010
     Category:    SOPER  Function: ADD
     S = 2  M(S) = 64513
     Top of data stack = 2
     Top of test stack = empty
    ---Before Simulation--- ---Before Simulation--- ---Before Simulation---


    ===After Simulation===  ===After Simulation===  ===After Simulation===
     LC =    7  MEM = 8002 = 1000000000000010  Op-code = 10  Function = 000  S = 0000000010
     Category:    SOPER  Function: ADD
     S = 2  M(S) = 64513
     Top of data stack = 9
     Top of test stack = empty
    ===After Simulation===  ===After Simulation===  ===After Simulation===



    ---Before Simulation--- ---Before Simulation--- ---Before Simulation---
     LC =    8  MEM = 340a = 0011010000001010  Op-code = 00  Function = 110  S = 0000001010
     Category:    STACK  Function: POP
     S = 10  M(S) = 0
     Top of data stack = 9
     Top of test stack = empty
    ---Before Simulation--- ---Before Simulation--- ---Before Simulation---


    ===After Simulation===  ===After Simulation===  ===After Simulation===
     LC =    8  MEM = 340a = 0011010000001010  Op-code = 00  Function = 110  S = 0000001010
     Category:    STACK  Function: POP
     S = 10  M(S) = 9
     Top of data stack = empty
     Top of test stack = empty
    ===After Simulation===  ===After Simulation===  ===After Simulation===



    ---Before Simulation--- ---Before Simulation--- ---Before Simulation---
     LC =    9  MEM = 200b = 0010000000001011  Op-code = 00  Function = 100  S = 0000001011
     Category:     CNTL  Function: GOTO
     S = 11  M(S) = 63498
     Top of data stack = empty
     Top of test stack = empty
    ---Before Simulation--- ---Before Simulation--- ---Before Simulation---


    ===After Simulation===  ===After Simulation===  ===After Simulation===
     LC =   11  MEM = f80a = 1111100000001010  Op-code = 11  Function = 111  S = 0000001010
     Category:    MOPER  Function: WRITEN
     S = 10  M(S) = 9
     Top of data stack = empty
     Top of test stack = empty
    ===After Simulation===  ===After Simulation===  ===After Simulation===



    ---Before Simulation--- ---Before Simulation--- ---Before Simulation---
     LC =   11  MEM = f80a = 1111100000001010  Op-code = 11  Function = 111  S = 0000001010
     Category:    MOPER  Function: WRITEN
     S = 10  M(S) = 9
     Top of data stack = empty
     Top of test stack = empty
    ---Before Simulation--- ---Before Simulation--- ---Before Simulation---

    9
    ===After Simulation===  ===After Simulation===  ===After Simulation===
     LC =   11  MEM = f80a = 1111100000001010  Op-code = 11  Function = 111  S = 0000001010
     Category:    MOPER  Function: WRITEN
     S = 10  M(S) = 9
     Top of data stack = empty
     Top of test stack = empty
    ===After Simulation===  ===After Simulation===  ===After Simulation===



    ---Before Simulation--- ---Before Simulation--- ---Before Simulation---
     LC =   12  MEM = 0000 = 0000000000000000  Op-code = 00  Function = 000  S = 0000000000
     Category:     CNTL  Function: HALT
     S = 0  M(S) = 8194
     Top of data stack = empty
     Top of test stack = empty
    ---Before Simulation--- ---Before Simulation--- ---Before Simulation---


    Program exited with code: 0 

As you can see, debug mode creates quite a lot of output, so you can trace the execution of your program should something go wrong.

Errors
======

.. include:: ../tmp/errorlist.rst

Limitations
===========

TODO


