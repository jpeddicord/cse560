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

This is highly dependent on `Unit Testing`, as the correctness of individual parts effects the outcome of the overall program execution.

Unit Testing
============

Unit testing will use the NUnit testing framework. Each component will have several tests associated to throughly test the functionality and correctnes of the result.


`Tokenizer Test List`
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


`Directive Test List`
-------------------

Test Number: D1
```````````````
Puspose:
	Ensures Contains() works for a directive known to exist exactly as it is found in the source file.
Expected Results Achieved:
	Yes
Concerns:
	None
	
Test Number: D2
```````````````
Puspose:
	Ensures Contains() returns false for a directive that does not exist.
Expected Results Achieved:
	Yes
Concerns:
	None
	
Test Number: D3
```````````````
Puspose:
	Ensures that directives can be upper, lower, or mixed case.  This gives the user more flexibility in input.
Expected Results Achieved:
	Yes
Concerns:
	None
	
Test Number: D4
```````````````
Puspose:
	This test serves two purposes. It tests that all directives are being read in from the file and that DirectiveCount is returning the correct number of directives.
Expected Results Achieved:
	Yes
Concerns:
	Because the count is dependent of all directives being correctly read in, this test could still pass even if all directives have not been read in.  Another test has been added so we can be more confident that it is functioning properly.
	
Test Number: D5
```````````````
Puspose:
	This is an extension of test [D4] to show all directives are being read in by checking that both the first and last directive from the text file and be found.
Expected Results Achieved:
	Yes
Concerns:
	None
