How to run the Assembler
------------------------

1) Moving the files
```````````````````

If you are running this program from a CD your files should already be unzipped.  For
convenience you may want to move your files off of the CD and on to your computer. Insert
your CD and open it so you are able to view all files on the CD. You should see two items:

* A folder named c560aa05.sp1.ontime - This contains all of the content for our project including source files and documentation.
* A README.txt - This README will give you a better understanding of the contents of the project. It is no neccessary to correctly run the Assembler but may still prove to be useful.

Move the folder off the CD either by clicking on it and dragging or copying then
pasting it somewhere on your computer.  It shouldn't matter where you place the folder, just
as long as it can be found easily.

If you have the zip file already on your computer, you will first need to unzip it using your
computer's default unzipper or another unzipping application.  Once the files are unzipped feel
free to move the folder to a location that is more easily accessible.

2) Ensuring it will run
```````````````````````

In order to run the Asembler it is required that the Microsoft .NET Framework 4 or Mono is installed on your machine
depending on the operating system you use.

* In Windows : If you do not already have the Microsoft .NET Framework or if it isn't up to date you will need to download and install the newest version.  Microsoft .NET Framework 4 and be found `on microsoft's website <http://www.microsoft.com/downloads/en/details.aspx?FamilyID=9cfb2d51-5ff4-4491-b0e5-b386f32c0992&displaylang=en#QuickDetails>`_.
* In Mac : The .NET Framework will not work in Mac so you will need Mono which is an open source implementation for compatibility with Microsoft.NET. You can download and install mono `from here <http://www.mono-project.com/Main_Page>`_.
* In Linux : Linux users should be fine as mono is usually included with most distributions of Linux.

3) Creating a source file
`````````````````````````

You will need a program written in FFA to be compiled by this application. The language
specification is given `here <language_spec.html>`_.  We also have a few example programs in our `testing plan <test_plan.html#sample-test-programs>`_. Your program
should be written in an ASCII text file.

Inside the c560aa05.sp1.ontime directory you will find two other directories (Assembler and
Programs) as well as the executable for Assembler. Either move or copy your FFA program into
the Programs folder.

4) Opening the Terminal
```````````````````````

The Assembler is going to be run from a command line.  You will need to open a command
prompt.

* In Windows : Open the start menu and type cmd in the search bar. Press enter or select cmd.exe from the results.
* In Unix : Open the terminal from your launcher or by going to Applications > Accessories > Terminal.
* In Mac: Launch Applications or Finder then locate Terminal.

Now navigate to the c560aa05.sp1.ontime folder located on your computer through the terminal.
By typing "cd <dir>" without the quotes and by replacing <dir> with the directory you want,
you can move into a new directory.  For example, if you are currently in /MyHome/ and you
want to move to /MyHome/Programs/c560aa05.sp1.ontime you can type::

	cd Programs/c560aa05.sp1.ontime/

and hit enter.  To move back a directory you can type "cd .." without the quotes. This will
move you to the directory directly above you.

You can also display all of the contents in your current directory to help you navigate.

* Windows: Use the command "dir".
* Unix/Mac: Use the command "ls".

Once you are in the c560aa05.sp1.ontime folder you can run the Assembler.

5) Running the Assembler
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
