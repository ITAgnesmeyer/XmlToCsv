# XmlToCsv #

Description:
----------

XmlToCsv is a tool to convert XML-Files to CSV files.
The CSV-files are seperated by ";".


Parameters
----------
    
	-i, --input     Required. Input XML file to read.
	-o, --output    Required. Output Folder for destination Files
	-j, --json      Input Json File to read
  	--help          Display this help screen.
	--version       Display version information.

in the rootfolder of the project, you can find a batchfile to create a merged [(ILMerge by Microsoft)](https://github.com/Microsoft/ILMerge "ILMerge") version of the relese executable.

simly call:

	<repository>\XmlToCsv\>xmltocsv

The merged file will be written to the "merged" folder in the "bin" Directory

Examples:
----------

##### Sample 1 #####

	xmltocsv -i test.xml -o c:\temp

The file "test.xml" will be converted to CSF-files in the folder "C:\temp"

##### Sample 2 #####
If the parameter -j/--json is given then the JSON-file will be convierted to the given -i file.

	xmltocsv -i test.xml -o c:\temp -j some.json

In this case the file "some.json" will be converted to "test.xml" and these file will be converted to CSV-Files in the folder "C:\temp".

