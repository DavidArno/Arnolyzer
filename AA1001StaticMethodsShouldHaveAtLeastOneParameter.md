---
title: Arnolyzer - AA1001 - Static Methods Should Have At Least One Parameter
layout: default
---
# AA1001 - Static Methods Should Have At Least One Parameter #
**Report code: AA1001-StaticMethodsShouldHaveAtLeastOneParameter**

## Summary ##
<table>
<tr>
  <td><strong>Status</strong></td>
  <td>Implemented</td>
</tr>
<tr>
  <td><strong>Description</strong></td>
  <td>Static methods should have at least one parameter</td>
</tr>
<tr>
  <td><strong>Category</strong></td>
  <td><a href="Pure-FunctionAnalyzers.html">Pure-Function Analyzers</a></td>
</tr>
<tr>
  <td><strong>Enabled by default:</strong></td>
  <td>Yes</td>
</tr>
<tr>
  <td><strong>Severity:</strong></td>
  <td>Error</td>
</tr>
</table>

## Cause ##

Static methods that take an input, and generate a deterministic output from that input, without having any side-effects are termed pure functions. These methods are thread-safe and easy to test.

Static methods that do not take any parameters, do not meet the critia of a pure function: they will have side-effects as they must derive a result from external resources. These methods are likely to not be thread-safe or easy to test as a result. Therefore methods without parameters should not be made static.


## How to fix violations ##

There currently aren't any implemented code-fixes for this rule.

## How to suppress violations ##

This rule can be suppressed using the following attributes: 

**[HasSideEffects]**<br/>A static method annotated with HasSideEffects attribute is allowed to be void and have no parameters as it is explicitly declaring its intent to cause side effects.

**[ConstantValueProvider]**<br/>A static method annotated with ConstantValueProvider attribute is allowed have no parameters as it is explicitly guaranteeing to return the same result every time it's called.

For example, a static factory method that generates the same shape object each time it's called can use this annotation.
