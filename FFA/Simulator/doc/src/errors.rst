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
     - `ErrorTest3 <testsim__errortest3.html>`_

   * - EF.03
     - Wrong number of fields in header record.
     - `ErrorTest3 FIXME <testsim__errortest3.html>`_

   * - EF.04
     - Invalid load address in header record.
     - `ErrorTest6 <testsim__errortest6.html>`_

   * - EF.05
     - Invalid execution start address in header record.
     - `ErrorTest7 <testsim__errortest7.html>`_

   * - EF.06
     - Invalid program length field in header record.
     - `ErrorTest8 <testsim__errortest8.html>`_

   * - EF.07
     - Error opening input file; cannot continue.
     - Cannot provide a test file, tested by giving a file that did not exist.

   * - EF.08
     - Location counter out of range of memory.
     - `ErrorTest5 <testsim__errortest5.html>`_

   * - EF.09
     - Unable to fit program in memory. Program length or execution start is too large.
     - `ErrorTest4 <testsim__errortest4.html>`_

   * - ES.01
     - Missing version number of linker, or incorrect format.
     - `ErrorTest2 FIXME5 <testsim__errortest2.html>`_

   * - ES.02
     - Wrong number of fields in text record.
     - `ErrorTest2 FIXME8 <testsim__errortest2.html>`_

   * - ES.03
     - Wrong number of fields in end record.
     - `ErrorTest2 FIXME12 <testsim__errortest2.html>`_

   * - ES.05
     - Duplicate location counter found, ignoring line.
     - `ErrorTest2 FIXME9 <testsim__errortest2.html>`_

   * - ES.06
     - Unknown record type.
     - `ErrorTest2 FIXME11 <testsim__errortest2.html>`_

   * - ES.07
     - Invalid memory location.
     - `ErrorTest2 FIXME9 <testsim__errortest2.html>`_

   * - ES.08
     - Invalid word size or format.
     - `ErrorTest2 FIXME10 <testsim__errortest2.html>`_

   * - ES.09
     - Garbage data found after program end.
     - `ErrorTest2 FIXME13 <testsim__errortest2.html>`_

   * - ES.10
     - Invalid flag for DUMP. Can only accept 1, 2 or 3.
     - `ErrorTest1 FIXME2 <testsim__errortest1.html>`_

   * - ES.11
     - Resulting data value is out of range for data stack or memory.
     - `ErrorTest1 <testsim__errortest1.html>`_

   * - ES.12
     - Division by zero.
     - `ErrorTest1 FIXME <testsim__errortest1.html>`_

   * - ES.14
     - Invalid program name in header record.
     - `ErrorTest2 <testsim__errortest2.html>`_

   * - ES.18
     - Invalid date format in header record.
     - `ErrorTest2 FIXME <testsim__errortest2.html>`_

   * - ES.19
     - Invalid total number of records specified in header record.
     - `ErrorTest2 FIXME2 <testsim__errortest2.html>`_

   * - ES.20
     - Text record count must match total length field in header record.
     - `ErrorTest2 FIXME3 <testsim__errortest2.html>`_

   * - ES.21
     - FFA-LLM field missing or incorrect in header.
     - `ErrorTest2 FIXME4 <testsim__errortest2.html>`_

   * - ES.22
     - JUMP ^=, >=, and <= are not supported.
     - `ErrorTest1 FIXME3 <testsim__errortest1.html>`_

   * - ES.23
     - No input given. Ignoring push to stack.
     - `ErrorTest2 FIXME15 <testsim__errortest2.html>`_

   * - ES.24
     - Given integer must be between -32768 and 32767. Ignoring push to stack.
     - `ErrorTest2 FIXME16 <testsim__errortest2.html>`_

   * - ES.25
     - Given input must be an integer but was not. Ignoring push to stack.
     - `ErrorTest2 FIXME17 <testsim__errortest2.html>`_

   * - ES.26
     - Address is out of range of memory.
     - `ErrorTest5 FIXME2 <testsim__errortest5.html>`_

   * - ES.27
     - Too many items on the data stack, unable to add more. Ignoring push to stack.
     - `ErrorTest1 FIXME4 <testsim__errortest1.html>`_

   * - ES.28
     - Too many items on the test stack, unable to add more. Ignoring push to stack.
     - `ErrorTest1 FIXME5 <testsim__errortest1.html>`_

   * - ES.29
     - Data stack is empty, unable to remove an item.
     - `ErrorTest1 FIXME6 <testsim__errortest1.html>`_

   * - ES.30
     - Test stack is empty, unable to remove an item.
     - `ErrorTest1 FIXME7 <testsim__errortest1.html>`_

   * - ES.31
     - Value is too large to fit in memory. Must be within 0 and 65535.
     - Unable to be tripped from a test program, only by calling a procedure in the Simulator incorrectly.

   * - EW.01
     - Invalid character entry, too many characters given. Accepting up to the first two and ignoring the rest.
     - `ErrorTest2 FIXME6 <testsim__errortest2.html>`_

   * - EW.02
     - Halt code out of range. Setting code to 1023.
     - Unable to be tripped from a test program, only by calling a procedure in the Simulator incorrectly.

   * - EW.03
     - Program name of record doesn't match header.
     - `ErrorTest2 FIXME7 <testsim__errortest2.html>`_

.. include:: ../tmp/errorlist.rst
