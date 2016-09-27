# Arnolyzer Analyzers #

## Introduction ##
Arnolyzer is a Roslyn-based C# code analyzer that aims to provide a set of rules that encourage modern, functional-orientated, coding standards in C#. Pure functions; no inheritance; no global state; adherence to SOLID principles; immutable variables; and short, concise sections of code.

## Current Release - 1.1.0 ##
The current release of Arnolyzer is 1.1.0, which is [available as a nuget package](https://www.nuget.org/packages/Arnolyzer/). 

That release included the following:
Modification of the analyzer names to include a code for each. This then linked in with the documentation pages. 

The aim is to improve the discoverability of the analyzers for search engines, with the hope that clicking on an error in Visual Studio will take the developer to the documentation for that analyzer.

For more details of that release, previous releases, how to install Arnolyzer and documentation for the analyzers in the current release, please refer to the [Arnolyzer website](http://davidarno.github.io/Arnolyzer/).

## Current state of development ##
Currently, version 1.1.0 is in development. This is subject to change without warning though, should eg an urgent bug fix release be needed in the meantime.

Last code commit: 23 Sep 2016.

The following documentation is a summary of the Arnolyzer project (analyzers so far implemented, and what's planned), as of the last code commit. Note, this may be different to what's in the 1.1.0 release, as the code base will likely contain unreleased changes. Please refer to the [Arnolyzer website](http://davidarno.github.io/Arnolyzer/) for details of what's in the current release.


## Installation ##
For detailed instructions on how to install and configure the Arnolyzer analyzers, please refer to the [installation guide](http://davidarno.github.io/Arnolyzer/Installation.html).

## Contributing ##
For details on how to contribute to the Analyzer Analyzers project, please refer to [Contributing to this project](https://github.com/DavidArno/Arnolyzer/wiki/Contributing.md).

## Analyzer Categories ##
The Arnolyzer analyzers are grouped by category. For details on each category, the thinking behind why following the rules of each category is a good idea, and the analyzers that enforce these rules, see:

* [Encapsulation Analyzers](https://github.com/DavidArno/Arnolyzer/wiki/EncapsulationAnalyzers.md)
* [Global State Analyzers](https://github.com/DavidArno/Arnolyzer/wiki/GlobalStateAnalyzers.md)
* [Immutability Analyzers](https://github.com/DavidArno/Arnolyzer/wiki/ImmutabilityAnalyzers.md)
* [Liskov Substitution Principle Analyzers](https://github.com/DavidArno/Arnolyzer/wiki/LiskovSubstitutionPrincipleAnalyzers.md)
* [Pure-Function Analyzers](https://github.com/DavidArno/Arnolyzer/wiki/Pure-FunctionAnalyzers.md)
* [Single Responsibilty Analyzers](https://github.com/DavidArno/Arnolyzer/wiki/SingleResponsibiltyAnalyzers.md)

## Analyzers ##

### What's implemented ###
Thus far, the analyzers implemented are:

**Pure-Function Analyzers**
* [AA1000 - Static Methods Should Not Be Void](https://github.com/DavidArno/Arnolyzer/wiki/AA1000StaticMethodsShouldNotBeVoid.md)
* [AA1001 - Static Methods Should Have At Least One Parameter](https://github.com/DavidArno/Arnolyzer/wiki/AA1001StaticMethodsShouldHaveAtLeastOneParameter.md)

**Immutability Analyzers**
* [AA1301 - Variables Should Be Assigned Once Only](https://github.com/DavidArno/Arnolyzer/wiki/AA1301VariablesShouldBeAssignedOnceOnly.md)

**Global State Analyzers**
* [AA1200 - Avoid Using Static Fields](https://github.com/DavidArno/Arnolyzer/wiki/AA1200AvoidUsingStaticFields.md)
* [AA1201 - Avoid Using Static Properties](https://github.com/DavidArno/Arnolyzer/wiki/AA1201AvoidUsingStaticProperties.md)

**Encapsulation Analyzers**
* [AA1100 - Interface Properties Should Be Read-Only](https://github.com/DavidArno/Arnolyzer/wiki/AA1100InterfacePropertiesShouldBeReadOnly.md)
* [AA1101 - Class Properties Should Be Publicly Read-Only](https://github.com/DavidArno/Arnolyzer/wiki/AA1101ClassPropertiesShouldBePubliclyReadOnly.md)
* [AA1102 - Inner Types Must Be Private](https://github.com/DavidArno/Arnolyzer/wiki/AA1102InnerTypesMustBePrivate.md)

**Liskov Substitution Principle Analyzers**
* [AA2000 - Do Not Use Not Implemented Exception](https://github.com/DavidArno/Arnolyzer/wiki/AA2000DoNotUseNotImplementedException.md)
* [AA2001 - Do Not Use Not Supported Exception](https://github.com/DavidArno/Arnolyzer/wiki/AA2001DoNotUseNotSupportedException.md)

**Single Responsibilty Analyzers**
* [AA2100 - Method Parameters Must Not Be Ref Or Out](https://github.com/DavidArno/Arnolyzer/wiki/AA2100MethodParametersMustNotBeRefOrOut.md)
* [AA2103 - Method Should Not Contain And](https://github.com/DavidArno/Arnolyzer/wiki/AA2103MethodShouldNotContainAnd.md)
* [AA2104 - File Must Only Contain One Type Definition](https://github.com/DavidArno/Arnolyzer/wiki/AA2104FileMustOnlyContainOneTypeDefinition.md)


For details of each of these, please follow the respective links.

### What's planned ###
The following analyzers are planned for future releases of Arnolyzer, but haven't yet been implemented:

**Pure-Function Analyzers**
* [AA1002 - Static Methods Should Not Access State](https://github.com/DavidArno/Arnolyzer/wiki/AA1002StaticMethodsShouldNotAccessState.md)
* [AA1003 - Static Methods Should Not Create State](https://github.com/DavidArno/Arnolyzer/wiki/AA1003StaticMethodsShouldNotCreateState.md)

**Immutability Analyzers**
* [AA1300 - Parameters Should Not Be Modified](https://github.com/DavidArno/Arnolyzer/wiki/AA1300ParametersShouldNotBeModified.md)

**Single Responsibilty Analyzers**
* [AA2101 - Method Too Long](https://github.com/DavidArno/Arnolyzer/wiki/AA2101MethodTooLong.md)
* [AA2102 - File Too Long](https://github.com/DavidArno/Arnolyzer/wiki/AA2102FileTooLong.md)


In addition to this list of planned analyzers, there are two key areas of further work planned:

1. There are situations where the code needs to violate some of these rules and so a means to suppress the rule is needed. This will be achieved via attributes. So far, very few rules take advantage of this.
2. The are situations where simple code fixes can be offered to fix violations. This area of work hasn't been started at all, yet.
  