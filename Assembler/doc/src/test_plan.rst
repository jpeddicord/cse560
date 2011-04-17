============
Testing Plan
============

Our testing plan for the assembler will comprise of two parts: unit testing of individual components, and testing of the overall functionality and correctness of the whole program.

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

Test Number: T1
```````````````
Purpose:
	This test is just to ensure basic functionality of the Tokenizer is being met.  Given a line of code, the Tokenizer should return the first word (all characters before the first space, comma, tab, or end of the line).
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: T2
```````````````
Purpose:
	This test is used to ensure the tokenizer is unaffected by adding extra whitespace.  It should remove all padding so that the token does not have any whitespace in it.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: T3
```````````````
Purpose:
	To tokenize an entire line and ensure that all tokens in the line are correctly identified and returned.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: T4
```````````````
Purpose:
	To tokenize an entire line with added whitespace and ensure all unneeded whitespace is properly removed and all tokens are identified and returned.
Expected Results Achieved:
    Yes
Concerns:
    If whitespace is added in the operand field, it will not recognize that it shouldn't be there.  This should be handled while parsing.

Test Number: T5
```````````````
Purpose:
	Tokenizing a line of code that has mixed case letters.  The user shouldn't have to worry about the case they are writing the code in so the tokenizer should be able to handle both upper and lower case.
Expected Results Achieved:
    Yes
Concerns:
    Unable to properly check both token text and kind.  Will add another test.

Test Number: T6
```````````````
Purpose:
	This test is an extension of test number T5.  Again it is testing a line written in mixed case, however this time it is checking the token kinds returned are correct instead of the token text.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: T7
```````````````
Purpose:
	Ensures the Tokenizer is able to correctly identify a token of kind Label_Or_Command.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: T8
```````````````
Purpose:
	Ensures the Tokenizer is able to correctly identify a token of kind Literal.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: T9
```````````````
Purpose:
    Ensures the Tokenizer is able to correctly identify a token of kind Comment.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: T10
````````````````
Purpose:
    Ensures the Tokenizer is able to correctly identify a token of kind Number.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: T11
````````````````
Purpose
    Ensures the Tokenizer is able to correctly identify a token of kind Empty.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: T12
````````````````
Purpose:
    Ensures the Tokenizer is able to correctly identify a token of kind Error.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: T13
````````````````
Purpose:
    Ensures the Tokenizer is able to correctly identify a token of kind JumpCond.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: T14
````````````````
Purpose:
    Ensures the Tokenizer is able to correctly identify a token of kind Expression.
Expected Results Achieved:
    Yes
Concerns:
    Labels and numbers within the expression are not tested for correct syntax by tokenizer, must be done elsewhere in the program.


Directive Test List
-------------------

Test Number: D1
```````````````
Purpose:
	Ensures Contains() works for a directive known to exist exactly as it is found in the source file.
Expected Results Achieved:
	Yes
Concerns:
	None
	
Test Number: D2
```````````````
Purpose:
	Ensures Contains() returns false for a directive that does not exist.
Expected Results Achieved:
	Yes
Concerns:
	None
	
Test Number: D3
```````````````
Purpose:
	Ensures that directives can be upper, lower, or mixed case.  This gives the user more flexibility in input.
Expected Results Achieved:
	Yes
Concerns:
	None
	
Test Number: D4
```````````````
Purpose:
	This test serves two purposes. It tests that all directives are being read in from the file and that DirectiveCount is returning the correct number of directives.
Expected Results Achieved:
	Yes
Concerns:
	Because the count is dependent of all directives being correctly read in, this test could still pass even if all directives have not been read in.  Another test has been added so we can be more confident that it is functioning properly.
	
Test Number: D5
```````````````
Purpose:
	This is an extension of test [D4] to show all directives are being read in by checking that both the first and last directive from the text file and be found.
Expected Results Achieved:
	Yes
Concerns:
	None


Instruction Test List
---------------------

Test Number: I1
```````````````
Purpose:
	A known instruction is shown to exist.
Expected Results Achieved:
	Yes
Concerns:
	None
	
Test Number: I2
```````````````
Purpose:
	Calling IsInstruction() with an existing group but nonexisting instruction should return false.
Expected Results Achieved:
	Yes
Concerns:
	None
	
Test Number: I3
```````````````
Purpose:
	IsInstruction() should be case insensitive.
Expected Results Achieved:
	Yes
Concerns:
	None
	
Test Number: I4
```````````````
Purpose:
	Calling IsInstruction with a group that doesn't exist but and instruction that does should return false.
Expected Results Achieved:
	Yes
Concerns:
	None
	
Test Number: I5
```````````````
Purpose:
	Calling IsInstruction with a group and instruction that doesn't exist should return false.
Expected Results Achieved:
	Yes
Concerns:
	None
	
Test Number: I6
```````````````
Purpose:
	Testing that IsGroup will return true for a group that is known to exist.
Expected Results Achieved:
	Yes
Concerns:
	None
	
Test Number: I7
```````````````
Purpose:
	Testing that IsGroup will return false for a group that does not exist.
Expected Results Achieved:
	Yes
Concerns:
	None
	
Test Number: I8
```````````````
Purpose:
	Ensures that IsGroup is case insensitive and sill return the correct result for both upper and lower case.
Expected Results Achieved:
	Yes
Concerns:
	None
	
Test Number: I9
```````````````
Purpose:
	Tests that getBytecodeString will return the code for the given instruction.
Expected Results Achieved:
	No
Concerns:
	This test revealed that when the bytecode was removed from the input file, it removed an appended carriage return as well which was unwanted.  This has since been corrected and now correctly passes this test.
	
Test Number: I10
````````````````
Purpose:
	Testing that getBytecodeString will throw an exception if the user attempts to look up a group that doesn't exist.
Expected Results Achieved:
	Yes
Concerns:
	None
	
Test Number: I11
````````````````
Purpose:
	Testing that getBytecodeString will throw an exception if the user attempts to look up an instruction that doesn't exist.
Expected Results Achieved:
	Yes
Concerns:
	None


Symbol Table Test List
----------------------

Test Number: S1
```````````````
Purpose:
    Tests adding a symbol by structure works.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: S2
```````````````
Purpose:
    Test adding a symbol by parameters.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: S3
```````````````
Purpose:
    Test the sorted output of symbols.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: S4
```````````````
Purpose:
    Test that an empty table is indeed empty.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: S5
```````````````
Purpose:
    Test that adding a duplicate symbol fails.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: S6
```````````````
Purpose:
    Test that looking up a nonexisting symbol fails.
Expected Results Achieved:
    Yes
Concerns:
    None


Binary Helper Test List
-----------------------

Test Number: B1
```````````````
Purpose:
    Tests that a negative number can be correctly converted to two's complement representation using 10 digits.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: B2
```````````````
Purpose:
    Tests that a negative number can be correctly converted to two's complement representation at the max digits of 16.
Expected Results Achieved:
    Yes
Concerns:
    None


Test Number: B3
```````````````
Purpose:
    Tests that a number in 10 digit two's complement can be correctly converted to its non-2's complement representation.
Expected Results Achieved:
    Yes
Concerns:
    None
	
Test Number: B4
```````````````
Purpose:
    Tests that a number in 8 digit two's complement can be correctly converted to its non-2's complement representation.
Expected Results Achieved:
    Yes
Concerns:
    None
	
Test Number: B5
```````````````
Purpose:
    Testing conversion of the smallest number to its negative two's complement representation with max digits (16).  This is an edge case.
Expected Results Achieved:
    Yes
Concerns:
    None
	
Test Number: B6
```````````````
Purpose:
    Testing conversion of a smallest two's complement to its negative corresponding value.  This is an edge case.
Expected Results Achieved:
    Yes
Concerns:
    None
	
Test Number: B7
```````````````
Purpose:
	Testing conversion of the largest number to its negative two's complement representation with max digits (16).  This is an edge case.
Expected Results Achieved:
    Yes
Concerns:
    None
	
Test Number: B8
```````````````
Purpose:
    Testing conversion of the largest two's complement to its negative corresponding value.  This is an edge case.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: B9
```````````````
Purpose:
    Testing conversion of that largest possible two's complement using two bits to its corresponding negative value.  This is an edge case using a different number of digits for further testing.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: B10
````````````````
Purpose:
    Testing conversion of the smallest possible two's complement using two bits to its corresponding negative value. This is an edge case.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: B11
````````````````
Purpose:
    Testing conversion of the largest negative number using 2 bits to its two's complement representation.  This is an edge case.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: B12
````````````````
Purpose:
    Testing numbers outside of the expected range.  This should return 0 rather than some other arbitrary (and incorrect) value.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: B13
````````````````
Purpose:
    Testing that providing a number below the expected range will also return 0.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: B14
````````````````
Purpose:
    Testing that when given the number 0, no conversions are made.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: B15
````````````````
Purpose:
    Testing a number that does not have a 1 in the most significant bit.  In otherwords, this value should be a positive number that is represented the same way regardless of its notation.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: B16
````````````````
Purpose:
    Testing that a value known to be in range returns true when provided to IsInRange.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: B17
````````````````
Purpose:
    Testing the largest possible value that should still be considered in range.  This is an edge case.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: B18
````````````````
Purpose:
    Testing the smallest possible value that should still be considered in range.  This is an edge case.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: B19
````````````````
Purpose:
    Checking just above the range of acceptable input using 10 digits. This should return false.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: B20
````````````````
Purpose:
    Checking just below the range of acceptable input using 10 digits.  This should return false.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: B21
````````````````
Purpose:
    Testing conversion of a hexadecimal number into binary.
Expected Results Achieved:
    Yes
Concerns:
    This will fail on integers that don't fit into 32 bits. However, since our machine is only 16-bit, this shouldn't be an issue.

Test Number: B22
````````````````
Purpose:
    Test that converting hex in 5 bits (arbitrary) results in the correct 32-bit integer.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: B23
````````````````
Purpose:
    Test converting a 10-bit 2's complement negative hexadecimal integer into a decimal integer works.
Expected Results Achieved:
    Yes
Concerns:
    None

Test Number: B24
````````````````
Purpose:
    Test that positive hex integers are still positive as decimal.
Expected Results Achieved:
    Yes
Concerns:
    None

