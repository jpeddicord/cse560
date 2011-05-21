============
Testing Plan
============

The test plan for the simulator consists of various test input files run through simulation, and then output is later observed and manually reviewed for errors.
Testing of individual instructions proves to be difficult when each instruction depends on memory, the current state of the machine, or even other instructions. So, for the simulator, we're following a **top-down** approach to testing.


.. contents::
   :backlinks: none
   :depth: 2

Overall Program Testing
=======================

Test cases are provided in the following list. Click on a test case to see the test program input, and the simulated output. Program input is in the form of an assembled and linked solution. Output provided in these test cases is run with debug mode on, so that the execution of individual instructions can be traced.

Test programs
-------------

.. include:: ../tmp/testscript_index.rst

