using System.IO;
using Arnolyzer.SyntacticAnalyzers;
using Arnolyzer.SyntacticAnalyzers.SHOFAnalyzers;
using Arnolyzer.Test.DiagnosticVerification;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuccincT.Options;
using static System.String;

namespace Arnolyzer.Test.SyntacticAnalyzers.SHOFAnalyzersTests
{
    [TestClass]
    public class StaticMethodMustNotBeVoidAnalyzerTests
    {
        //No diagnostics expected to show up
        [TestMethod]
        public void NoCode_ShouldYieldNoDiagnostics() =>
            DiagnosticVerifier.VerifyDiagnostics<StaticMethodMustNotBeVoidAnalyzer>("");

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void CodeWithTwoVoidMethods_YieldsTwoDiagnostics()
        {
            var test = File.ReadAllText(@"..\..\CodeUnderTest\CodeToTestStaticVoidAnalyzer.cs");
            var commonExpected = new DiagnosticResultCommonProperties(Resources.StaticMethodMustNotBeVoidTitle,
                                                                      Resources.StaticMethodMustNotBeVoidDescription,
                                                                      DiagnosticSeverity.Error,
                                                                      AnalyzerCategories.ShofAnalyzers,
                                                                      StaticMethodMustNotBeVoidAnalyzer.DiagnosticId);
            var expected1 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.StaticMethodMustNotBeVoidMessageFormat, "DoNothing"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(5, 28, 37)));

            var expected2 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.StaticMethodMustNotBeVoidMessageFormat, "StillDoNothing"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(8, 28, 42)));

            DiagnosticVerifier.VerifyDiagnostics<StaticMethodMustNotBeVoidAnalyzer>(test, expected1, expected2);
        }
    }
}