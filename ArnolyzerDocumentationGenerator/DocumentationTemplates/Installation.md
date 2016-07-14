---
title: Arnolyzer - Installation
layout: default
---
### Installing the Arnolyzer Analyzers ###
The Arnolyzer Analyzers (hereafter, just Arnolyzer) is available as a [nuget package](https://www.nuget.org/packages/Arnolyzer/). It isn't available as a VSIX package. The consequence of this is that it must be installed for each solution individually. The upside to the nuget approach is that the rule-exception attributes are made available via a standard nuget library reference. (see what is installed for more details).

The following instructions are for installing Arnolyzer into a solution for Visual Studio 2015 only as previous versions do not support Roslyn and thus cannot run Roslyn-based analyzers.

To install it, either run `Install-Package Arnolyzer` within the [Package Manager Console](https://docs.nuget.org/consume/package-manager-console) in Visual Studio, or select "`Manage NuGet Packages for solution...`" from the solution's context menu and add it to the desired projects.

### What is installed ###
The nuget package adds three dll's to the `References -> Analyzers` section of each project selected when the nuget package is installed. They are:
* `ArnolyzerAnalyzers.dll` - This is the analyzers assembly itself and the list of analyzers supported by the version installed show up below this.
* `SuccincT.dll` and `YamlDotNet.dll` - These two assemblies are used by `ArnolyzerAnalyzers` but do not contain analyzers themselves. Due to the way Visual Studio exposes other assemblies to an analyzer one, they must be installed alongside the latter assembly and show up accordingly. They can effectively be ignored, but are documented here for anyone curious as to why they are there.

The `ArnolyzerAnalyzers.dll` assembly is also exposed to each project as a normal reference. This allows the rule-exception attributes (in the `Arnolyzer.RuleExceptionAttributes` namespace) to be used by those projects to suppress Arnolyzer yyy's when necessary.

### Changing the severity/disabling analyzers you don't like ###

### Disabling the checking of auto-generated code and other files ###