# Arnolyzer
Clean-code Roslyn-based analyzer for C# 6

## What's implemented
Nothing yet.

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
Words about immutability to go here.

####ParameterShouldNotBeModified
Reports when a parameter is used as a mutable variable.

####VariableShouldBeAssignedOnce
Reports when a variable is re-assigned.

####InterfacePropertyShouldBeReadOnly
Properties defined by interfaces should not have setters.

####ClassPropertyShouldBePubliclyReadOnly
Class properties should not have public setters.
