---
title: Arnolyzer - AA2001 - Do Not Use Not Supported Exception
layout: default
---
# AA2001 - Do Not Use Not Supported Exception #
**Report code: AA2001-DoNotUseNotSupportedException**

## Summary ##
<table>
<tr>
  <td><strong>Status</strong></td>
  <td>Implemented</td>
</tr>
<tr>
  <td><strong>Description</strong></td>
  <td>The NotSupportedException is a direct violation of the Liskov Substitution Principle (LSP) and so must not be used</td>
</tr>
<tr>
  <td><strong>Category</strong></td>
  <td><a href="LiskovSubstitutionPrincipleAnalyzers.html">Liskov Substitution Principle Analyzers</a></td>
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

From the [Liskov Substitution Principle](https://en.wikipedia.org/wiki/Liskov_substitution_principle) (LSP) article on Wikipedia:
> Substitutability is a principle in object-oriented programming. It states that, in a computer program, if S is a subtype of T, then objects of type T may be replaced with objects of type S 
(i.e., objects of type S may substitute objects of type T) without altering any of the desirable properties of that program (correctness, task performed, etc.)

A subtype (or interface implementation) that throws a `NotSupportedException` cannot be used as a substitute to its parent "without altering any of the desirable properties of that program". 
Therefore the use of this exception is a violation of the  LSP.

## How to fix violations ##

There currently aren't any implemented code-fixes for this rule.

## How to suppress violations ##

This rule cannot be suppressed.
