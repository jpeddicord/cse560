===========
Limitations
===========

This linker does not have any known limitations beyond limits required by the software being used to run it.

There are no limitations to the names of input files. The output file will have the name of the first given input file with the extension replaced with ``.ffa``.

The .NET 4 framework has the following requirements[*]_:

* Supported Operating Systems: Windows 7; Windows 7 Service Pack 1; Windows Server 2003 Service Pack 2; Windows Server 2008; Windows Server 2008 R2; Windows Server 2008 R2 SP1; Windows Vista Service Pack 1; Windows XP Service Pack 3
* Supported Architectures: x86; x64; ia64 (some features are not supported on ia64 for example, WPF)
* Hardware Requirements: Recommended Minimum: Pentium 1Ghz or higher with 512 MB RAM or more; Minimum disk space: x86 - 850MB, x64 - 2GB
* Prerequisites: `Windows Installer 3.1 <http://www.microsoft.com/downloads/details.aspx?familyid=889482fc-5f56-4a38-b838-de776fd4138c&displaylang=en>`_ or later; `Internet Explorer 5.01 <http://www.microsoft.com/windows/downloads/ie/getitnow.mspx>`_ or later

.. [*] Up to date requirements can be found on the `Microsoft Website for .NET 4 <http://www.microsoft.com/downloads/en/details.aspx?FamilyID=9cfb2d51-5ff4-4491-b0e5-b386f32c0992&displaylang=en#SystemRequirements>`_.

Mono has the following requirements[*]_:

* Supported Operating Systems: Windows 2000, XP, 2003 and Vista; Mac OS X Leopard (10.5), Snow Leopard (10.6). Tiger (10.4) is no longer supported; Linux: Many distributions, see `website <http://mono-project.com/Main_Page>`_ for more information.
* libc6 - C library
* glib2

.. [*] Up to date requirements can be found on the `site for MonoDevelop <http://mono-project.com/Main_Page>`_.


Programs assembled, source input files, and programs linked are only limited by the capabilities of the host operating system. However, keep in mind requirements in the target machine specification and language, such as the limit of 1024 words per program (after linking). These limits can be found on the `machine description page <../machine_description.html>`_.

