==================
Object File Format
==================

.. contents::

Structure
=========

The structure of an FFA-assembled object file consists of five major sections, each containing a record or group of records. They are:

* Header - 1 **H** record
* Linking - Any number of **L** records
* Text - Any number of **T** records
* Modification - Any number of **M** records
* End - 1 **E** record

Each record is separated by a newline, with no major distinction between sections. The format of each record varies, and is described in the following sections.

Header (H) Record
=================

The Header record contains an overview of the contents of the rest of the object file. It contains the following fields, separated by colon (:) characters:

* ``H``
* Program name
* Assembler-assigned program load address (4-digit hex)
* Module length (4-digit hex)
* Execution start address (4-digit hex)
* Date & time of assembly
  * Julian date: YYYYDDD,HH:MM:SS
  * Where YYYY is the year, DDD is the day of the year, HH is hours, MM minutes, and SS seconds
* Version number of this assembler (4-digit hex)
* Total number of records in this object file (4-digit hex)
* Total number of linking records in this object file (4-digit hex)
* Total number of text records in this object file (4-digit hex)
* Total number of modification records in this object file (4-digit hex)
* ``FFA-ASM``
* Program name

An example H record could be::

    H:ALT01:0000:0031:0000:2011135,19:11:09:9001:0031:0000:0031:0000:FFA-ASM:ALT01

Linking (L) Record
==================

A linking record describes a symbol that has been exported (via EXTRN or RESET directives) for use in other modules by the linker. Any number may be present, as long as they are grouped together in the Linking section of the object file. A linking record contains the following fields, separated by colon (:) characters:

* ``L``
* Entry name
* Location within this program (4-digit hex)
* Program name

An example L record::

    L:CD:0008:ALT05

Text (T) Record
===============

A text record contains a single instruction or piece of data in the program. Any number may be present as long as they are in the Text section of the file. A text record consists of the following fields, separated by colon (:) characters:

* ``T``
* Program assigned location (4-digit hex)
* Hex code of line
* Address status flag (A, R, or M)
* Number of M adjustments required (1-digit hex)
* Program name

An example T record::

    T:0005:F809:R:0:ALT05

Modification (M) Record
=======================

A modification record describes a number of modifications that need to be applied to a text record. When a modification record is associated with a text record, the corresponding text record should have the appropriate number set in the M-adjustments field. A modification record contains the following fields, separated by color (:) characters:

* ``M``
* Program location to be modified (4-digit hex)
* Original instruction/data word (4-digit hex)
* Up to 15 adjustments, each separated by an additional colon (:)
  * A sign: + or -
  * ``:``
  * A label name to adjust by
* Program name

An example M record, containing three modifications::

    M:0004:D800:+:test:-:sub:+:MuD:ALT05

End (E) Record
==============

The end record signifies the end of the object file. It has the simplest syntax:

* ``E:``
* Program name

Example::

    E:Program1

