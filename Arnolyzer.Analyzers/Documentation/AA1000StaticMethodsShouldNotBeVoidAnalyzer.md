# Static Methods Should Not Be Void
**Report code: AA1000-StaticMethodsShouldNotBeVoid**

## Summary
<table>
<tr>
  <td><strong>Description</strong></td>
  <td>Static methods should return a value (must not be null)</td>
</tr>
<tr>
  <td><strong>Category</strong></td>
  <td>Pure-Function Analyzers</td>
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

## Cause

Static methods that take an input, and generate a deterministic output from that input, without having any side-effects are termed pure functions. These methods are thread-safe and easy to test.

Static methods that are declared `void`, do not meet the critia of a pure function: they will have side-effects as they do not return a result. These methods are likely to not be thread-safe or easy to test as a result. Therefore `void` methods should not be made static.

## How to fix violations

There currently aren't any implemented code-fixes for this rule.

## How to suppress violations

This rule can be suppressed using the following attributes: 

**[HasSideEffects]**
A static method annotated with HasSideEffects attribute is allowed to be void and have no parameters as it is explicitly declaring its intent to cause side effects.

**[MutatesParameter]**
A static method annotated with MutatesParameter attribute is allowed to be void as it is explicitly declaring its intent to return a result through mutating the contents of a parameter.
