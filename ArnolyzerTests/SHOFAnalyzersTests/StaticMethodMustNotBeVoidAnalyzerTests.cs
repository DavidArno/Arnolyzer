using System.IO;
using Arnolyzer.SHOFAnalyzers;
using Arnolyzer.Test.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Arnolyzer.Test.DiagnosticVerification.DiagnosticVerifier;

namespace Arnolyzer.Test.SHOFAnalyzersTests
{
    [TestClass]
    public class StaticMethodMustNotBeVoidAnalyzerTests
    {

        //No diagnostics expected to show up
        [TestMethod]
        public void NoCode_ShouldYieldNoDiagnostics()
        {
            const string test = @"";
            VerifyDiagnostics< StaticMethodMustNotBeVoidAnalyzer>(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void CodeWithTwoVoidMethods_YieldsTwoDiagnostics()
        {
            var test = File.ReadAllText(@"..\..\CodeUnderTest\CodeToTestStaticVoidAnalyzer.cs");
            var expected1 = new DiagnosticResult
            {
                Id = "StaticMethodMustNotBeVoid",
                Severity = DiagnosticSeverity.Error,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 5, 28) }
            };

            var expected2 = new DiagnosticResult
            {
                Id = "StaticMethodMustNotBeVoid",
                Severity = DiagnosticSeverity.Error,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 8, 28) }
            };

            VerifyDiagnostics<StaticMethodMustNotBeVoidAnalyzer>(test, expected1, expected2);
        }
    }
}