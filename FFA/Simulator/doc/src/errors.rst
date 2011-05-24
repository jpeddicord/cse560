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
	 - Tested In:

   * - EF.01
     - Memory reference is out of range. Can only access 0 to 1023.
     - Unable to provide test program due to structure of instructions.

   * - EF.02
     - Invalid header data; cannot continue.
     - `ErrorTest3 <testsim__errortest3.html>`_

   * - EF.03
     - Wrong number of fields in header record.
     - `ErrorTest3 <testsim__errortest3.html>`_

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


.. include:: ../tmp/errorlist.rst
