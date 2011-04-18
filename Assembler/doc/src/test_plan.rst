============
Testing Plan
============

Our testing plan for the assembler will comprise of two parts: unit testing of individual components, and testing of the overall functionality and correctness of the whole program. Our focus is on bottom-up testing: individual components of the program are tested, and this helps ensure the correct operation of the overall program.

.. contents::
   :backlinks: none
   :depth: 2

Overall Program Testing
=======================

Tests of the program functionality will be conducted by giving the program various test applicaitons to compile. The output of each run will be manually reviewed by the team, and will determine if further work is needed.

This is highly dependent on `Unit Testing`_, as the correctness of individual parts effects the outcome of the overall program execution.

Sample test programs
--------------------

.. include:: ../tmp/testscript_index.rst

Unit Testing
============

Unit testing will use the NUnit testing framework. Each component will have several tests associated to throughly test the functionality and correctnes of the result.


Tokenizer Test List
-------------------

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

   * - T2
     - This test is used to ensure the tokenizer is unaffected by adding extra whitespace.  It should remove all padding so that the token does not have any whitespace in it.
     - Yes
     - None

   * - T3
     - To tokenize an entire line and ensure that all tokens in the line are correctly identified and returned.
     - Yes
     - None

   * - T4
     - To tokenize an entire line with added whitespace and ensure all unneeded whitespace is properly removed and all tokens are identified and returned.
     - Yes
     - If whitespace is added in the operand field, it will not recognize that it shouldn't be there.  This should be handled while parsing.

   * - T5
     - Tokenizing a line of code that has mixed case letters.  The user shouldn't have to worry about the case they are writing the code in so the tokenizer should be able to handle both upper and lower case.
     - Yes
     - Unable to properly check both token text and kind.  Will add another test.

   * - T6
     - This test is an extension of test number T5.  Again it is testing a line written in mixed case, however this time it is checking the token kinds returned are correct instead of the token text.
     - Yes
     - None

   * - T7
     - Ensures the Tokenizer is able to correctly identify a token of kind Label_Or_Command.
     - Yes
     - None

   * - T8
     - Ensures the Tokenizer is able to correctly identify a token of kind Literal.
     - Yes
     - None

   * - T9
     - Ensures the Tokenizer is able to correctly identify a token of kind Comment.
     - Yes
     - None

   * - T10
     - Ensures the Tokenizer is able to correctly identify a token of kind Number.
     - Yes
     - None

   * - T11
     - Ensures the Tokenizer is able to correctly identify a token of kind Empty.
     - Yes
     - None

   * - T12
     - Ensures the Tokenizer is able to correctly identify a token of kind Error.
     - Yes
     - None

   * - T13
     - Ensures the Tokenizer is able to correctly identify a token of kind JumpCond.
     - Yes
     - None

   * - T14
     - Ensures the Tokenizer is able to correctly identify a token of kind Expression.
     - Yes
     - Labels and numbers within the expression are not tested for correct syntax by tokenizer, must be done elsewhere in the program.


Directive Test List
-------------------

.. list-table::
   :widths: 5 50 5 40
   :header-rows: 1
   :stub-columns: 1
   
   * - Test #
     - Purpose
     - Expected Results Achieved
     - Concerns

   * - D1
     - Ensures Contains() works for a directive known to exist exactly as it is found in the source file.
     - Yes
     - None
	
   * - D2
     - Ensures Contains() returns false for a directive that does not exist.
     - Yes
     - None
	
   * - D3
     - Ensures that directives can be upper, lower, or mixed case.  This gives the user more flexibility in input.
     - Yes
     - None
	
   * - D4
     - This test serves two purposes. It tests that all directives are being read in from the file and that DirectiveCount is returning the correct number of directives.
     - Yes
     - Because the count is dependent of all directives being correctly read in, this test could still pass even if all directives have not been read in.  Another test has been added so we can be more confident that it is functioning properly.
	
   * - D5
     - This is an extension of test [D4] to show all directives are being read in by checking that both the first and last directive from the text file and be found.
     - Yes
     - None


Instruction Test List
---------------------

.. list-table::
   :widths: 5 50 5 40
   :header-rows: 1
   :stub-columns: 1
   
   * - Test #
     - Purpose
     - Expected Results Achieved
     - Concerns

   * - I1
     - A known instruction is shown to exist.
     - Yes
     - None
	
   * - I2
     - Calling IsInstruction() with an existing group but nonexisting instruction should return false.
     - Yes
     - None
	
   * - I3
     - IsInstruction() should be case insensitive.
     - Yes
     - None
	
   * - I4
     - Calling IsInstruction with a group that doesn't exist but and instruction that does should return false.
     - Yes
     - None
	
   * - I5
     - Calling IsInstruction with a group and instruction that doesn't exist should return false.
     - Yes
     - None
	
   * - I6
     - Testing that IsGroup will return true for a group that is known to exist.
     - Yes
     - None
	
   * - I7
     - Testing that IsGroup will return false for a group that does not exist.
     - Yes
     - None
	
   * - I8
     - Ensures that IsGroup is case insensitive and sill return the correct result for both upper and lower case.
     - Yes
     - None
	
   * - I9
     - Tests that getBytecodeString will return the code for the given instruction.
     - No
     - This test revealed that when the bytecode was removed from the input file, it removed an appended carriage return as well which was unwanted.  This has since been corrected and now correctly passes this test.
	
   * - I10
     - Testing that getBytecodeString will throw an exception if the user attempts to look up a group that doesn't exist.
     - Yes
     - None
	
   * - I11
     - Testing that getBytecodeString will throw an exception if the user attempts to look up an instruction that doesn't exist.
     - Yes
     - None


Symbol Table Test List
----------------------

.. list-table::
   :widths: 5 50 5 40
   :header-rows: 1
   :stub-columns: 1
   
   * - Test #
     - Purpose
     - Expected Results Achieved
     - Concerns

   * - S1
     - Tests adding a symbol by structure works.
     - Yes
     - None

   * - S2
     - Test adding a symbol by parameters.
     - Yes
     - None

   * - S3
     - Test the sorted output of symbols.
     - Yes
     - None

   * - S4
     - Test that an empty table is indeed empty.
     - Yes
     - None

   * - S5
     - Test that adding a duplicate symbol fails.
     - Yes
     - None

   * - S6
     - Test that looking up a nonexisting symbol fails.
     - Yes
     - None


Binary Helper Test List
-----------------------

.. list-table::
   :widths: 5 50 5 40
   :header-rows: 1
   :stub-columns: 1
   
   * - Test #
     - Purpose
     - Expected Results Achieved
     - Concerns

   * - B1
     - Tests that a negative number can be correctly converted to two's complement representation using 10 digits.
     - Yes
     - None

   * - B2
     - Tests that a negative number can be correctly converted to two's complement representation at the max digits of 16.
     - Yes
     - None

   * - B3
     - Tests that a number in 10 digit two's complement can be correctly converted to its non-2's complement representation.
     - Yes
     - None
	
   * - B4
     - Tests that a number in 8 digit two's complement can be correctly converted to its non-2's complement representation.
     - Yes
     - None
	
   * - B5
     - Testing conversion of the smallest number to its negative two's complement representation with max digits (16).  This is an edge case.
     - Yes
     - None
	
   * - B6
     - Testing conversion of a smallest two's complement to its negative corresponding value.  This is an edge case.
     - Yes
     - None
	
   * - B7
     - Testing conversion of the largest number to its negative two's complement representation with max digits (16).  This is an edge case.
     - Yes
     - None
	
   * - B8
     - Testing conversion of the largest two's complement to its negative corresponding value.  This is an edge case.
     - Yes
     - None

   * - B9
     - Testing conversion of that largest possible two's complement using two bits to its corresponding negative value.  This is an edge case using a different number of digits for further testing.
     - Yes
     - None

   * - B10
     - Testing conversion of the smallest possible two's complement using two bits to its corresponding negative value. This is an edge case.
     - Yes
     - None

   * - B11
     - Testing conversion of the largest negative number using 2 bits to its two's complement representation.  This is an edge case.
     - Yes
     - None

   * - B12
     - Testing numbers outside of the expected range.  This should return 0 rather than some other arbitrary (and incorrect) value.
     - Yes
     - None

   * - B13
     - Testing that providing a number below the expected range will also return 0.
     - Yes
     - None

   * - B14
     - Testing that when given the number 0, no conversions are made.
     - Yes
     - None

   * - B15
     - Testing a number that does not have a 1 in the most significant bit.  In otherwords, this value should be a positive number that is represented the same way regardless of its notation.
     - Yes
     - None

   * - B16
     - Testing that a value known to be in range returns true when provided to IsInRange.
     - Yes
     - None

   * - B17
     - Testing the largest possible value that should still be considered in range.  This is an edge case.
     - Yes
     - None

   * - B18
     - Testing the smallest possible value that should still be considered in range.  This is an edge case.
     - Yes
     - None

   * - B19
     - Checking just above the range of acceptable input using 10 digits. This should return false.
     - Yes
     - None

   * - B20
     - Checking just below the range of acceptable input using 10 digits.  This should return false.
     - Yes
     - None

   * - B21
     - Testing conversion of a hexadecimal number into binary.
     - Yes
     - This will fail on integers that don't fit into 32 bits. However, since our machine is only 16-bit, this shouldn't be an issue.

   * - B22
     - Test that converting hex in 5 bits (arbitrary) results in the correct 32-bit integer.
     - Yes
     - None

   * - B23
     - Test converting a 10-bit 2's complement negative hexadecimal integer into a decimal integer works.
     - Yes
     - None

   * - B24
     - Test that positive hex integers are still positive as decimal.
     - Yes
     - None


Errors Test List
----------------

.. list-table::
   :widths: 5 50 5 40
   :header-rows: 1
   :stub-columns: 1
   
   * - Test #
     - Purpose
     - Expected Results Achieved
     - Concerns

   * - E1
     - Checks that errors are correctly read in from the error file and seperated into the correct category.  This test checks that fatal errors are read in.
     - Yes
     - This test relies on ToString for Errors.  If ToString is changed, this test may give a false negative.

   * - E2
     - Checks that errors are correctly read in from the error file and seperated into the correct category.  This test checks that serious errors are read in.
     - Yes
     - This test relies on ToString for Errors.  If ToString is changed, this test may give a false negative.

   * - E3
     - Checks that errors are correctly read in from the error file and seperated into the correct category.  This test checks that warning errors are read in.
     - Yes
     - This test relies on ToString for Errors.  If ToString is changed, this test may give a false negative.

   * - E4
     - Testing that every line from the errors file is read in so no error messages are missing.
     - Yes
     - This test relies on ToString for Errors.  If ToString is changed, this test may give a false negative. Due to the fact that we will be adding more errors to the error file as we discover they are needed, the error in the current test may not always be the last one in the file.
