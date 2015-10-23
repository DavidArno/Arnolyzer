using System.IO;
using Arnolyzer.SHOFAnalyzers;
using Arnolyzer.Test.DiagnosticVerification;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuccincT.Options;

namespace Arnolyzer.Test.SyntacticAnalyzers.SHOFAnalyzersTests
{
    [TestClass]
    public class StaticMethodMustNotBeVoidAnalyzerTests
    {

        //No diagnostics expected to show up
        [TestMethod]
        public void NoCode_ShouldYieldNoDiagnostics()
        {
            const string test = @"";
            DiagnosticVerifier.VerifyDiagnostics< StaticMethodMustNotBeVoidAnalyzer>(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void CodeWithTwoVoidMethods_YieldsTwoDiagnostics()
        {
            var test = File.ReadAllText(@"..\..\CodeUnderTest\CodeToTestStaticVoidAnalyzer.cs");
            var expected1 = new DiagnosticResult(
                Option<DiagnosticResultLocation>.Some(new DiagnosticResultLocation("Test0.cs", 5, 28, 37)),
                DiagnosticSeverity.Error,
                "StaticMethodMustNotBeVoid");

            var expected2 = new DiagnosticResult(
                Option<DiagnosticResultLocation>.Some(new DiagnosticResultLocation("Test0.cs", 8, 28, 42)),
                DiagnosticSeverity.Error,
                "StaticMethodMustNotBeVoid");

            DiagnosticVerifier.VerifyDiagnostics<StaticMethodMustNotBeVoidAnalyzer>(test, expected1, expected2);
        }
    }
}