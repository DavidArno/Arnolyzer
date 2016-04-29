## Arnolyzer ##

#### A clean-code, Roslyn-based, analyzer for C# 6. ####

Last updated: 29 Apr 2016.

----------
### Introduction ###
Arnolyzer is a Roslyn-based C# code analyzer that aims to provide a set of rules that encourage modern, functional-orientated, coding standards in C#. Pure functions; no inheritance; no global state; adherence to SOLID principles; immutable variables; and short, concise sections of code.

### Current Release - 1.1.0 ###
The current release of Succinc\<T\> is 1.1.0, which is [available as a nuget package](https://www.nuget.org/packages/Arnolyzer/). 

This release includes the following:
Modification of the analyzer names to include a code for each. This then linked in with the documentation pages. 

The aim is to improve the discoverability of the analyzers for search engines, with the hope that clicking on an error in Visual Studio will take the developer to the documentation for that analyzer.

### Previous Releases ###
Details of previous releases can be viewed at [Previous releases](https://github.com/DavidArno/Arnolyzer/wiki/PreviousReleases.md).

### Installation ###
For detailed instructions on how to install and configure the Arnolyzer analyzers, please refer to the [installation guide](**************).

### Analyzers ###
#### What's implemented ####
Thus far, the analyzers implemented are:

**Pure-Function Analyzers**
[AA1000 - Static Methods Should Not Be Void](https://github.com/DavidArno/Arnolyzer/wiki/AA1000StaticMethodsShouldNotBeVoid.md)
[AA1001 - Static Methods Should Have At Least One Parameter](https://github.com/DavidArno/Arnolyzer/wiki/AA1001StaticMethodsShouldHaveAtLeastOneParameter.md)

**Encapsulation Analyzers**
[AA1100 - Interface Properties Should Be Read-Only](https://github.com/DavidArno/Arnolyzer/wiki/AA1100InterfacePropertiesShouldBeReadOnly.md)
[AA1101 - Class Properties Should Be Publicly Read-Only](https://github.com/DavidArno/Arnolyzer/wiki/AA1101ClassPropertiesShouldBePubliclyReadOnly.md)
[AA1102 - Inner Types Must Be Private](https://github.com/DavidArno/Arnolyzer/wiki/AA1102InnerTypesMustBePrivate.md)

**Liskov Substitution Principle Analyzers**
[AA2000 - Do Not Use Not Implemented Exception](https://github.com/DavidArno/Arnolyzer/wiki/AA2000DoNotUseNotImplementedException.md)
[AA2001 - Do Not Use Not Supported Exception](https://github.com/DavidArno/Arnolyzer/wiki/AA2001DoNotUseNotSupportedException.md)

**Single Responsibilty Analyzers**
[AA2100 - Method Parameters Must Not Be Ref Or Out](https://github.com/DavidArno/Arnolyzer/wiki/AA2100MethodParametersMustNotBeRefOrOut.md)
[AA2103 - Method Should Not Contain And](https://github.com/DavidArno/Arnolyzer/wiki/AA2103MethodShouldNotContainAnd.md)
[AA2104 - File Must Only Contain One Type Definition](https://github.com/DavidArno/Arnolyzer/wiki/AA2104FileMustOnlyContainOneTypeDefinition.md)


For details of each of these, please follow the respective links.

#### What's planned ####
The following analyzers are planned for future releases of Arnolyzer, but haven't yet been implemented:

**Pure-Function Analyzers**
[AA1002 - Static Methods Should Not Access State](https://github.com/DavidArno/Arnolyzer/wiki/AA1002StaticMethodsShouldNotAccessState.md)
[AA1003 - Static Methods Should Not Create State](https://github.com/DavidArno/Arnolyzer/wiki/AA1003StaticMethodsShouldNotCreateState.md)

**Immutability Analyzers**
[AA1300 - Parameters Should Not Be Modified](https://github.com/DavidArno/Arnolyzer/wiki/AA1300ParametersShouldNotBeModified.md)
[AA1301 - Variables Should Be Assigned Once Only](https://github.com/DavidArno/Arnolyzer/wiki/AA1301VariablesShouldBeAssignedOnceOnly.md)

**Global State Analyzers**
[AA1200 - Avoid Using Static Fields](https://github.com/DavidArno/Arnolyzer/wiki/AA1200AvoidUsingStaticFields.md)
[AA1201 - Avoid Using Static Properties](https://github.com/DavidArno/Arnolyzer/wiki/AA1201AvoidUsingStaticProperties.md)

**Single Responsibilty Analyzers**
[AA2101 - Method Too Long](https://github.com/DavidArno/Arnolyzer/wiki/AA2101MethodTooLong.md)
[AA2102 - File Too Long](https://github.com/DavidArno/Arnolyzer/wiki/AA2102FileTooLong.md)


In addition to this list of planned analyzers, there are two key areas of further work planned:

1. There are situations where the code needs to violate some of these rules and so a means to suppress the rule is needed. This will be achieved via attributes. So far, very few rules take advantage of this.
2. The are situations where simple code fixes can be offered to fix violations. This area of work hasn't been started at all, yet.

#### Analyzer Categories ####
The Arnolyzer analyzers are grouped by category. For details on each category, the thinking behind why following the rules of each category is a good idea, and the analyzers that enforce these rules, see:

* [Encapsulation Analyzers](https://github.com/DavidArno/Arnolyzer/wiki/EncapsulationAnalyzers.md)
* [Global State Analyzers](https://github.com/DavidArno/Arnolyzer/wiki/GlobalStateAnalyzers.md)
* [Immutability Analyzers](https://github.com/DavidArno/Arnolyzer/wiki/ImmutabilityAnalyzers.md)
* [Liskov Substitution Principle Analyzers](https://github.com/DavidArno/Arnolyzer/wiki/LiskovSubstitutionPrincipleAnalyzers.md)
* [Pure-Function Analyzers](https://github.com/DavidArno/Arnolyzer/wiki/Pure-FunctionAnalyzers.md)
* [Single Responsibilty Analyzers](https://github.com/DavidArno/Arnolyzer/wiki/SingleResponsibiltyAnalyzers.md)


#Remove this stuff >>>>>>>>>>>>

### Static higher order functions (SHOFs)
Static methods that create or access global state are well recognised as code smells: they create thread-unsafe units of code with tight coupling and they make unit testing harder than it need be. Functional languages though also have static functions, called higher order (or pure) functions. These functions are completely deterministic and are well recognised as a way of avoiding coupling, simplifying testing and for creating thread-safe code.

To distinguish between stateful static methods and methods that copy the principles of higher order functions, [Mike Hadlow](https://twitter.com/mikehadlow/status/646645950656708608) coined the term Static Higher Order Function (SHOF) to identify the latter in OO languages, such as C#.

#### AA1000 Static Methods Must Not Be Void
To count as a SHOF, a static method must have no side effects, and thus to have a purpose, it must return a value.

#### AA1001 Static Methods Must Have At Least One Parameter
To count as a SHOF, a static method must not receive data from any source other than its parameters, thus it must have at least one parameter to have a purpose.

#### AA1002 Static Methods Must Not Access State
A SHOF must only derive a result from the supplied parameter(s). It must not access any static field or property.

#### AA1003 Static Methods Must Not Create State
The only result from calling a SHOF must be the returned value, or an exception. Therefore it must not invoke any method that has a void return type, nor write to any field or property.

### Immutability and Encapsulation
A badly named variable that has many values written to it over the course of a long-winded method makes for hard-to-read code. Variables that only written to once both make for easier reading, as a new appropriately named variable must be created for each assignment, and encourage the principle of least surprise. The following rules enforce that idea:

#### AA1100 - Parameters Should Not Be Modified
Reports when a parameter is used as a mutable variable.

#### AA1101 - Variables Should Be Assigned Once
Reports when a variable is re-assigned.

#### AA1102 - Interface Properties Must Be Read-Only
Properties defined by interfaces should not have setters.

#### AA1103 - Class Properties Must Be Publicly Read-Only
Class properties should not have public setters.

#### AA1104 - Inner Types Must Be Private
Inner types should not be marked `internal` or `public` as this leads to leaky abstractions.

###No global/static state
Global (and to a lesser extent, any static) state creates a testing and maintenance nightmare as code becomes both tightly coupled and tests must be run in sequence through fear that two tests might call two pieces of code that both modify shared state resulting in brittle tests. These rules prevent global and static state:

#### AA1200 Do Not Use Static Fields
Static fields contain static scope and so should be avoided.

#### AA1201 Do Not Use Static Properties
Static properties contain static scope and so should be avoided.

###Liskov Substitution Principle
From the [Liskov Substitution Principle](https://en.wikipedia.org/wiki/Liskov_substitution_principle) article on Wikipedia:
> Substitutability is a principle in object-oriented programming. It states that, in a computer program, if S is a subtype of T, then objects of type T may be replaced with objects of type S (i.e., objects of type S may substitute objects of type T) without altering any of the desirable properties of that program (correctness, task performed, etc.)

Two exceptions defined by the .NET framework are a direct violation of this principle (LSP) and the following two rules check for such violations:

#### AA2000 Do Not Use NotImplementedException
NotImplementedException violates LSP and should not be used.

#### AA2001 Do Not Use NotSupportedException
NotSupportedException violates LSP and should not be used.

###Single responsibility principle
The [single responsibility principle](https://en.wikipedia.org/wiki/Single_responsibility_principle) article on Wikipedia states:
> the single responsibility principle states that every class should have responsibility over a single part of the functionality provided by the software, and that responsibility should be entirely encapsulated by the class. All its services should be narrowly aligned with that responsibility

The following rules extend this principle to methods and files and seek to identify sections of code that may break that principle:

#### AA2100 Method Parameters Must Not Be Ref Or Out
Reports when either `ref` or `out` are used for any parameter of any method. Both of these keywords lead to a method that returns two results via two different routes, breaking the single responsibility principle.

#### AA2101 Method Too Long
A long method is likely to break the principle by carrying out too many functions. What is too long is highly subjective, so this rule will need to be configurable to allow the maximum lines to be specified.

#### AA2102 File Too Long
A long class is likely to break the principle by carrying out too many functions. What is too long is highly subjective, so this rule will need to be configurable to allow the maximum lines to be specified.

#### AA2103 Method Should Not Contain And
Methods that contain "And" in their name often undertake two tasks and thus have two responsibilities. Thus they are likely to violate the principle.

#### AA2104 File Must Only Contain One Type Definition
A file that contains more than one class, enum or interface definition inherently contains more than one responsibility.  