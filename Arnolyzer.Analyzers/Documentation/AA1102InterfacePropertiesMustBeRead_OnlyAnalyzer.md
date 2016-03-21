# Interface Properties Must Be Read-Only
**Report code: AA1102-InterfacePropertiesMustBeReadOnly**

## Summary
<table>
<tr>
  <td><strong>Description</strong></td>
  <td>To provide encapsulation, properties should only make getters publicly available, so interfaces should not define setters for properties.</td>
</tr>
<tr>
  <td><strong>Category</strong></td>
  <td>Encapsulation Analyzers</td>
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

Therefore, interfaces should not, in most cases, define setters as that forces implementing classes down the above violation routes.

## How to fix violations

There currently aren't any implemented code-fixes for this rule.

## How to suppress violations

This rule can be suppressed using the following attributes: 

**[MutableProperty]**<br/>
A public setter may sometimes be required. It is therefore allowed if decorated with the MutableProperty attribute as it explicitly reaffirms the need for the property to be mutable.
