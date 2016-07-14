%HEADER%# Arnolyzer Analyzers #

## Introduction ##
Arnolyzer is a Roslyn-based C# code analyzer that aims to provide a set of rules that encourage modern, functional-orientated, coding standards in C#. Pure functions; no inheritance; no global state; adherence to SOLID principles; immutable variables; and short, concise sections of code.

%RELEASE-AND-DEVELOPMENT%

## Installation ##
For detailed instructions on how to install and configure the Arnolyzer analyzers, please refer to the [installation guide](http://davidarno.github.io/Arnolyzer/Installation.html).

## Contributing ##
For details on how to contribute to the Analyzer Analyzers project, please refer to [Contributing to this project](https://github.com/DavidArno/Arnolyzer/wiki/Contributing.md).

## Analyzer Categories ##
The Arnolyzer analyzers are grouped by category. For details on each category, the thinking behind why following the rules of each category is a good idea, and the analyzers that enforce these rules, see:

%CATEGORY-LIST%

## Analyzers ##

### What's implemented ###
Thus far, the analyzers implemented are:

%IMPLEMENTED-LIST%
For details of each of these, please follow the respective links.

### What's planned ###
The following analyzers are planned for future releases of Arnolyzer, but haven't yet been implemented:

%PLANNED-LIST%
In addition to this list of planned analyzers, there are two key areas of further work planned:

1. There are situations where the code needs to violate some of these rules and so a means to suppress the rule is needed. This will be achieved via attributes. So far, very few rules take advantage of this.
2. The are situations where simple code fixes can be offered to fix violations. This area of work hasn't been started at all, yet.
  