---
title: Arnolyzer - AA2103 - Method Should Not Contain And
layout: default
---
# AA2103 - Method Should Not Contain And #
**Report code: AA2103-MethodShouldNotContainAnd**

## Summary ##
<table>
<tr>
  <td><strong>Status</strong></td>
  <td>Implemented</td>
</tr>
<tr>
  <td><strong>Description</strong></td>
  <td>Method names that contain "And" often indicate a method is doing more than one thing. Consider refacting into two methods.</td>
</tr>
<tr>
  <td><strong>Category</strong></td>
  <td><a href="SingleResponsibiltyAnalyzers.html">Single Responsibilty Analyzers</a></td>
</tr>
<tr>
  <td><strong>Enabled by default:</strong></td>
  <td>Yes</td>
</tr>
<tr>
  <td><strong>Severity:</strong></td>
  <td>Warning</td>
</tr>
</table>

## Cause ##

Methods that contain "And" in their name often undertake two tasks and thus have two responsibilities. Thus they are likely to violate the single responsibility principle.


## How to fix violations ##

There currently aren't any implemented code-fixes for this rule.

## How to suppress violations ##

This rule can be suppressed using the following attributes: 

**[HasSingleResponsibility]**<br/>A method annotated with HasSingleResponsibility is allowed to contain "And" as it is explicitly guaranteeing that it only has one responsibility.
