==================
Loader File Format
==================

.. contents::

Structure
=========

The structure of an FFA-linked loader file is similar to that of an object file, but is generally more simplified and linear. A load file can be fed to the simulator to be run in a virtual machine.
It consists of three sections:

* Header - 1 **H** record
* Text - Any number of **T** records
* End - 1 **E** record

Each record is separated by a newline, with no major distinction between sections. The format of each record varies, and is described in the following sections. In general, fields in a record are separated by a colon (:).

Header (H) Record
=================

The Header record contains an overview of the contents of the load file. It contains the following fields, each separated by a colon.

* ``H``
* Module name from first object (program name)
* Combined module load address (4-digit hex)
* Module execution start address (4-digit hex)
* Total length of module (4-digit hex)
* Date & time of assembly
    * Julian date: YYYYDDD,HH:MM:SS
    * Where YYYY is the year, DDD is the day of the year, HH is hours, MM minutes, and SS seconds
* Total number of records (4-digit hex)
* Total number of text records (4-digit hex)
* ``FFA-LLM``
* Version number of the linker (4-digit hex)
* Program name

Text (T) Record
===============

A text record describes a single point of execution or data word. It contains the following fields separated by a colon:

* ``T``
* Location in program (4-digit hex)
* Instruction or data word (4-digit hex)
* Program name (same as in ``H`` record)

End (E) Record
==============

The End record contains two fields, separated by a colon:

* ``E``
* Program name (same as in ``H`` record)

