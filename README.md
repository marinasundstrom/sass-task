Sass Task
==============================

A simple build task for compiling Sass files in your MSBuild project.

Easy to configure via familiar JSON-based config file.

Cross-platform alternative to the [WebCompiler](https://github.com/madskristensen/WebCompiler).

## Prerequisites

* [.NET Core SDK 2](https://www.dot.net/) or later, or .NET Framework 4.7.2 or later.
* Latest version of [Sass](https://sass-lang.com) installed on your computer.

## Usage

Just add a reference to the [SassTask](https://www.nuget.org/packages/SassTask) package in your project.

```sh
dotnet add package SassTask --version 0.1.0-*
```

The build task is then automatically imported.


### Example file

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>netcoreapp3.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="SassTask" Version="0.2.0-*" PrivateAssets="All" />
  </ItemGroup>

</Project>
```

### Configuration

The file extensions recogzined by the build task are ```.sass``` and ```.scss```. 

## Configuration file

The build task can be confgured in the ```sassconfig.json``` file.

The default settings are:

```json
{
    "compilerOptions": {
      "style": "expanded",
      "sourceMap": true,
      "sourceMapUrls": "relative",
      "embedSources": false,
      "embedSourceMap": false,
      "sourceDir": ".",
      "outDir": "."
    },
    "files": undefined
}
```

### Properties
* compilerOptions
  * style *("expanded" | "compressed")* - the style of the output.
  * sourceMap - indicating whether a source map should be created or not.
  * sourceMapUrls *("relative" | "absolute")* - the type of paths to source files.
  * embedSources - embed sources in the resulting CSS-files.
  * embedSourceMap - embed source map in the resulting CSS-files.
  * sourceDir - the directory in which the sass-files are located.
  * outDir - the directory to output css-files in.
* files - an array of file paths that will be exclusively included as input. Wildcards are accepted. 

## Build

To build this solution:

1. Install .NET Core CLI 3.0.0 or greater. See <https://dot.net/core> for download links.
2. Run `./build.ps1` in PowerShell or `./build.sh` from Bash.
