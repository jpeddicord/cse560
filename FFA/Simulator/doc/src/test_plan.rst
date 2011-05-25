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

Testing for the Simulator relies on both unit and general program testing. While unit tests provide us with an idea
of how well our implementations work, they make it difficult to test for `errors <errors.html>`_ and they don't
show how well everything works together. We have created some test programs which test various aspects of the
simulator. Some just test that general functionality is there, while others have been edited to contain errors
and will trip some of our error messages.

Test cases are provided in the following list. Click on a test case to see the test program input, and the simulated output. Program input is in the form of an assembled and linked solution. Output provided in these test cases is run with debug mode both off and on, so that the execution of individual instructions can be traced.

Test programs
-------------

.. include:: ../tmp/testsim_index.rst


Unit Testing
============

As with the Assembler, we used the NUnit Test Framework to conduct our unit tests.

These unit tests are aimed toward the actual implementations of various functions. The tests are looking for correct
output from the procedures, rather then error catching. All of our tests for error catching are done in the
overall program testing above. Some functions such as HALT and DUMP in CNTL are very difficult to test using
unit tests so these will be tested manually using test programs we provide to the Simulator. The below table lists
all of the unit tests as well as a link to the source to show exactly how they are implemented. Each test has a test
number (usually the first letter or two of the Instruction group they are testing followed by a number), a description
of what the test is looking for, if we achieved the desired output, and any concerns we had while running the test. If our concern was an error that
was revealed by the test we will also provide a description of what the issue was and how we fixed it.

CNTL Test List
--------------

Test case sources are available in `CNTLTest.cs <_c_n_t_l_test_8cs_source.html>`_.

.. list-table::
   :widths: 5 50 5 40
   :header-rows: 1
   :stub-columns: 1
   
   * - Test #
     - Purpose
     - Expected Results Achieved
     - Concerns

   * - C1
     - Tests that GOTO works with the minimum location counter by telling it to goto LC 0 and checking that LC is equal to 0.
     - Yes
     - None

   * - C2
     - Tests that GOTO works with the maximum location counter by telling it to goto LC 1023 and checking that LC is equal to 1023. 
     - Yes
     - None

   * - C3
     - Tests that GOTO works with an arbitrary location counter by telling it to goto LC 98 and checking that LC is equal to 98.
     - Yes
     - None

   * - C4
     - Tests that CLRD removes all items from the data stack. Adds an item to the data stack and then uses CLRD and checks that the data stack then has a size of 0.
     - Yes
     - None

   * - C5
     - Tests that CLRT removes all items from the test stack. Adds an item to the test stack and then uses CLRT and checks that the test stack then has a size of 0.
     - Yes
     - None



STACK Test List
---------------

Test case sources are available in `STACKTest.cs <_s_t_a_c_k_test_8cs_source.html>`_.

.. list-table::
   :widths: 5 50 5 40
   :header-rows: 1
   :stub-columns: 1
   
   * - Test #
     - Purpose
     - Expected Results Achieved
     - Concerns

   * - ST1
     - Tests that push correctly pushes the largest possible positive integer.
     - Yes
     - None

   * - ST2
     - Tests that push correctly pushes the smallest possible negative integer.
     - No
     - This test revealed an incorrect implementation for Stack Push. When negative values were stored as literals in the instructions, stack push interpretted them as 16 bit 2s complement numbers instead of 10 bit 2s complement which affected how it was being stored. This was corrected and now works the right way.

   * - ST3
     - Tests that push correctly pushes the largest possible negative integer.
     - Yes
     - None

   * - ST4
     - Tests that Pop works with the largest positive integer.
     - Yes
     - None

   * - ST5
     - Tests that Pop works with the smallest negative integer.
     - Yes
     - None

   * - ST6
     - Tests that Pop works with the largest negative integer.
     - Yes
     - None

   * - ST7
     - Test that STACK Test works when the item on the stack is less than the item in memory.
     - Yes
     - None

   * - ST8
     - Test that STACK Test works when the item on the stack is greater than the item in memory.
     - Yes
     - None

   * - ST9
     - Test that STACK Test works when the item on the stack is equal to the item in memory.
     - Yes
     - None


JUMP Test List
--------------

Test case sources are available in `JUMPTest.cs <_j_u_m_p_test_8cs_source.html>`_.

.. list-table::
   :widths: 5 50 5 40
   :header-rows: 1
   :stub-columns: 1
   
   * - Test #
     - Purpose
     - Expected Results Achieved
     - Concerns

   * - J1
     - Tests that jump sets the location counter when it is <
     - Yes
     - None

   * - J2
     - Tests that jump does not set the location counter when it is not <
     - Yes
     - None

   * - J3
     - Tests that jump sets the location counter when it is >
     - Yes
     - None

   * - J4
     - Tests that jump does not set the location counter when it is not >
     - Yes
     - None

   * - J5
     - Tests that jump sets the location counter when it is =
     - Yes
     - None

   * - J6
     - Tests that jump does not set the location counter when it is not =
     - Yes
     - None


SOPER Test List
---------------

Test case sources are available in `SOPERTest.cs <_s_o_p_e_r_test_8cs_source.html>`_.

.. list-table::
   :widths: 5 50 5 40
   :header-rows: 1
   :stub-columns: 1
   
   * - Test #
     - Purpose
     - Expected Results Achieved
     - Concerns

   * - SO1
     - Tests that Add works with multiple positive numbers.
     - Yes
     - None

   * - SO2
     - Tests that Add works with multiple negative numbers.
     - Yes
     - None

   * - SO3
     - Tests that Add works with multiple positive and negative numbers.
     - Yes
     - None

   * - SO4
     - Tests that Sub works with multiple positive numbers.
     - Yes
     - None

   * - SO5
     - Tests that Sub works with multiple negative numbers.
     - Yes
     - None

   * - SO6
     - Tests that Sub works with multiple positive and negative numbers.
     - Yes
     - None

   * - SO7
     - Tests that Mul works with multiple positive numbers.
     - Yes
     - None

   * - SO8
     - Tests that Mul works with multiple negative numbers.
     - Yes
     - None

   * - SO9
     - Tests that Mul works with multiple positive and negative numbers.
     - Yes
     - None

   * - SO10
     - Tests that Div works with multiple positive numbers.
     - Yes
     - None

   * - SO11
     - Tests that Div works with multiple negative numbers.
     - Yes
     - None

   * - SO12
     - Tests that Div works with multiple positive and negative numbers.
     - Yes
     - None

   * - SO13
     - Tests that Or works with multiple positive numbers.
     - Yes
     - None

   * - SO14
     - Tests that Or works with multiple negative numbers.
     - Yes
     - None

   * - SO15
     - Tests that Or works with multiple positive and negative numbers.
     - Yes
     - None

   * - SO16
     - Tests that And works with multiple positive numbers.
     - Yes
     - None

   * - SO17
     - Tests that And works with multiple negative numbers.
     - Yes
     - None

   * - SO18
     - Tests that And works with multiple positive and negative numbers.
     - Yes
     - None


MOPER Test List
---------------

Test case sources are available in `MOPERTest.cs <_m_o_p_e_r_test_8cs_source.html>`_.

.. list-table::
   :widths: 5 50 5 40
   :header-rows: 1
   :stub-columns: 1
   
   * - Test #
     - Purpose
     - Expected Results Achieved
     - Concerns

   * - M1
     - Tests that Add works with both positive numbers.
     - Yes
     - None

   * - M2
     - Tests that Add works with both negative numbers.
     - Yes
     - None

   * - M3
     - Tests that Add works with a positive and negative number.
     - Yes
     - None

   * - M4
     - Tests that Sub works with both positive numbers.
     - Yes
     - None

   * - M5
     - Tests that Sub works with both negative numbers.
     - Yes
     - None

   * - M6
     - Tests that Sub works with a positive and negative number.
     - Yes
     - None

   * - M7
     - Tests that Mul works with both positive numbers.
     - Yes
     - None

   * - M8
     - Tests that Mul works with both negative numbers.
     - Yes
     - None

   * - M9
     - Tests that Mul works with a positive and negative number.
     - Yes
     - None

   * - M10
     - Tests that Div works with both positive numbers.
     - Yes
     - None

   * - M11
     - Tests that Div works with both negative numbers.
     - Yes
     - None

   * - M12
     - Tests that Div works with a positive and negative number.
     - Yes
     - None

   * - M13
     - Tests that Or works with both positive numbers.
     - Yes
     - None

   * - M14
     - Tests that Or works with both negative numbers.
     - Yes
     - None

   * - M15
     - Tests that Or works with a positive and negative number.
     - Yes
     - None

   * - M16
     - Tests that And works with both positive numbers.
     - Yes
     - None

   * - M17
     - Tests that And works with both negative numbers.
     - Yes
     - None

   * - M18
     - Tests that And works with a positive and negative number.
     - Yes
     - None
