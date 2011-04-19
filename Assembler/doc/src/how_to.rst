1) If you are running this program from a CD your files should already be unzipped.  For
convenience you may want to move your files off of the CD and on to your computer. Insert
your CD and open it so you are able to view all files on the CD. You should see two items, a
folder named c560aa05.sp1.early and a README.txt.  The readme will give you a better idea of
the files in our project but is not neccessary to correctly run the Assembler. Move the folder
off the CD either by clicking on it and dragging or right clicking and selecing copy then
pasting it somewhere on your computer.  It shouldn't matter where you place the folder, just
as long as it can be found easily. If you have the zip file already on your computer, you will
first need to unzip it using your computer's default unzipper or another unzipping
application.  Once the files are unzipped feel free to move the folder to a location that is
more easily accessible.

2) You will need a program written in FFA to be compiled by this application. The language
specification is given [here].  We also have a few example programs [here]. Your program
should be written in an ASCII text file.

Inside the c560aa05.sp1.early directory you will find two other directories (Assembler and
Programs) as well as the executable for Assembler. Either move or copy your FFA program into
the Programs folder.

3) The Assembler is going to be run from a command line.  You will need to open a command
prompt.

- Windows : Open the start menu and type cmd in the search bar. Press enter or select cmd.exe
from the results.
- Unix : Open the terminal from your launcher or by going to Applications > Accessories >
Terminal.
- Mac: Launch Applications or Finder then locate Terminal.

Now navigate to the c560aa05.sp1.early folder located on your computer through the terminal.
By typing "cd <dir>" without the quotes and by replacing <dir> with the directory you want,
you can move into a new directory.  For example, if you are currently in /MyHome/ and you
want to move to /MyHome/Programs/c560aa05.sp1.early you can type "cd Programs/c560aa05.sp1.early/"
and hit enter.  To move back a directory you can type "cd .." without the quotes. This will
move you to the directory directly above you.

You can also display all of the contents in your current directory to help you navigate.

-Windows: Use the command "dir".
-Unix/Mac: Use the command "ls".

Once you are in the c560aa05.sp1.early folder you can run the Assembler.

4) Run the Assembler using the command "Assembler.exe Programs/<inputfile>" where input file is
the name of your program that you wish to compile. At this point, the assembler will complete
pass 1 and produced a formatted output of your program including bytecode for the parts it knows
and lists of errors that appear in your program. You can store this output in a file by adding
"> <outputfile>" to the command above.  Here is an example of how you may run this program: 
"Assembler.exe Programs/PRGM1.txt > PRGM1output.txt"