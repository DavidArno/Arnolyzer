# Inner Types Must Be Private
**Report code: AA1104-InnerTypesMustBePrivate**

## Summary
<table>
<tr>
  <td><strong>Description</strong></td>
  <td>Inner types should be treated as implementation details and encapsulated by marking them as private</td>
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

Inner types that are marked `internal` or `public` are likely doing one (or, in some cases, both) of two things:

1. They are using the outer class as a namespace label,
2. They are exposing the inner workings of the outer class, thus weakening encapsulation.

In the first case, the type should be moved to its own file in an appropriate directory to give it a proper namespace name.

In the latter case, either the type should be made `private`, to properly encapsulate the inner workings of the outer class, 
or the two types should be redesigned and the inner type moved to its own file and marked as `internal` or `public` as appropriate. 


## How to fix violations

There currently aren't any implemented code-fixes for this rule.

## How to suppress violations

This rule cannot be suppressed.
