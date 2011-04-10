=============
Errata Report
=============

To be written.

Date: 4/7/2011
Error: Tokenizer broke on empty strings. Tokenizer was set up to check for comments before breaking apart the string.  To do this it would check if the first character was ':' as this would signify that the rest of the line was a comment.
Solution: Tokenizer now checks that the length of the line of code is at least 1 before checking to see if the first character is ':'.

Date: 4/9/2011
Error: Checking valid operand field gave an incorrect result when the line began with whitespace.  This only only occurred when the line had multiple spaces or tabs in between tokens.
Solution: Altered Tokenizer to strip leading white space from the altered line of code that was returned.
