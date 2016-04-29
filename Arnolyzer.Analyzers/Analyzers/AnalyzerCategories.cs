using System.Collections.Generic;
using static System.Environment;

namespace Arnolyzer.Analyzers
{
    public static class AnalyzerCategories
    {
        public static readonly AnalyzerCategoryDetails PureFunctionAnalyzers = new AnalyzerCategoryDetails(
            "Pure-Function Analyzers", 
            "AA10nn",
            "Static methods that create or access global state are well recognised as code smells: they create " +
            "thread-unsafe units of code with tight coupling and they make unit testing harder than it need be. " +
            "Functional languages though also have static functions, called pure functions. These functions are " +
            "completely deterministic and are well recognised as a way of avoiding coupling, simplifying testing " +
            "and for creating thread-safe code." + NewLine + NewLine +
            "The pure function analyzers identify a set of condictions that generally indicate a static method" +
            "creating or accessing global state.");

        public static readonly AnalyzerCategoryDetails ImmutabilityAnalyzers = new AnalyzerCategoryDetails(
            "Immutability Analyzers", 
            "AA11nn",
            "A badly named variable that has many values written to it over the course of a long-winded method makes " +
            "for hard-to-read, and hard-to-mentally-trace, code. Variables that only written to once both make for " +
            "easier reading, as a new appropriately named variable must be created for each assignment. They create " +
            "easier to understand code as they help maintain the principle of least surprise.");

        public static readonly AnalyzerCategoryDetails GlobalStateAnalyzers = new AnalyzerCategoryDetails(
            "Global State Analyzers",
            "AA12nn",
            "Static, global state creates a testing and maintenance nightmare as code becomes both tightly coupled and " +
            "tests must be run in sequence through fear that two tests might call two pieces of code that both modify " +
            "shared state resulting in brittle tests.");

        public static readonly AnalyzerCategoryDetails EncapsulationAnalyzers = new AnalyzerCategoryDetails(
            "Encapsulation Analyzers",
            "AA13nn",
            "Words about encapsulation needed here.");

        public static readonly AnalyzerCategoryDetails LiskovSubstitutionAnalyzers = new AnalyzerCategoryDetails(
            "Liskov Substitution Principle Analyzers", 
            "AA20nn",
            "From the [Liskov Substitution Principle](https://en.wikipedia.org/wiki/Liskov_substitution_principle) " +
            "article on Wikipedia:" + NewLine +
            "> Substitutability is a principle in object-oriented programming. It states that, in a computer program, " +
            "if S is a subtype of T, then objects of type T may be replaced with objects of type S(i.e., objects of " +
            "type S may substitute objects of type T) without altering any of the desirable properties of that program" +
            "(correctness, task performed, etc.)");

        public static readonly AnalyzerCategoryDetails SingleResponsibiltyAnalyzers = new AnalyzerCategoryDetails(
            "Single Responsibilty Analyzers", 
            "AA21nn",
            "The [single responsibility principle](https://en.wikipedia.org/wiki/Single_responsibility_principle) article" +
            " on Wikipedia states:" + NewLine +
            "> the single responsibility principle states that every class should have responsibility over a single part " +
            "of the functionality provided by the software, and that responsibility should be entirely encapsulated by the " +
            "class. All its services should be narrowly aligned with that responsibility");

        public static readonly IEnumerable<AnalyzerCategoryDetails> AnalyzerCategoryList =
            new List<AnalyzerCategoryDetails>
            {
                PureFunctionAnalyzers,
                ImmutabilityAnalyzers,
                GlobalStateAnalyzers,
                EncapsulationAnalyzers,
                LiskovSubstitutionAnalyzers,
                SingleResponsibiltyAnalyzers
            };
    }
}