# YoungestFile
Recursively look for the most recent file(s) in a folder stucture. I found this useful when I remember downloading or saving a file a few days prior, but can't remember where exactly I saved it.

# Build
Compiled & tested in x86-64 Windows 10. In Visual Studio, the program cam be compiled and "Published" to a single file.

# PATH variable
You can add the directory of the compiled binary to your PATH variable to make the usage work as shown below.

# Usage:
```batch
        yf.exe -help
                Displayes this help message.
        yf.exe [DIRECTORY NAME] [MAX NUMBER OF FILES TO LIST]
               [DIRECTORY NAME] is the directory name to search. if ommitted, the current working directory is used.
               [MAX NUMBER OF FILES TO LIST] limits the output in the console window to the specified number of files. If ommitted, all of them are displayed.
```

Example:
```batch
C:\temp>yf 7
Most recently modified files (recursively searched) in "C:\temp":
FILE NAME                      DATE/TIME MODIFIED       PATH TO FILE (Relative to "C:\temp\")
--------------------------------------------------------------------------------------------------------------------
Photo_taken_today.png          4/07/2024 10:22 am       .
Test.bmp                       4/07/2024 10:22 am       temp_subfolder
Service Report July 2024.pdf   3/07/2024 4:23 pm        .
Stored data.xlsx               14/06/2024 12:34 am      .
Screenshot 1.png               11/06/2024 12:07 am      .
Links.txt                      1/05/2024 1:45 pm        .
Project_Archive.zip            9/03/2024 1:24 am        temp_subfolder\sub_sub_folder

C:\temp>
```

```batch
C:\temp>yf .\temp_subfolder
Most recently modified files (recursively searched) in ".\temp_subfolder":
FILE NAME             DATE/TIME MODIFIED        PATH TO FILE (Relative to ".\temp_subfolder\")
-----------------------------------------------------------------------------------------------------------
Test.bmp              4/07/2024 10:22 am        .
Project_Archive.zip   9/03/2024 1:24 am sub_sub_folder

C:\temp>
```