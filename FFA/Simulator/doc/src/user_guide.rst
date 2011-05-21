============
User's Guide
============

Welcome to the user's guide for the Final Four Accelerated Simulator. Here you will find usage notes, instructions on how to build the simulator, and general documentation. A more detailed description of how to get the simulator up and running can be found in our `How To <how_to.html>`_ section.

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

TODO:
    * Basic intro
    * Command-line args
    * Debug mode
    * Sample input
    * Sample output

Limitations
===========

TODO


