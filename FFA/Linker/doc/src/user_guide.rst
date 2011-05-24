============
User's Guide
============

Welcome to the user's guide for the Final Four Accelerated Linker. Here you will find usage notes, instructions on how to build the linker, and general documentation. A more detailed description of how to get the linker up and running can be found in our `How To <how_to.html>`_ section.

Requirements
============

* A build environment (see Building_)
* .NET 4 Runtime (`Microsoft .NET Framework 4 <http://www.microsoft.com/net/>`_ **or** Mono 2.8+)

Building
========

.. note::
    Your distribution already contains a pre-compiled executable. If a build cannot be accomplished, this platform-independent binary may be used in-place.

The FFA Linker is provided in source form. To compile, users may use Visual Studio 2010 or MonoDevelop 2.4.

To build, open Linker.sln in either IDE and build. Depending on the release type selected at build, the executable may be in bin/Debug or in bin/Release.

Alternatively, if ``xbuild`` is installed, it may be run from the Linker directory on the command-line and will result in output to bin/Debug by default.

For detailed instructions, please see the `Howto Guide <how_to.html>`_.

Usage
=====

The linker, like the assembler, is run from the command-line.
Basic usage of the linker is as follows::

    Linker.exe first.obj [second.obj [third.obj [...]]]

Any number of object files can be given to the linker, as long as they contain `valid output <../assembler/object_file.html>`_ from the assembler. The name inside the first object file will be the name that the output program has. The linker will output a file with the same name as the first object file, but with the file extension ``.ffa``. For the format of this file, see the `loader file format <loader_file.html>`_. For example, if you ran::

    Linker.exe module.obj strings.obj

Then ``module.ffa`` would be generated in the same directory you ran the linker from.

.. warning::
    The Linker and Assembler, to save space, share some internal code. **When running the Linker, be sure that Assembler.exe is still in the same directory**, or it may not launch.

Example
-------

Assembly
~~~~~~~~

As an example, ``module1.txt`` and ``strings.obj`` (given below) are individually passed to the assembler.

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

Essentially, ``module1`` loads some resources from ``strings``, and prints out Hello along with the result of 7 + 2.

Assembling these files results in the object files given in the next step.

Linking
~~~~~~~

These two object files, ``module1.obj`` and ``strings.obj``, given below, are then fed to the linker.

``module1.obj``::

    H:Module1:000F:000D:000F:2011142,20,49,26:9001:0014:0000:000D:0007:FFA-ASM:Module1
    T:000F:2011:M:1:Module1
    T:0010:4865:A:0:Module1
    T:0011:FC10:R:0:Module1
    T:0012:FC00:M:1:Module1
    T:0013:FC00:M:1:Module1
    T:0014:2800:M:1:Module1
    T:0015:2C02:A:0:Module1
    T:0016:8002:A:0:Module1
    T:0017:3419:M:1:Module1
    T:0018:201A:M:1:Module1
    T:0019:0000:A:0:Module1
    T:001A:F819:M:1:Module1
    T:001B:0000:A:0:Module1
    M:000F:2011:+:Module1:Module1
    M:0012:FC00:+:hello2:Module1
    M:0013:FC00:+:hello3:Module1
    M:0014:2800:+:seven:Module1
    M:0017:3419:+:Module1:Module1
    M:0018:201A:+:Module1:Module1
    M:001A:F819:+:Module1:Module1
    E:Module1

``strings.obj``::

    H:ModStrings:0009:0003:0009:2011142,20,32,47:9001:0006:0003:0003:0000:FFA-ASM:ModStrings
    L:hello2:0009:ModStrings
    L:hello3:000A:ModStrings
    L:seven:000B:ModStrings
    T:0009:6C6C:A:0:ModStrings
    T:000A:6F00:A:0:ModStrings
    T:000B:0007:A:0:ModStrings
    E:ModStrings

They are linked with the following command::

    Linker.exe module1.obj strings.obj

Output
~~~~~~

The linker then outputs a single ``module1.ffa`` file containing::
    
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

Additionally, the linker outputs the symbols found and relocated to the screen::

   ----------- SYMBOL TABLE -----------
           Label: Relocation value Symbol usage

          hello2: 000D             ENTRY
          hello3: 000E             ENTRY
      ModStrings: 0004             PRGMNAME
         Module1: 03F1             PRGMNAME
           seven: 000F             ENTRY

This linked ``module1.ffa`` file can then be passed into the `Simulator <../simulator/index.html>` to test in a virtual machine.

Errors
======

.. include:: ../tmp/errorlist.rst

Limitations
===========

TODO


