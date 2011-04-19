How to run the Assembler
------------------------

1) Moving the files
```````````````````

If you are running this program from a CD your files should already be unzipped.  For
convenience you may want to move your files off of the CD and on to your computer. Insert
your CD and open it so you are able to view all files on the CD. You should see two items:

* A folder named c560aa05.sp1.early - This contains all of the content for our project including source files and documentation.
* A README.txt - This README will give you a better understanding of the contents of the project. It is no neccessary to correctly run the Assembler but may still prove to be useful.

Move the folder off the CD either by clicking on it and dragging or copying then
pasting it somewhere on your computer.  It shouldn't matter where you place the folder, just
as long as it can be found easily.

If you have the zip file already on your computer, you will first need to unzip it using your
computer's default unzipper or another unzipping application.  Once the files are unzipped feel
free to move the folder to a location that is more easily accessible.

2) Creating a source file
`````````````````````````

You will need a program written in FFA to be compiled by this application. The language
specification is given `here <language_spec.html>`_.  We also have a few example programs in our `testing plan <test_plan.html#sample-test-programs>`_. Your program
should be written in an ASCII text file.

Inside the c560aa05.sp1.early directory you will find two other directories (Assembler and
Programs) as well as the executable for Assembler. Either move or copy your FFA program into
the Programs folder.

3) Opening the Terminal
```````````````````````

The Assembler is going to be run from a command line.  You will need to open a command
prompt.

* In Windows : Open the start menu and type cmd in the search bar. Press enter or select cmd.exe from the results.
* In Unix : Open the terminal from your launcher or by going to Applications > Accessories > Terminal.
* In Mac: Launch Applications or Finder then locate Terminal.

Now navigate to the c560aa05.sp1.early folder located on your computer through the terminal.
By typing "cd <dir>" without the quotes and by replacing <dir> with the directory you want,
you can move into a new directory.  For example, if you are currently in /MyHome/ and you
want to move to /MyHome/Programs/c560aa05.sp1.early you can type::

	cd Programs/c560aa05.sp1.early/

and hit enter.  To move back a directory you can type "cd .." without the quotes. This will
move you to the directory directly above you.

You can also display all of the contents in your current directory to help you navigate.

* Windows: Use the command "dir".
* Unix/Mac: Use the command "ls".

Once you are in the c560aa05.sp1.early folder you can run the Assembler.

4) Running the Assembler
````````````````````````

Run the Assembler using the command::

	Assembler.exe Programs/<inputfile>

where input file is the name of your program that you wish to compile.

At this point, the assembler will complete pass 1 and produce a formatted output of your
program including bytecode for the parts it knows and lists of errors that appear in your program.
Please see the `User's Guide <user_guide.html>`_ for more information on this output.

You can store this output in a file by adding "> <outputfile>" to the command above.

Some examples of how you may run this program::
 
	Assembler.exe Programs/PRGM1.txt
	
	Assembler.exe Programs/PRGM2.txt > PRGM2output.txt
