# Static Methods Must Not Be Void
**Report code: AA1000-StaticMethodsMustNotBeVoid**

## Summary
<table>
<tr>
  <td><strong>Description</strong></td>
  <td>Static methods should return a value (must not be null)</td>
</tr>
<tr>
  <td><strong>Category</strong></td>
  <td>SHOF Analyzers</td>
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



## How to fix violations

There currently aren't any implemented code-fixes for this rule.

## How to suppress violations

This rule can be suppressed using the following attributes: 

**[MutatesParameter]**<br/>
A static method annotated with MutatesParameter attribute is allowed to be void as it is explicitly declaring its intent to return a result through mutating the contents of a parameter.

**[HasSideEffects]**<br/>
A static method annotated with HasSideEffects attribute is allowed to be void and have no parameters as it is explicitly declaring its intent to cause side effects.
