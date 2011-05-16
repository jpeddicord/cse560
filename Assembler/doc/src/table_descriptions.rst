==================
Table Descriptions
==================

Directives, Instructions, and Errors are contained inside the Resources folder in the source. This page describes the file format of each resource.

Directives
==========

Located in ``directives.txt``, this file contains valid directives that the assembler can process. The format of this file is one directive name per line, with no spacing or special fields.

Instructions
============

``instructions.txt`` contains the instruction map to bytecode. Each line contains three fields:

* Instruction category
* Instruction function
* 5-digit binary bytecode

Errors
======

``errors.txt`` contains all possible errors that can be thrown when processing a source file. They are organized by severity. Each line has two fields; the first being the ID of the error followed by a space, with the remainder of the line being the error text. The error ID is made up of a severity of EF (fatal), ES (serious), and EW (warning), followed by a dot (.), followed by the numeric error code.

