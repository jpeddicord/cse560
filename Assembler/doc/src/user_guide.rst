============
User's Guide
============

Thank for evaluating the test edition of the Final Four Accelerated Assembler. Here you will find usage notes, build instructions, and general documentation about the assembler.

This test edition only processes a single pass over input files, and does not generate executable code. A future release will process a second pass.

.. contents::
    :depth: 1

Requirements
============

* A build environment (see `Building`)
* .NET 4 Runtime (Microsoft .NET Framework 4 **or** Mono 2.8+)

Building
========

The FFA assembler is provided in source form. To compile, users may use Visual Studio 2010 or MonoDevelop 2.4.

To build, open Assembler.sln in either IDE and build. Depending on the release type selected at build, the executable may be in bin/Debug or in bin/Release.

Alternatively, if ``xbuild`` is installed, it may be run from the Assembler directory on the command-line and will result in output to bin/Debug by default.

.. note::
    Your distribution may already include a pre-compiled executable in bin/. If a build cannot be accomplished, these platform-independent binaries may be used in-place.

Usage
=====

The usage of the FFA Assembler is as follows::

    Assembler <source>

Output will be printed to the screen in this test edition.

The format of the ``source`` file is documented in the FFA machine `language specifications <language_spec.html>`_. The format of the file is one instruction per line, and comments are accepted as defined in the language specification. Specific deviations from specification may have been made for additional functionality or semantics. For specifics, please see `Errata <errata.html>`_.


Output
------

Pass 1 of the FFA Assembler produces intermediate code that cannot be executed. Certain references may not be defined during the first pass, and so code produced cannot be directly used. This will be implemented in the second pass of the assembler.

However, the intermediate code produced can provide insight into syntactic or semantic errors, and can be used as an initial check to see if code is semi-correct. The list of possible errors is desceribed below:

Errors
``````

.. include:: ../tmp/errorlist.rst


Errors are currently *not* printed for the following issues, since they will be processed in pass 2:

* Malformed expressions in EQUE and ADCE instructions
* Undefined label usage
* Undefined externals (linker)

