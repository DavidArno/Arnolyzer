# Class Properties Must Be Publicly Read-Only
**Report code: AA1103-ClassPropertiesMustBePubliclyReadOnly**

## Summary
<table>
<tr>
  <td><strong>Description</strong></td>
  <td>Public classes should not provide publicly accessible setters for properties</td>
</tr>
<tr>
  <td><strong>Category</strong></td>
  <td>Encapsulation and Immutability Analyzers</td>
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

A property that providers a getter is likely either:
1. Violating encapsulation by providing direct read/write access to a backing field, effectively making that field public, or
2. Breaking the principle of least astonishment by manipulating the value provided to the setter, such that requesting the value
back supplies a different value to that set.

In addition, mutable properties break the immutability pattern: the value can change throughout the application. This can lead to
thread-unsafe code and can make both testing and maintenance harder.

## How to fix violations

There currently aren't any implemented code-fixes for this rule.

## How to suppress violations

This rule can be suppressed using the following attributes: 

**[MutableProperty]**
A public setter may sometimes be required. It is therefore allowed if decorated with the MutableProperty attribute as it explicitly reaffirms the need for the property to be mutable.
