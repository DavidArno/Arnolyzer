# Arnolyzer
A clean-code, Roslyn-based, analyzer for C# 6.

Readme last updated: 24th Oct 2015.

## Why does this project exist?
Let's roll back the clock a bit to life before C# 6 and the Roslyn compiler. There existed a tool called StyleCop. Its idea was a good one: automate the checking of coding standards. Its execution though was poor: it focused on the mundane (eg checking correct spacing around keywords and symbols) and the downright bad, such as insisting one clutter a source file was distracting noise. Noise like mandating the superfluous use of `this.` and the requirement to create long-winded XML-based comments for properties, resulting in nonsense like:

    /// <summary>
    /// Gets/sets the customer ID.
    /// </summary>
    /// <returns>The customer ID</returns>
    /// <value>A new customer ID</value>
    public int Id { get; set; }

What saved StyleCop from itself was an excellent extension called StyleCop+. This extension replaced many of StyleCop's weaker rules with more powerful ones and, most importantly, it introduced rules that encouraged the writing of better code, such as maximum method and file lengths.

Fast forward to the summer of 2015 and Roslyn has effectively killed off StyleCop. It's introduced a new way of writing analyzers using the compiler. A number of analyzer projects exist, but the best known one sadly just replicates the rules of StyleCop. Yet Roslyn offers the opportunity to support such so much more, and thus this project was created.

Its aims are simple: provide a set of rules that encourage modern, functional-orientated, coding standards in C#. Pure functions, no inheritance, no global state, immutable variables and short, concise sections of code.
   
## What's implemented
This project is as much about me exploring and learning about Roslyn as creating a set of analyzers that encourage clean code practices. One of the things I've learned so far is that Roslyn analyzers fall into two categories: syntax analyzers and semantic ones. The first set, as the name implies, simply check syntax with no semantic context. These are fast, as no compilation of the code is required. Semantic analyzers either are triggered by, or trigger themselves, a compilation of the code in order to obtain semantic information about the tokens in the code.

So far, I've concentrated solely on syntactic analyzers as they are simpler to get my head around! Ones implemented thus far are:

**StaticMethodMustNotBeVoid**

**StaticMethodMustHaveAtLeastOneParameter**

**MethodParameterMustNotBeRefOrOut**

For details of each of these, please see the descriptions below.

## What's planned
### Static higher order functions (SHOFs)
Static methods that create or access global state are well recognised as code smells: they create thread-unsafe units of code with tight coupling and they make unit testing harder than it need be. Functional languages though also have static functions, called higher order (or pure) functions. These functions are completely deterministic and are well recognised as a way of avoiding coupling, simplifying testing and for creating thread-safe code.

To distinguish between stateful static methods and methods that copy the principles of higher order functions, [Mike Hadlow](https://twitter.com/mikehadlow/status/646645950656708608) coined the term Static Higher Order Function (SHOF) to identify the latter in OO languages, such as C#.

I plan on creating a set of analyzers that determine if a static method is a SHOF:

#### StaticMethodMustNotBeVoid
To count as a SHOF, a static method must have no side effects, and thus to have a purpose, it must return a value.

#### StaticMethodMustHaveAtLeastOneParameter
To count as a SHOF, a static method must not receive data from any source other than its parameters, thus it must have at least one parameter to have a purpose.

#### StaticMethodMustNotAccessState
A SHOF must only derive a result from the supplied parameter(s). It must not access any static field or property.

#### StaticMethodMustNotCreateState
The only result from calling a SHOF must be the returned value, or an exception. Therefore it must not invoke any method that has a void return type, nor write to any field or property.

### Immutability
A badly named variable that has many values written to it over the course of a long-winded method makes for hard-to-read code. Variables that only written to once both make for easier reading, as a new appropriately named variable must be created for each assignment, and encourage the principle of least surprise. The following rules enforce that idea:

####ParameterShouldNotBeModified
Reports when a parameter is used as a mutable variable.

####VariableShouldBeAssignedOnce
Reports when a variable is re-assigned.

####InterfacePropertyShouldBeReadOnly
Properties defined by interfaces should not have setters.

####ClassPropertyShouldBePubliclyReadOnly
Class properties should not have public setters.

###No global/static state
Global (and to a lesser extent, any static) state creates a testing and maintenance nightmare as code becomes both tightly coupled and tests must be run in sequence through fear that two tests might call two pieces of code that both modify shared state resulting in brittle tests. These rules prevent global and static state:

####DoNotUseStatefulStaticMethods
Static methods that either take no parameters or have no return type use static state and so should not be used. This rule will clearly need to support suppressing violations as the .NET framework is littered with such methods. It should encourage encapsulating such functionality though.

####DoNotUseStaticFields
Static fields contain static scope and so should be avoided.

####DoNotUseStaticProperties
Static properties contain static scope and so should be avoided.

###Liskov Substitution Principle
From the [Liskov Substitution Principle](https://en.wikipedia.org/wiki/Liskov_substitution_principle) article on Wikipedia:
> Substitutability is a principle in object-oriented programming. It states that, in a computer program, if S is a subtype of T, then objects of type T may be replaced with objects of type S (i.e., objects of type S may substitute objects of type T) without altering any of the desirable properties of that program (correctness, task performed, etc.)

Two exceptions defined by the .NET framework are a direct violation of this principle (LSP) and the following two rules check for such violations:

####DoNotUseNotImplementedException
NotImplementedException violates LSP and should not be used.

####DoNotUseNotSupportedException
NotSupportedException violates LSP and should not be used.

###Single responsibility principle
The [single responsibility principle](https://en.wikipedia.org/wiki/Single_responsibility_principle) article on Wikipedia states:
> the single responsibility principle states that every class should have responsibility over a single part of the functionality provided by the software, and that responsibility should be entirely encapsulated by the class. All its services should be narrowly aligned with that responsibility

The following rules extend this principle to methods and files and seek to identify sections of code that may break that principle:

####MethodParameterMustNotBeRefOrOut
Reports when either `ref` or `out` are used for any parameter of any method. Both of these keywords lead to a method that returns two results via two different routes, breaking the single responsibility principle.

####MethodTooLong
A long method is likely to break the principle by carrying out too many functions. What is too long is highly subjective, so the rule would need to be configurable to allow the maximum lines to be specified.

####FileTooLong
A long class is likely to break the principle by carrying out too many functions. What is too long is highly subjective, so the rule would need to be configurable to allow the maximum lines to be specified.

####MethodNameContainsAnd
Methods that contain "And" in their name often undertake two tasks and thus have two responsibilities. Thus they are likely to violate the principle.

####FileMustOnlyContainOneTypeDefinition
A file that contains more than one class, enum or interface definition inherently contains more than one responsibility.  