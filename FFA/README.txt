==================
FFA Software Suite
==================

FOR FULL DOCUMENTATION, PLEASE SEE THE ``doc/html/index.html`` PAGE UNDERNEATH THIS FOLDER.

Platform Support
----------------

The FFA software suite currently supports all OS that can run Microsoft .NET Framework 4 or Mono.
Please see the How To online or in doc/html/index.html for more information.


Getting Started
---------------

To get started, you should copy files from this disc or archive to a local folder.
For information, please see the full Getting Started guide in doc/html/index.html or online
at http://files.codechunk.net/560-doc/.


Contents
--------
This disc contains:

* the executable for the FFA Assembler as well as all C# (.cs) files required to build it.
		Assembler.exe and Assembler/
		Linker.exe and Linker/
		Simulator.exe and Simulator/

* resource files used by the application (primarily to read in instructions, directives and
errors).
		Assembler/Resources/
		Linker/Resources/
		Simulator/Resources/

* log folder to hold log files generated during runtime.
		Assembler/bin/Debug/Log/
		Linker/bin/Debug/Log/
		Simulator/bin/Debug/Log/

* CSV files used to generate DED or online documentation.
		Assembler/, Linker/, Simulator/

* documentation including a link to the online documentation, all html files, and the
sources used to generate it.
		doc/

* C# files containing NUnit test cases and test programs used.
		Assembler/Tests/ and Assembler/Tests/Programs
		Simulator/Tests/ and Simulator/Tests/Programs

Credits
-------

Andrew Buelow 	- Testing Lead
Mark Mathis 	- Code & Design Lead
Jacob Peddicord - Documentation Lead
