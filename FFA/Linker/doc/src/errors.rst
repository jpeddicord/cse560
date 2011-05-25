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
     - HeaderErrorTest1_

   * - EW.02
     - Version number of assembler should be 4 digits, but it doesn't matter. Linking continues.
     - HeaderErrorTest1_

   * - EW.03
     - Total number of records should be 4 hex digits but is not, assuming valid hex. Linking continues.
     - HeaderErrorTest1_

   * - EW.04
     - Total number of records should be valid hex but is not. The linker will not be able to verify that the proper number of records have been read in. Linking continues, but things might get weird.
     - HeaderErrorTest1_

   * - EW.05
     - Total number of linking records should be 4 hex digits but is not, assuming valid hex. Linking continues.
     - HeaderErrorTest1_

   * - EW.06
     - Total number of linking records should be valid hex but is not. The linker will not be able to verify that the proper number of linking records have been read in. Linking continues, but beware the Kraken.
     - HeaderErrorTest1_

   * - EW.07
     - Total number of modify records should be 4 hex digits but is not, assuming valid hex. Linking continues.
     - HeaderErrorTest1_

   * - EW.08
     - Total number of modify records should be valid hex but is not. The linker will not be able to verify that the proper number of linking records have been read in. Linking continues, danger: choking hazard.
     - HeaderErrorTest1_

   * - EW.09
     - Name of assembler does not match expected (FFA-ASM), it's probably okay. Linking continues.
     - HeaderErrorTest1_

   * - EW.10
     - Entry location in program should be 4 hex digits but is not, assuming valid hex. Linking continues.
     - LinkingErrorTest1_

   * - EW.11
     - Text record location in program should be 4 hex digits but is not, assuming valid hex. Linking continues.
     - TextErrorTest1_

   * - EW.12
     - Text record hex code should be 4 hex digits but is not, assuming valid hex. Linking continues.
     - TextErrorTest1_

   * - EW.13
     - Text record amount to be adjusted should only be 1 hex digit but is not. Adjustment amount assumed to be 1, though it doesn't really matter too much. For recreational use only. Linking continues.
     - TextErrorTest1_

   * - EW.14
     - Text record amount to be adjusted should be valid hex but is not. Adjustment amount assumed to be 1. Linking continues. Slippery when wet.
     - TextErrorTest1_

   * - EW.15
     - Modify record location in program should be 4 hex digits but is not, assuming valid hex. Linking continues.
     - ModifyErrorTest1_, RelocationErrorTest1_

   * - EW.16
     - Modify record hex code to modify should be 4 hex digits but is not, assuming valid hex. Linking continues.
     - ModifyErrorTest1_

   * - EW.17
     - Modify record should not have more than 15 adjustments but it does. But it doesn't really matter. Linking continues.
     - ModifyErrorTest1_

   * - EW.18
     - End record has incorrect number of fields. Linker will assume that the end record belongs. Linking continues.
     - EndErrorTest1_

   * - EW.19
     - Actual number of linking records encountered in this program does not match number of linking records in header record. Linking continues.
     - LinkingErrorTest1_, LinkingErrorTest2_, RelocationErrorTest1_

   * - EW.20
     - Actual number of modify records encountered in this program does not match number of modify records in header record. Linking continues.
     - ModifyErrorTest1_, ModifyErrorTest2_, RelocationErrorTest1_

   * - ES.01
     - Input found after end record was reached. Input ignored.
     - InputAfterEndTest1_

   * - ES.02
     - Program name must be between 2 and 32 characters long, start with a letter, and contain only letters and numbers. Linking continues.
     - HeaderErrorTest1_

   * - ES.03
     - Assembler assigned location counter should be 4 hex digits but is not, assuming valid hex. Linking continues.
     - HeaderErrorTest1_

   * - ES.04
     - Module length should be 4 hex digits but is not, assuming valid hex. Linking continues.
     - HeaderErrorTest1_

   * - ES.05
     - Execution start address should be 4 hex digits but is not, assuming valid hex. Linking continues.
     - HeaderErrorTest1_

   * - ES.06
     - Total number of text records should be 4 hex digits but is not, assuming valid hex. Linking continues.
     - HeaderErrorTest1_

   * - ES.07
     - Total number of text records should be valid hex but is not. The linker will not be able to verify that the proper number of text records have been read in. Linking continues, here be dragons.
     - HeaderErrorTest1_

   * - ES.08
     - Program name at the end of header record does not match program name at the beginning of header record. Linking continues, handle with care.
     - HeaderErrorTest1_

   * - ES.09
     - Linking record has the incorrect number of fields. Record ignored, linking continues.
     - LinkingErrorTest1_

   * - ES.10
     - Entry name must be between 2 and 32 characters long, start with a letter, and contain only letters and numbers. The Linker can roll with it. Linking continues.
     - LinkingErrorTest1_

   * - ES.11
     - Entry location should be valid hex but is not. Linking record will be ignored. Linking continues. May cause irritation!
     - LinkingErrorTest1_

   * - ES.12
     - Entry location should be within the range of 0-1023 but it is not. This entry is ignored. Linking continues.
     - LinkingErrorTest1_, RelocationErrorTest1_

   * - ES.13
     - Program name at the end of linking record should be between 2 and 32 characters long, start with a letter, and contain only letters and numbers. Entry is ignored. Linking continues.
     - LinkingErrorTest2_

   * - ES.14
     - Program name at the end of linking record does not match name of program being parsed. Entry is ignored, linking continues.
     - LinkingErrorTest2_

   * - ES.15
     - Program name at the end of linking record does not appear in the symbol table. Entry will be added to symbol table and linking continues. May cause drowsiness.
     - 

   * - ES.16
     - Text record has incorrect number of fields. Record ignored, linking continues.
     - TextErrorTest1_

   * - ES.17
     - Text record location should be valid hex but is not. The linker will assume the location is 0 and continue linking. Do not swallow.
     - TextErrorTest1_

   * - ES.18
     - Text record location should be within the range of 0-1023 but it is not. The record is ignored. Linking continues.
     - RelocationErrorTest1_, TextErrorTest1_

   * - ES.19
     - Text record hex code cannot be more than 4 digits. NOP inserted. Linking continues.
     - TextErrorTest1_

   * - ES.20
     - Text record hex code should be valid hex but is not. NOP inserted. Linking continues.
     - TextErrorTest1_

   * - ES.21
     - Text record status flag invalid. Assume flag should be 'A'. Linking continues.
     - TextErrorTest1_

   * - ES.22
     - Program name at the end of text record does not match name of program being parsed. Text record is ignored, linking continues.
     - TextErrorTest1_

   * - ES.23
     - Program name at the end of text record does not appear in the symbol table. Text record is ignored, linking continues.
     - TextErrorTest1_

   * - ES.24
     - Modify record location in program should be valid hex but is not. Location cannot be determined, modify record is ignored. Linking continues.
     - ModifyErrorTest1_

   * - ES.25
     - Modify record location in program should be within the range of 0-1023 but it is not. The record is ignored. linking continues.
     - ModifyErrorTest1_, RelocationErrorTest1_

   * - ES.26
     - Modify record hex code cannot be more than 4 hex digits. Modify record ignored, linking continues.
     - ModifyErrorTest1_

   * - ES.27
     - Modify record hex code should be valid hex but is not. Modify record ignored, linking continues.
     - ModifyErrorTest1_

   * - ES.28
     - Modify record sign of adjustment must be either a + or - but is not. Modify record ignored, linking continues.
     - ModifyErrorTest1_

   * - ES.29
     - Modify record label to be adjusted must be between 2 and 32 characters long, start with a letter, and contain only letters and numbers. Linking continues.
     - ModifyErrorTest2_

   * - ES.30
     - Program name at the end of modify record does not match name of program being parsed. Modify record is ignored, linking continues.
     - ModifyErrorTest2_

   * - ES.31
     - Program name at the end of modify record does not exist in the symbol table. Modify record is ignored, linking continues.
     - 

   * - ES.32
     - Modify record adjustments contain mismatched sets. Modify record ignored, linking continues.
     - ModifyErrorTest2_

   * - ES.33
     - Invalid record type encountered. Record will be ignored, linking continues.
     - 

   * - ES.34
     - Program name at end of end record must be between 2 and 32 characters long, start with a letter, and contain only letters and numbers. Linking continues.
     - 

   * - ES.35
     - Program name at end of end record does not match name of program being parsed. Linker will assume end record has been reached as normal. Linking continues.
     - EndErrorTest1_

   * - ES.36
     - Program name at end of end record does not exist in symbol table. Linker will assume end record has been reached and continue as normal. Linking continues.
     - EndErrorTest1_

   * - ES.37
     - Actual number of text records encountered in this program does not match number of text records in header record. This probably is a problem. Linking continues. Harmful or fatal if swallowed.
     - RelocationErrorTest1_

   * - ES.38
     - Attempted to define duplicate entry in symbol table. Duplicate symbol will be discarded. Linking continues but unexpected things may happen at runtime.
     - LinkingSymbolErrorTest1_

   * - ES.39
     - Location value in linking record does not have a corresponding text record. Entry is ignored. Linking continues.
     - 

   * - ES.40
     - Location of text record will be relocated out of bounds of memory. Text record discarded. Linking continues.
     - RelocationErrorTest1_

   * - ES.41
     - Location of text record will be relocated our of bounds of module. Text record discarded. Linking continues.
     - RelocationErrorTest1_

   * - ES.42
     - The text record being added to the module has the same location counter as a previously added text record. Current text record will be discarded. Linking continues.
     - RelocationErrorTest1_

   * - ES.43
     - The address field of the text record will be relocated out of the range of memory. NOP inserted. Linking continues.
     - 

   * - ES.44
     - Location of linking record will be relocated out of bounds of memory. Linking record discarded. Linking continues.
     - LinkingErrorTest1_, RelocationErrorTest1_

   * - ES.45
     - Location of linking record will be relocated our of bounds of module. Linking record discarded. Linking continues.
     - RelocationErrorTest1_

   * - ES.46
     - The linking record being added to the module has the same location counter as a previously added linking record. Current linking record will be discarded. Linking continues.
     - RelocationErrorTest1_

   * - ES.47
     - Location of modify record will be relocated out of bounds of memory. modify record discarded. Linking continues.
     - 

   * - ES.48
     - Location of modify record will be relocated our of bounds of module. modify record discarded. Linking continues.
     - RelocationErrorTest1_

   * - ES.49
     - The modify record being added to the module has the same location counter as a previously added modify record. Current modify record will be discarded. Linking continues.
     - RelocationErrorTest1_

   * - ES.50
     - While evaluating modify record adjustments, an improper operator has been encountered. This probably means something wrong happened while parsing. Adjustment will be skipped. Linking continues.
     - 

   * - ES.51
     - While evaluating modify record adjustments, an entry was requested that does not exist in the symbol table. This probably means something wrong happened while parsing. Adjustment will be skipped. Linking continues.
     - LinkingErrorTest1_, LinkingErrorTest2_, ModifyErrorTest2_, RelocationErrorTest1_

   * - ES.52
     - Error when writing object load file to disk. Check your permissions. Object file will be printed to the screen.
     - 

   * - ES.53
     - Total number of text records should not be less than the total program length. Object file will be written, check it for errors.
     - 

   * - ES.54
     - Error when writing object load file to disk. Check your permissions. Some of the object file may have been written to disk. Check for errors.
     - 

   * - ES.55
     - Error opening input file. File will be skipped. Linking continues.
     - 

   * - ES.56
     - While evaluating modify record adjustments, a modify record modified a location that does not exist in this module. Modify record is ignored. Linking continues.
     - RelocationErrorTest1_

   * - ES.57
     - No valid input files found. At least one input file must be a valid FFA-ASM object file.
     - 

   * - EF.01
     - Header record has the incorrect number of fields. Stopping linker.
     - HeaderErrorTest2_

   * - EF.02
     - Assembler assigned location value must be valid hex. Stopping linker.
     - HeaderErrorTest3_

   * - EF.03
     - Assembler assigned location value must be in the range of 0-1023. Stopping linker.
     - HeaderErrorTest4_

   * - EF.04
     - Module length must be valid hex. Stopping Linker.
     - HeaderErrorTest5_

   * - EF.05
     - Module length must be in the range of 0-1024. Stopping linker.
     - HeaderErrorTest6_

   * - EF.06
     - Execution start address must be valid hex. Stopping linker.
     - HeaderErrorTest7_

   * - EF.07
     - Execution start address must be in the range of 0-1023. Stopping linker.
     - HeaderErrorTest8_

   * - EF.08
     - Total number of text records must be in the range of 0 to ModuleLength but is not. Stopping linker.
     - HeaderErrorTest9_

.. _EndErrorTest1: testlink__end_error_test1.html
.. _HeaderErrorTest1: testlink__header_error_test1.html
.. _HeaderErrorTest2: testlink__header_error_test2.html
.. _HeaderErrorTest3: testlink__header_error_test3.html
.. _HeaderErrorTest4: testlink__header_error_test4.html
.. _HeaderErrorTest5: testlink__header_error_test5.html
.. _HeaderErrorTest6: testlink__header_error_test6.html
.. _HeaderErrorTest7: testlink__header_error_test7.html
.. _HeaderErrorTest8: testlink__header_error_test8.html
.. _HeaderErrorTest9: testlink__header_error_test9.html
.. _InputAfterEndTest1: testlink__input_after_end_test1.html
.. _LinkingErrorTest1: testlink__linking_error_test1.html
.. _LinkingErrorTest2: testlink__linking_error_test2.html
.. _LinkingSymbolErrorTest1: testlink__linking_symbol_error_test1.html
.. _ModifyErrorTest1: testlink__modify_error_test1.html
.. _ModifyErrorTest2: testlink__modify_error_test2.html
.. _RelocationErrorTest1: testlink__relocation_error_test1.html
.. _TextErrorTest1: testlink__text_error_test1.html
