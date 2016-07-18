---
title: Arnolyzer - AA1101 - Class Properties Should Be Publicly Read-Only
layout: default
---
# AA1101 - Class Properties Should Be Publicly Read-Only #
**Report code: AA1101-ClassPropertiesShouldBePubliclyReadOnly**

## Summary ##
<table>
<tr>
  <td><strong>Status</strong></td>
  <td>Implemented</td>
</tr>
<tr>
  <td><strong>Description</strong></td>
  <td>Public classes should not provide publicly accessible setters for properties</td>
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

Class properties should not have public setters. Properties with public setters become mutable from any other point in the code that has access to the object instance, making it much harder to track changes to the object's state.

## How to fix violations ##

There currently aren't any implemented code-fixes for this rule.

## How to suppress violations ##

This rule can be suppressed using the following attributes: 

**[MutableProperty]**<br/>A public setter may sometimes be required. It is therefore allowed if decorated with the MutableProperty attribute as it explicitly asserts the need for the property to be mutable.
