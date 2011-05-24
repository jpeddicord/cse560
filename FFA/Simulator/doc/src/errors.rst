======
Errors
======

Below is a list of potential errors you may come across when running a simulated program:

.. list-table::
   :widths: 5 60 15
   :header-rows: 1
   :stub-columns: 1
   
   * - Error #
     - Message
     - Tested In

   * - EF.01
     - Memory reference is out of range. Can only access 0 to 1023.
     - Unable to be tripped from a test program, only by calling a procedure in the Simulator incorrectly.

   * - EF.02
     - Invalid header data; cannot continue.
     - ErrorTest3_

   * - EF.03
     - Wrong number of fields in header record.
     - ErrorTest3_

   * - EF.04
     - Invalid load address in header record.
     - ErrorTest6_

   * - EF.05
     - Invalid execution start address in header record.
     - ErrorTest7_

   * - EF.06
     - Invalid program length field in header record.
     - ErrorTest8_

   * - EF.07
     - Error opening input file; cannot continue.
     - Cannot provide a test file, tested by giving a file that did not exist.

   * - EF.08
     - Location counter out of range of memory.
     - ErrorTest5_

   * - EF.09
     - Unable to fit program in memory. Program length or execution start is too large.
     - ErrorTest4_

   * - ES.01
     - Missing version number of linker, or incorrect format.
     - ErrorTest2_

   * - ES.02
     - Wrong number of fields in text record.
     - ErrorTest2_

   * - ES.03
     - Wrong number of fields in end record.
     - ErrorTest2_

   * - ES.05
     - Duplicate location counter found, ignoring line.
     - ErrorTest2_

   * - ES.06
     - Unknown record type.
     - ErrorTest2_

   * - ES.07
     - Invalid memory location.
     - ErrorTest2_

   * - ES.08
     - Invalid word size or format.
     - ErrorTest2_

   * - ES.09
     - Garbage data found after program end.
     - ErrorTest2_

   * - ES.10
     - Invalid flag for DUMP. Can only accept 1, 2 or 3.
     - ErrorTest1_

   * - ES.11
     - Resulting data value is out of range for data stack or memory.
     - ErrorTest1_

   * - ES.12
     - Division by zero.
     - ErrorTest1_

   * - ES.14
     - Invalid program name in header record.
     - ErrorTest2_

   * - ES.18
     - Invalid date format in header record.
     - ErrorTest2_

   * - ES.19
     - Invalid total number of records specified in header record.
     - ErrorTest2_

   * - ES.20
     - Text record count must match total length field in header record.
     - ErrorTest2_

   * - ES.21
     - FFA-LLM field missing or incorrect in header.
     - ErrorTest2_

   * - ES.22
     - JUMP ^=, >=, and <= are not supported.
     - ErrorTest1_

   * - ES.23
     - No input given. Ignoring push to stack.
     - ErrorTest2_

   * - ES.24
     - Given integer must be between -32768 and 32767. Ignoring push to stack.
     - ErrorTest2_

   * - ES.25
     - Given input must be an integer but was not. Ignoring push to stack.
     - ErrorTest2_

   * - ES.26
     - Address is out of range of memory.
     - ErrorTest5_

   * - ES.27
     - Too many items on the data stack, unable to add more. Ignoring push to stack.
     - ErrorTest1_

   * - ES.28
     - Too many items on the test stack, unable to add more. Ignoring push to stack.
     - ErrorTest1_

   * - ES.29
     - Data stack is empty, unable to remove an item.
     - ErrorTest1_

   * - ES.30
     - Test stack is empty, unable to remove an item.
     - ErrorTest1_

   * - ES.31
     - Value is too large to fit in memory. Must be within 0 and 65535.
     - Unable to be tripped from a test program, only by calling a procedure in the Simulator incorrectly.

   * - EW.01
     - Invalid character entry, too many characters given. Accepting up to the first two and ignoring the rest.
     - ErrorTest2_

   * - EW.02
     - Halt code out of range. Setting code to 1023.
     - Unable to be tripped from a test program, only by calling a procedure in the Simulator incorrectly.

   * - EW.03
     - Program name of record doesn't match header.
     - ErrorTest2_

.. _ErrorTest1: testsim__errortest1.html
.. _ErrorTest2: testsim__errortest2.html
.. _ErrorTest3: testsim__errortest3.html
.. _ErrorTest4: testsim__errortest4.html
.. _ErrorTest5: testsim__errortest5.html
.. _ErrorTest6: testsim__errortest6.html
.. _ErrorTest7: testsim__errortest7.html
.. _ErrorTest8: testsim__errortest8.html
