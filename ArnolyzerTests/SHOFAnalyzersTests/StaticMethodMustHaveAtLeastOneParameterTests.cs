using System.IO;
using Arnolyzer.SHOFAnalyzers;
using Arnolyzer.Test.DiagnosticVerification;
using Arnolyzer.Test.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Arnolyzer.Test.SHOFAnalyzersTests
{
    [TestClass]
    public class StaticMethodMustHaveAtLeastOneParameterTests
    {

        //No diagnostics expected to show up
        [TestMethod]
        public void NoCode_ShouldYieldNoDiagnostics()
        {
            var test = @"";

            DiagnosticVerifier.VerifyDiagnostics<StaticMethodMustHaveAtLeastOneParameterAnalyzer>(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void StaticMethodWithNoParams_YieldsADiagnostic()
        {
            var test = File.ReadAllText(@"..\..\CodeUnderTest\CodeToTestAtLeastOneParameterAnalyzer.cs");
            var expected = new DiagnosticResult
            {
                Id = "StaticMethodMustHaveAtLeastOneParameter",
                Severity = DiagnosticSeverity.Error,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 5, 28) }
            };

            DiagnosticVerifier.VerifyDiagnostics<StaticMethodMustHaveAtLeastOneParameterAnalyzer>(test, expected);
        }
    }
}