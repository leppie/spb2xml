README for spb2xml -     Release 1.2 - 21 May 2007
lc0277 
http://lc0277.nerim.net
------------------------------------------------------

0. History/Changes
Release 1.2 fixes two bugs concerning explicit SPB namespace
usage, preventing decompiled SPB files from being recompiled.
V1.2 also includes source code.

1. What are SPB/SimProp/PropDefs files ?

SPB means SimPropBinary. It is a proprietary file format
introduced in Microsoft Flight Simulator X. It is a generic
data format that store various kind of informations and can be 
compiled from different XML files.
Files in SPB are read quicker by the simulator than
XML files, so when performance is an issue files can be
compiled to SPB.

PropDefs or property definitions are XML documents that
tells how a particular kind of XML file can be compiled
to SPB.

2. What is SPB used for in FSX ?

SPB is used in place of XML in some parts of the simulator:
- Missions
- Autogen description
- Dialogs localisation
- Living World configuration
- Gauges (unused)
If you look at propdefs files (in the propdefs subdirectory
of FSX), you will see all currently supported types. 

3. What is this program ?

SPB files are compiled using simpropcompiler.exe shipped
with the FSX deluxe SDK. However no tools were providen to
do the inverse task.
This program can decompile SPB and produces the original
XML document.

4. How do I use it ?

4a. Requirements
You need the .NET 2.0 framework installed on your
computer to use this program. 
It can be downloaded from :
http://www.microsoft.com/downloads/details.aspx?FamilyID=0856eacb-4362-4b0d-8edd-aab15c5e04f5&displaylang=en

4b. Installation
No installation required. Just drop the spb2xml.exe
where you want

4c. Run
spb2xml is a command line utility, you must use it
from a terminal. Use start -> run and use "cmd" then
change to the directory where you installed spb2xml 
(cd X:\some dir\etc\)

Usage:

spb2xml [-v] [-h] [-s symboldir] [-m modellist] file.spb [output.xml]

	-v gives verbose information
	-h gives you this help
	
	-s specify the path where propdefs files are stored.
	It is automatically detected, but if spb2xml complains
	about missing symbols, you should set up manually.
	eg:
	spb2xml -s c:\FSX\propdefs c:\FSX\Missions\Tutorials\GSNovice.spb
	
	-m specify an optional list of models. Only model GUID 
	are stored in SPB files, if you want a friendly output you
	could use this option.
	Models list are in the same format as the library objects.txt
	files of Autogen SDK, ie two tokens per line, the first
	one being the model name and the second the GUID.

	X:\dir\file.spb will be decompiled to X:\dir\file.xml
	if you want a different file, you can pass it as an argument
	eg:
	spb2xml c:\FSX\Missions\Tutorials\GSNovice.spb c:\MissionsWork\Start.xml
	

5. Feedback/Bugs
If you find any bugs or invalid data produced by this program
please report to the original author at the following address:
lc0277@nerim.net
Be sure to include failing SPB files.


6. Licence/Disclaimer

This software and its source code are in the public domain. 
Permission to use, copy, modify, and distribute this program for any purpose is 
hereby granted, without any conditions or restrictions. 
This software is provided "as is" without express or implied warranty.
