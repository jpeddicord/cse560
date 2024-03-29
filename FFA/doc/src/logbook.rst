=======
Logbook
=======

.. contents::
   :backlinks: none
   :depth: 1

May 24, 2011 - 10:00 pm
=======================
Nearly everything is finished and we are pretty happy with out program. Test programs have been created for both
the linker and the simulator that throw every error that can be thrown (some errors won't get thrown unless 
the simulator or linker procedures are used incorrectly which shouldn't be a problem unless a user goes poking around in the source code).
Nearly 60 unit cases were created to test implementations in the simulator. Only one of them actually revealed an
issue with our implementation. We like to think this is a result of great coding rather than bad testing. Our
test programs would have revealed other issues if it was bad coding. Documentation has been created for almost everything.
We have a few more things to touch up (such as finishing this log entry) and then we can submit.

It's almost sad that we are done with this.  But not really.

To to
-----
- Everyone - Celebrate!

May 22, 2011 - 9:00 pm
======================
Both the linker and the simulator are now functional! The linker still needs more error checking which Mark has
been working on lately. The simulator has most error chacking in place and we now need to shift our focus to
testing it. Because of the nature of the linker, it would be very difficult to test it using unit tests, so we
have decided to create many different test cases, some with errors that should be caught, and some without. The
simulator is a little different and has some procedures that can be tested using unit tests. So while we will test
what we can on the simulator using unit tests, it will also rely heavily on testing through test programs that
we create.

To do
-----
- Andrew - Create test programs that will throw every error on the error list for the simulator.
- Jacob - Begin working on documentation for Simulator and Linker, specifically, write a user guide/how to for both.
- Mark - Finish implementing error checking in the linker.

May 20, 2011 - 7:00 pm
======================
Andrew isn't able to attend our meetings currently as he has gone home for the weekend, however we are still able to
communicate with him online so he is up to date on the project and our plans.

We've spent the last couple days getting these projects off the ground. The linker isn't functioning yet but has a
pretty good start. We have decided to use the memory efficient way of linking after realizing it wouldn't really
be all that difficult to implement. The Simulator is also doing pretty well. Parsing of the load file is just about
finished and a little over half of the functions in FFA have been implemented. Andrew has been working on these and
plans on having them finished in the next day or two so we can begin testing them.

To do
-----
- Andrew - Finish implementations for FFA functions
- Jacob - Work on error catching in parsing and runtime for the simulator
- Mark - Continue work on linker

May 17, 2011 - 5:00 pm
======================
We've begun working on SP3/SP4 already since we only have about a week to get both done. Our group only has 3 members
but we have still decided to split the work to try to get everything done faster. Mark has started working on the linker
while Jacob and Andrew are splitting the work on the Simulator. Once the simulator is finished (which will probably be
done before the linker) we can all focus on the linker to get it working. Jacob has set up a new layout for
the online documentation but currently has it under a different URL so we don't interupt grading for SP2.

To do
-----
- Andrew and Jacob - Figure out how to structure the simulator. Come up with a list of possible errors that need to be caught so we are aware of them as we code.
- Mark - Begin working on the Linker. Decide if we want to take the lazy, or more memory conservative approach.

May 15, 2011 - 9:00 pm
======================
We are no longer aware of any bugs in our program. Perhaps the grading session will reveal some but we feel we have a quality project. 

To do
-----
- Everyone - Relax for now.

May 14, 2011 - 7:00 pm
======================
SP2 is now nearly complete though we do still have a few minor bugs that need to be worked out. The Assembler can successfully parse a source file
and generate correct object code. It also displayes an assembly report for the user. Documentation for SP2 has been updated including the format
for the object file. New test programs have been added.  We decided that due to the nature of SP2 it is easier to test its functionality strictly
through these test programs rather than trying to make unit tests. Similar to parse, it is hard to test individual parts because of the way it is
structured. We plan on waiting to turn this in until tomorrow so we can get the bugs fixed and browse through the documentation to see if anything
else needs to be added or updated.

To do
-----
- Mark - Fix bugs in assembling.
- Everyone - Look for issues in documentation.

May 10, 2011 - 5:00 pm
======================
Worked on getting pass 2 off of the ground. We're now able to correctly generate output for the midterm problem. Records are split out into multiple classes, which are contained in one ObjectFile to pull them together and render the correct output. We also added an AssemblyReport class to add the appropriate entries to.

To do
-----
- Mark - Get ADC/E and expressions working
- Jacob - Clean up current code and documentation
- Andrew - Create some more test programs to use when checking object code

May 3rd 2011 - 5:00 pm
======================
Documentation is looking better at this point.  Some reorganization was done to make it easier on the user and to accomodate new
documentation we will be creating for SP2. We've realized we will need to redo the organization again once we have to deal with
SP3 and SP4 as well as these are entirely different from the Assembler and will need their own sections.  Back and top links have
been made to assist users in navigating the documentation with less scrolling. The user guide has been updated with more detail and
pictures, however this will need to be updated again after the creation of SP2 to explain to the user the object file and new
Assembly report that will be created. The tables have also been updated with descriptions. We've has some discussion about getting SP2
working and as of now don't expect it to be too difficult. Because we did so much in pass 1, we should have a majority of hex code
calculated and really just need to deal with adc/adce and labels that were used before they were declared and of course output
all of the info in the correct format.

To do
-----
- Andrew - Create descriptions for each of the test cases to give a better idea of what we are testing.

April 27th 2011 - 7:00 pm
=========================
Our group took a week break from working on the project after SP1 was finished. Today we got our grade back for it, and while not
bad we aren't entirely pleased. While the program itself is in good condition we need to polish up or documentation quite a bit as
this is where we lost all of our points. Most of this was lost in the user guide and organization of our documentation so we 
plan to spend some more time in this area.  We want to get our SP1 up to the level expected before really getting into the features
required by SP2.

To do
-----
- Andrew - Work on the user guide. Ensure that it is more detailed by providing pictures, more examples, and better descriptions of how to run the program if the executable is not available.
- Jacob - Work on documentation organization such as adding top of page and back links and perhaps resturcturing some stuff to make it easier to find.
- Mark - Create table descriptions.  Even though these are rather self explanatory, users may need all the help they can get.

April 19th 2011 - 5:00 pm
=========================
We have accomplished a lot over the past couple days and had various "mini meetings" in class and by email/online voice chat. This
log will be more of a summary of those meetings as well as where we stand currently.

Unit tests now have a link to the source file to let the user know how we are testing the components, not just what we are testing.
The test plan has been rewritten to give a better idea of our testing approach.

Documentation is still in the process of being cleaned up.  More has been added to the user guide and an entire How To section has
been added to help users run the Assembler.  Pictures will be added to this soon.  We spoke with Al and showed him our current
documentation and he seems to be pretty content with it.  He pointed out a few issues we have in the language specifications and
we plan on going through that and updating it so that all of the information is valid (as far as we know).

Directives will be complete shortly.  Mark is currently working on ADC and ADCE which are the last directives to be implemented.

Our focus for the rest of the day will be going through the documentation and ensuring everything has been properly documented and
running test programs to look for errors that may not have been caught yet by the assembler. Unless any serious issues pop up
during this time, we feel like we should be finished tonight.

To do
-----
- Everyone - Look through the documentation for possible issues and try to find errors that aren't being caught.

April 16th 2011 - 2:00 pm
=========================
Documentation for language specification has been completed. We will still need to go back through it after we have finished
more of the parser to ensure the documentation is consistent with implementation. Presentation for test cases has been improved.
Unit tests are now seperated into tables making them much easier to read and understand.  We still need to link each test to
the actual testing code to show how the test was performed.  Test programs have been seperated to their own pages and display
the output given after being run.  This output is recreated each time a change has been made to the program so the output on
the site at any time should be completely up to date.

We have a good foundation for error catching at this point.  A new Errors class has been created which will store all of our
errors.  When an error is caught while parsing, we can add the error to that line and messages will be displayed in the
intermediate file. This will be our focus the next couple days.  We need to figure out where errors can occur and ensure they
are being caught.  We must also create a way to end parsing if a fatal error is found.

On Monday (or Tuesday at the latest) we wish to meet with a grader to share our progress and get advice on how to improve it.

To do
-----
- Everyone - Add error catching in parsing.
- Jacob - Cleanup documentation and update DEDs.
- Mark - Finish parsing directives.
- Andrew - Rewrite testing plan description. Give the user a better idea of how we have been testing our program.

April 14th 2011 - 4:00 pm
=========================
After some discussion about the use of BinaryHelper, it was decided that it would be easier to make it also
handle values that would be the same whether they were in two's complement or not.  Andrew added these changes.
It was also decided to add a function to help out the user. IsInRange was added so the user can determine if
they are providing valid input before calling ConvertNumber.

Since last meeting a lot of work has been done on the documentation.  We have a little clearer idea of what is
to be required. So far the major additions have been a user guide and a language specification for FFA (which can
probably be considered as part of the user guide).  Jacob and Andrew took some time to write brief descriptions and
examples of CNTL and STACK instructions and most of the directives.  A couple directives still need to be
documented but we want some further clarification on their purpose before doing so.  JUMP, SOPER, MOPER and Literals
all still need documentation.

The decision was made to also split Parser up.  While it is possible to keep Parser as one class, we decided that it
would be better for organization if we had seperate classes to deal with different aspects of parsing.

To do
-----
- Jacob - Finish up documentation for language specification besides items that require further clarification from instructor.
- Andrew - Work on testing plan and documentation.  Find a way to present our ideas on the subject and how we accomplished it in a better format.
- Mark - Work on a method of error catching and reporting.

April 13th 2011 - 8:00 pm
=========================
We met up to discuss how to process directives for SP1, and how to properly handle errors. At the moment, errors aren't really handled at all, and what directive code that does exist is "patched in" in places where it probably shouldn't be.

Looking at the results from SP1, we developed a much clearer plan for documentation. We're planning on writing out the machine specification for the users' guide (which may also be relevant for the developer's guide). We learned of what was missing from our testing documents and what to improve, as well.

To do
-----
- Jacob - Fix up small documentation issues, and begin write-up of language specification
- Andrew - Write up error and testing documentation
- Mark - Look into processing directives

April 10th 2011 - 3:00 pm
=========================
The group met last night and worked together to get the rest of the functionality needed for SP0 finished.
Our program can now read in a correctly formatted FFA program and parse it.  Each line is broken down
into its different parts and a report is generated for the user.  The program now also creates and sorts
a symbol table which is displayed at the end of the report.

Currently very few errors are being checked by the parser as this will be a big part of SP1.  We have
begun planning for how we want to handle these errors and created a file that contains a list of errors we
may run into with their description of how to fix and what action will be taken by the assembler.  The
current plan is to have these messages displayed to the user if the error occurs.

Document generation is functional and complete for the purposes of this lab.  We may tweak it later for
purposes of looks but we are happy with where it is at now.  Since Doxygen is now has our custom tags we
can document the other items that are required such as original author, modification log, etc.  We plan on
going through and ensuring all of our procedures have all of these tags.

To do
-----
- Everyone
 
  - Ensure all of the procedures have been properly documented and make changes when needed.
  - Finish creating test fixtures for individual procedures.

April 9th 2011 - 9:45 pm
========================
We added tests for the Directives, Instructions, SymbolTable, and Tokenizer classes. The Parser
correctly parses the valid source file provided by Al. A documentation framework was created to
take advantage of Doxygen's capabilities and to make some work more automatic. We're at a point
where the assembler works for SP0, but it needs lots of polishing.

We're planning on finishing the lab by tomorrow to submit early.

To do
-----
- Jacob - Finish documentation generation
- Everyone - Add DED entries, full source comments, and unit tests

April 8th 2011 - 6:00 pm
========================
Met to discuss further specifics for pass 1 and work on getting some of our ideas coded.  Tokenizer 
was updated to use enumerated types for token kind rather than strings.  Also added the ability to
create intermediate lines which will be used to later create our intermediate version.

We installed and tweaked NUnit and it appears it will work very similar to JUnit tests which will
work well for this project.  We can now begin to code the tests we had planned and make sure that
our current implementations are functioning correctly.

Mark add Directives.cs.

Issues
------
An issue came up involving going from pass 1 to pass 2 and value conversions.  When going from pass
1 to pass 2 we were told that literals should be converted to hex.  However during pass two this must
be then converted back to binary and then to hex again (because the literal value can be 10 digits long
in which case there would be overlap and we couldn't simply append the hex value on to the end of the
translated instruction).  It seems like it would be more efficient to just pass this value as a binary
value from pass 1 to pass 2.  We plan on bringing this up with an instructor.

April 4th 2011 - 4:30 pm
========================
The team has decided to use Doxygen for documentation purposes. Doxygen can create html
documentation based on in code documentation.  Some small modifications may need to be made
to suit this project.  Discussed this with the instructor and he suggested speaking with
Brianna as she has some experience with Doxygen.

We are currently looking into using NUnit for testing.  None of the group members have any
experience with NUnit so until we start actually implementing it, we won't be sure it will
suit our needs.  Testing will be done with unit tests as methods and classes are created and
then using test programs once we have funtioning code.

We discussed if a Tokenizer was needed and decided that while it is possible to implement
without a tokenizer, having one will create cleaner and better structured code.  We also
discussed how we will store operation/directive names and their opcodes.  We plan on following
the instructors advice by storing them in a text file to be read into the program.  From there
we may use a Map to hold the operation, function and opcode.

Previous assignments were met.

To do
-----
- Andrew - Write the tokenizer.
- Mark - Begin working on the format of intermediate version/Lab 0 output, either by design or coding.

March 30th 2011 - 7:30 pm
=========================
All members have installed git and are able to connect to the directory on the cse server.
Mark added the ability to log so our program will create a log file when run.  This will be
helpful both when developing and for users.

Group roles were assigned and are as follows:

* Project/Design Leader: Mark
* Documentation Leader:  Jacob
* Testing Leader:        Andrew

Previous assignments were met.

To do
-----
- Jacob - Since we are not using Java, we are unable to use Javadoc and must find a new way to document. Jacob is going to look into methods of documenting and present our options so we can decide what will work the best.
- Andrew - We also need a structured way to test our implemention without JUnit. Andrew will look into testing options.

March 28th 2011 - 7:00 pm
=========================

Group members
-------------
* Andrew Buelow
* Mark Mathis
* Jacob Peddicord

We have decided to use C# to code our project.

To do
-----
- Jacob - Set up git so we have a common project directory.

