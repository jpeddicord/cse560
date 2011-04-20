=======================
FFA Machine Description
=======================

.. contents::
    :depth: 1
    :backlinks: none

Machine Hardware
================

The FFA machine has a word addressable memory of size 1024 (from memory location 0 to 1023). Each word has a size of 16 bits.

It has two stacks, a data stack and a test-stack.

* Data-stack : Contains all data that is pushed onto the stack due to user instructions.  See the `language specifications <language_spec.html>`_ for specific instructions.  Limited to 256 entries (words).
* Test-stack : Stores the results of the ``TEST`` operation.  Limited to 5 entries.

Numbers are represented using two's complement.  Number size is based on the use of the number, for example 10 bits for operands and 16 bits for entire words such as when using the ``DAT`` directive.  Characters are represented used ASCII codes and use 8 bits per character for a total of two characters per word.

Binary Formats
==============

Binary formats for instructions will differ based on the instruction, but will always follow one of the following formats:

.. list-table::
   :widths: 6 9 33
   :header-rows: 0
   :stub-columns: 0
   
   * - Opcode 2 bits
     - Function 3 bits
     - Unused 11 bits

Examples: ``CNTL CLRD``, ``CNTL CLRT``


.. list-table::
   :widths: 6 9 5 30
   :header-rows: 0
   :stub-columns: 0
   
   * - Opcode 2 bits
     - Function 3 bits
     - Literal Flag 1 bit
     - Memory address 10 bits

Examples: ``STACK PUSH``, ``STACK POP``, ``STACK TEST``


.. list-table::
   :widths: 6 9 5 30
   :header-rows: 0
   :stub-columns: 0
   
   * - Opcode 2 bits
     - Function 3 bits
     - Unused 1 bit
     - Memory address 10 bits

Examples: ``JUMP`` functions


.. list-table::
   :widths: 6 9 5 6 24
   :header-rows: 0
   :stub-columns: 0
   
   * - Opcode 2 bits
     - Function 3 bits
     - Read/Write flag 1 bit
     - Unused 2 bits
     - Number 8 bits

Examples: ``SOPER`` functions


.. list-table::
   :widths: 6 9 5 30
   :header-rows: 0
   :stub-columns: 0
   
   * - Opcode 2 bits
     - Function 3 bits
     - Read/Write flag 1 bit
     - Memory address 10 bits

Examples: ``MOPER`` functions


Assembler and Project Description
=================================

Development tools
-----------------

* Language: We've chosen to use C# for this project. Our group has little experience with this language and found this to be a good opportunity to develop our skills with it.
* IDEs: We use `Visual Studio 2010 <http://msdn.microsoft.com/en-us/vstudio/aa718325>`_ and `MonoDevelop <http://monodevelop.com/>`_.
* Version Control: We use `Git <http://git-scm.com/>`_ with our repository stored on the CSE servers.
* Testing: Much of our testing is done through unit cases which is handled using `NUnit <http://www.nunit.org/>`_.
* Documentation: Our documentation is created using `Doxygen <http://www.doxygen.org/>`_ as well as reStructuredText `<http://docutils.sourceforge.net/rst.html>`_ and some custom scripting.

Application Description
-----------------------

Note: This is simply a brief decription of how our application functions to give a general idea. For a better understanding of method functions please see the `developer guide <annotated.html>`_.

Currently our Assembler only handles pass 1 of the compilation process.  Lines are read in from the source file and stored using `IntermediateLine`s which acts similar to an array holding various data about each line such as the actually line text, its different parts, its line number and location counter and any errors found on the line during parsing. These `IntermediateLine`s are stored in an `IntermediateFile`. Pass 1 is driven by `Parser` which calls `Tokenizer` to correctly break down each line. Instructions (as well as their functions and respective byte code values) and Directives are stored in text files and read in at the start of parsing. Lines determined to be instructions are parsed in the `Parser` class. Lines determined to be Directives are parsed in `DirectiveParser`.  Dealing with operands such as literals, numbers and expressions are handled by `OperandParser`. When dealing with converting between two's complement and back we have a function in BinaryHelper that will take any value (within a range) and convert to the correct representation (see the `binary helper <class_assembler_1_1_binary_helper.html>`_ documentation for a better description). Symbols are added to and looked up from the symbol table by using the `SymbolTable` class.  We have also created a `Logger` class that will create log files in Assembler/Assembler/bin/Debug/Log in the event we need to trace the program for bugs. The `Logger` is called to create a new log entry whenever a major action has been made such as completing a procedure.
