---
title: Arnolyzer - Home
layout: default
---
# Arnolyzer Analyzers #

## Introduction ##
Arnolyzer is a Roslyn-based C# code analyzer that aims to provide a set of rules that encourage modern, functional-orientated, coding standards in C#. Pure functions; no inheritance; no global state; adherence to SOLID principles; immutable variables; and short, concise sections of code.

## Current Release - 1.1.0 ##
The current release of Arnolyzer is 1.1.0, which is [available as a nuget package](https://www.nuget.org/packages/Arnolyzer/). 

This release includes the following:
Modification of the analyzer names to include a code for each. This then linked in with the documentation pages. 

The aim is to improve the discoverability of the analyzers for search engines, with the hope that clicking on an error in Visual Studio will take the developer to the documentation for that analyzer.

### Previous Releases ###
Details of previous releases can be viewed at [Previous releases](PreviousReleases.html).


## Installation ##
For detailed instructions on how to install and configure the Arnolyzer analyzers, please refer to the [installation guide](http://davidarno.github.io/Arnolyzer/Installation.html).

## Contributing ##
For details on how to contribute to the Analyzer Analyzers project, please refer to [Contributing to this project](https://github.com/DavidArno/Arnolyzer/wiki/Contributing.md).

## Analyzer Categories ##
The Arnolyzer analyzers are grouped by category. For details on each category, the thinking behind why following the rules of each category is a good idea, and the analyzers that enforce these rules, see:

* [Encapsulation Analyzers](EncapsulationAnalyzers.html)
* [Global State Analyzers](GlobalStateAnalyzers.html)
* [Immutability Analyzers](ImmutabilityAnalyzers.html)
* [Liskov Substitution Principle Analyzers](LiskovSubstitutionPrincipleAnalyzers.html)
* [Pure-Function Analyzers](Pure-FunctionAnalyzers.html)
* [Single Responsibilty Analyzers](SingleResponsibiltyAnalyzers.html)

## Analyzers ##

### What's implemented ###
Thus far, the analyzers implemented are:

**Pure-Function Analyzers**
* [AA1000 - Static Methods Should Not Be Void](AA1000StaticMethodsShouldNotBeVoid.html)
* [AA1001 - Static Methods Should Have At Least One Parameter](AA1001StaticMethodsShouldHaveAtLeastOneParameter.html)

**Encapsulation Analyzers**
* [AA1100 - Interface Properties Should Be Read-Only](AA1100InterfacePropertiesShouldBeReadOnly.html)
* [AA1101 - Class Properties Should Be Publicly Read-Only](AA1101ClassPropertiesShouldBePubliclyReadOnly.html)
* [AA1102 - Inner Types Must Be Private](AA1102InnerTypesMustBePrivate.html)

**Liskov Substitution Principle Analyzers**
* [AA2000 - Do Not Use Not Implemented Exception](AA2000DoNotUseNotImplementedException.html)
* [AA2001 - Do Not Use Not Supported Exception](AA2001DoNotUseNotSupportedException.html)

**Single Responsibilty Analyzers**
* [AA2100 - Method Parameters Must Not Be Ref Or Out](AA2100MethodParametersMustNotBeRefOrOut.html)
* [AA2103 - Method Should Not Contain And](AA2103MethodShouldNotContainAnd.html)
* [AA2104 - File Must Only Contain One Type Definition](AA2104FileMustOnlyContainOneTypeDefinition.html)


For details of each of these, please follow the respective links.

### What's planned ###
The following analyzers are planned for future releases of Arnolyzer, but haven't yet been implemented:

**Pure-Function Analyzers**
* [AA1002 - Static Methods Should Not Access State](AA1002StaticMethodsShouldNotAccessState.html)
* [AA1003 - Static Methods Should Not Create State](AA1003StaticMethodsShouldNotCreateState.html)

**Immutability Analyzers**
* [AA1300 - Parameters Should Not Be Modified](AA1300ParametersShouldNotBeModified.html)
* [AA1301 - Variables Should Be Assigned Once Only](AA1301VariablesShouldBeAssignedOnceOnly.html)

**Global State Analyzers**
* [AA1200 - Avoid Using Static Fields](AA1200AvoidUsingStaticFields.html)
* [AA1201 - Avoid Using Static Properties](AA1201AvoidUsingStaticProperties.html)

**Single Responsibilty Analyzers**
* [AA2101 - Method Too Long](AA2101MethodTooLong.html)
* [AA2102 - File Too Long](AA2102FileTooLong.html)


In addition to this list of planned analyzers, there are two key areas of further work planned:

1. There are situations where the code needs to violate some of these rules and so a means to suppress the rule is needed. This will be achieved via attributes. So far, very few rules take advantage of this.
2. The are situations where simple code fixes can be offered to fix violations. This area of work hasn't been started at all, yet.
  