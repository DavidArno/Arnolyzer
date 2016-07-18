---
title: Arnolyzer - AA1100 - Interface Properties Should Be Read-Only
layout: default
---
# AA1100 - Interface Properties Should Be Read-Only #
**Report code: AA1100-InterfacePropertiesShouldBeReadOnly**

## Summary ##
<table>
<tr>
  <td><strong>Status</strong></td>
  <td>Implemented</td>
</tr>
<tr>
  <td><strong>Description</strong></td>
  <td>To provide encapsulation, properties should only make getters publicly available, so interfaces should not define setters for properties.</td>
</tr>
<tr>
  <td><strong>Category</strong></td>
  <td><a href="EncapsulationAnalyzers.html">Encapsulation Analyzers</a></td>
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

Interface properties should not have setters. Doing so forces implemnting classes to have public setters on those properties and so they become mutable from any other point in the code that has access to the object instance, making it much harder to track changes to the object's state.

## How to fix violations ##

There currently aren't any implemented code-fixes for this rule.

## How to suppress violations ##

This rule can be suppressed using the following attributes: 

**[MutableProperty]**<br/>A public setter may sometimes be required. It is therefore allowed if decorated with the MutableProperty attribute as it explicitly asserts the need for the property to be mutable.
