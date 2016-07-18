---
title: Arnolyzer - Pure-Function Analyzers
layout: default
---
# Pure-Function Analyzers #
Static methods that create or access global state are well recognised as code smells: they create thread-unsafe units of code with tight coupling and they make unit testing harder than it need be. Functional languages though also have static functions, called pure functions. These functions are completely deterministic and are well recognised as a way of avoiding coupling, simplifying testing and for creating thread-safe code.

The pure function analyzers identify a set of condictions that generally indicate a static methodcreating or accessing global state.

## Implemented analyzers ##
* [AA1000 - Static Methods Should Not Be Void](AA1000StaticMethodsShouldNotBeVoid.html)
* [AA1001 - Static Methods Should Have At Least One Parameter](AA1001StaticMethodsShouldHaveAtLeastOneParameter.html)



## Planned analyzers ##
* [AA1002 - Static Methods Should Not Access State](AA1002StaticMethodsShouldNotAccessState.html)
* [AA1003 - Static Methods Should Not Create State](AA1003StaticMethodsShouldNotCreateState.html)


