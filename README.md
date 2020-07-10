# SSBHLib
A library for parsing the SSBH format for Smash Ultimate. Cross Mod provides an
example of how the library can be used. If you discover a bug in any of these projects, report it in
[issues](https://github.com/Ploaj/CrossMod/issues).

# Cross Mod
An experimental Smash Ultimate model viewer designed for shader development, testing SSBHLib, and reverse engineering Smash Ultimate's rendering. For creating model imports, see [StudioSB](https://github.com/Ploaj/StudioSB).

### System Requirements
The recommended OpenGL version for Cross Mod is 4.20. Version 3.30 or higher may still work as long as the necessary OpenGL extensions are present.

# Mat Lab
A simple program for converting .numatb files to .xml and .xml files to .numatb. The output path will be generated as `<input>_out.xml` or `<input>_out.numatb` if not specified. Files can also be dragged onto the executable to convert them.

**Usage**
`MatLab.exe <input> [output]`  

# Building
Compile in Visual Studio 2019 or later on Windows. Requires .NET Framework 4.6.1.
