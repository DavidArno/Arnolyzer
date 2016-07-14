## Arnolyzer ##

#### A clean-code, Roslyn-based, analyzer for C# 6. ####


----------
### Introduction ###
Arnolyzer is a Roslyn-based C# code analyzer that aims to provide a set of rules that encourage modern, functional-orientated, coding standards in C#. Pure functions; no inheritance; no global state; adherence to SOLID principles; immutable variables; and short, concise sections of code.


### Analyzer Categories ###
The Arnolyzer analyzers are grouped by category. For details on each category, the thinking behind why following the rules of each category is a good idea, and the analyzers that enforce these rules, see:

%CATEGORY-LIST%

### Analyzers ###
#### What's implemented ####
Thus far, the analyzers implemented are:

%IMPLEMENTED-LIST%
For details of each of these, please follow the respective links.

#### What's planned ####
The following analyzers are planned for future releases of Arnolyzer, but haven't yet been implemented:

%PLANNED-LIST%
In addition to this list of planned analyzers, there are two key areas of further work planned:

1. There are situations where the code needs to violate some of these rules and so a means to suppress the rule is needed. This will be achieved via attributes. So far, very few rules take advantage of this.
2. The are situations where simple code fixes can be offered to fix violations. This area of work hasn't been started at all, yet.


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