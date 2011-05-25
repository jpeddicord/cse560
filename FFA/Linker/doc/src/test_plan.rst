============
Testing Plan
============

The test plan for the linker consists of various test input files run through linking, and then output is later observed and manually reviewed for errors.
Testing of individual instructions proves to be difficult when each instruction depends on memory, the current state of the machine, or even other instructions. So, for the linker, we're following a **top-down** approach to testing.

.. contents::
   :backlinks: none
   :depth: 2

Overall Program Testing
=======================

Testing for the Linker relies on general program testing. We have created some test programs which test various aspects of the
linker. Some just test that general functionality is there, while others have been edited to contain errors
and will trip some of our error messages.

Test cases are provided in the following list. Click on a test case to see the test program input, and the linked load output. Program input is in the form of an assembled and linked solution. Output provided in these test cases is run with debug mode both off and on, so that the execution of individual instructions can be traced.

Sample test programs
--------------------

.. include:: ../tmp/testlink_index.rst

