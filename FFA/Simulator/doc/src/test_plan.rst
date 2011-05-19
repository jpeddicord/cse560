============
Testing Plan
============

THIS NEEDS TO BE EDITED!

Our testing plan for the assembler will comprise of two parts: unit testing of individual components,
and testing of the overall functionality and correctness of the whole program. Our focus is on bottom-up
testing: creating test cases for individual procedures as they are created to ensure our entire program
will function as intended. Most of our team's unit testing experience has come from Java with the use
of JUnit tests.  Due to the fact we are not using Java for this project, we were required to find an
alternative. We chose to use `NUnit <http://www.nunit.org/>`_ which is very similar in functionality to
JUnit but is compatible with C#.  Below we have a list of all of the current unit tests.  Included with
each test is a test number, the purpose of the test, if the expected results have been acheived, concerns
with the test (either concerns that there may be something wrong with the test, that it doesn't test
exactly how we want it to, or if a test failed and how it was fixed), as well as a link to the individual
test cases to show you exactly how we perform the specified test.

.. contents::
   :backlinks: none
   :depth: 2

Overall Program Testing
=======================

Some procedures are difficult to test with unit tests alone, specifically parsing. We must still ensure that
this is working correctly and we do so by checking overall program functionality by giving the parser various
test programs to compile.  A link to each of these test programs and the output given by our assembler are
given below. Each test is run when updates are made to the assembler and the online documentation is updated
automatically, so all current online for these tests should be up to date. The output of each run will be manually
reviewed by the team, and will determine if the output is giving what is expected and if further work is needed.

This is highly dependent on `Unit Testing`_, as the correctness of individual parts effects the outcome of the overall program execution.

Sample test programs
--------------------

XXXX .. include:: ../tmp/testscript_index.rst

Unit Testing
============

Unit testing uses the NUnit testing framework. Each component will have several tests associated to throughly test the functionality and correctness of the result.

Below are tables for all of the unit tests currently available for the Assembler. Above each table is a link which will bring you to the code for the unit tests.
This will let you see exactly how we are accomplishing the test. In each table there are rows with test numbers. These test numbers begin with an abbreviation
(usually the first letter or two) of the class or component that is being tested and is followed by a number starting with 1 and increasing based on the number
of tests for each class. These test numbers are also located just before the unit test that implements it in the code. The test number is then followed by a short
description of the purpose. We then say if the expected results were achieved or not. If they were not then we will provide a description of why it didn't work,
and how the issue was fixed. If our original fix still doesn't work, any further modifications to get the test to run properly will also be noted in the
concerns column. There may also be concerns even if the expected results were achieved.

XXXXXXXXXXXX Test List
----------------------

Test case sources are available in `TokenizerTest.cs <_tokenizer_test_8cs_source.html>`_.

.. list-table::
   :widths: 5 50 5 40
   :header-rows: 1
   :stub-columns: 1
   
   * - Test #
     - Purpose
     - Expected Results Achieved
     - Concerns

   * - T1
     - This test is just to ensure basic functionality of the Tokenizer is being met.  Given a line of code, the Tokenizer should return the first word (all characters before the first space, comma, tab, or end of the line).
     - Yes
     - None

