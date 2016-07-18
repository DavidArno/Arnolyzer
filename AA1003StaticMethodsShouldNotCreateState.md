---
title: Arnolyzer - AA1003 - Static Methods Should Not Create State
layout: default
---
# AA1003 - Static Methods Should Not Create State #
**Report code: AA1003-StaticMethodsShouldNotCreateState**

## Summary ##
<table>
<tr>
  <td><strong>Status</strong></td>
  <td>Planned for a future release</td>
</tr>
<tr>
  <td><strong>Description</strong></td>
  <td>The only result from calling a static method should be the returned value, or an exception. Therefore it should not invoke any method that has no parameters and/or a void return type, nor write to any field or property.</td>
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



## How to fix violations ##

Not yet implemented.

## How to suppress violations ##

Not yet implemented.
