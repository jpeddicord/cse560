======
Errors
======

Below is the vast list of errors you may encounter when linking a program that is incorrect or has been tampered with.

.. list-table::
   :widths: 5 55 20
   :header-rows: 1
   :stub-columns: 1
   
   * - Error #
     - Message
     - Tested In

   * - EW.01
     - Assembly date and time invalid, but it doesn't matter. Linking continues.
     - 

   * - EW.02
     - Version number of assembler should be 4 digits, but it doesn't matter. Linking continues.
     - 

   * - EW.03
     - Total number of records should be 4 hex digits but is not, assuming valid hex. Linking continues.
     - 

   * - EW.04
     - Total number of records should be valid hex but is not. The linker will not be able to verify that the proper number of records have been read in. Linking continues, but things might get weird.
     - 

   * - EW.05
     - Total number of linking records should be 4 hex digits but is not, assuming valid hex. Linking continues.
     - 

   * - EW.06
     - Total number of linking records should be valid hex but is not. The linker will not be able to verify that the proper number of linking records have been read in. Linking continues, but beware the Kraken.
     - 

   * - EW.07
     - Total number of modify records should be 4 hex digits but is not, assuming valid hex. Linking continues.
     - 

   * - EW.08
     - Total number of modify records should be valid hex but is not. The linker will not be able to verify that the proper number of linking records have been read in. Linking continues, danger: choking hazard.
     - 

   * - EW.09
     - Name of assembler does not match expected (FFA-ASM), it's probably okay. Linking continues.
     - 

   * - EW.10
     - Entry location in program should be 4 hex digits but is not, assuming valid hex. Linking continues.
     - 

   * - EW.11
     - Text record location in program should be 4 hex digits but is not, assuming valid hex. Linking continues.
     - 

   * - EW.12
     - Text record hex code should be 4 hex digits but is not, assuming valid hex. Linking continues.
     - 

   * - EW.13
     - Text record amount to be adjusted should only be 1 hex digit but is not. Adjustment amount assumed to be 1, though it doesn't really matter too much. For recreational use only. Linking continues.
     - 

   * - EW.14
     - Text record amount to be adjusted should be valid hex but is not. Adjustment amount assumed to be 1. Linking continues. Slippery when wet.
     - 

   * - EW.15
     - Modify record location in program should be 4 hex digits but is not, assuming valid hex. Linking continues.
     - 

   * - EW.16
     - Modify record hex code to modify should be 4 hex digits but is not, assuming valid hex. Linking continues.
     - 

   * - EW.17
     - Modify record should not have more than 15 adjustments but it does. But it doesn't really matter. Linking continues.
     - 

   * - EW.18
     - End record has incorrect number of fields. Linker will assume that the end record belongs. Linking continues.
     - 

   * - EW.19
     - Actual number of linking records encountered in this program does not match number of linking records in header record. Linking continues.
     - 

   * - EW.20
     - Actual number of modify records encountered in this program does not match number of modify records in header record. Linking continues.
     - 



.. include:: ../tmp/errorlist.rst
