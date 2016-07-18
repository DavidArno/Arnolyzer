---
title: Arnolyzer - AA2104 - File Must Only Contain One Type Definition
layout: default
---
# AA2104 - File Must Only Contain One Type Definition #
**Report code: AA2104-FileMustOnlyContainOneTypeDefinition**

## Summary ##
<table>
<tr>
  <td><strong>Status</strong></td>
  <td>Implemented</td>
</tr>
<tr>
  <td><strong>Description</strong></td>
  <td>To comply with the single responsibility principle, a file should only contain one non-private type definition.</td>
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
  <td>Error</td>
</tr>
</table>

## Cause ##

A file that contains more than one class, enum or interface definition inherently contains more than one responsibility.

In addition, the discoverability of these types is made harder as the pattern of matching type and file names breaks down, as there's no clear way of naming the file to match the contents.

## How to fix violations ##

There currently aren't any implemented code-fixes for this rule.

## How to suppress violations ##

This rule cannot be suppressed.
