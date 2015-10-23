using System.IO;
using Arnolyzer.SHOFAnalyzers;
using Arnolyzer.Test.DiagnosticVerification;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuccincT.Options;

namespace Arnolyzer.Test.SyntacticAnalyzers.SHOFAnalyzersTests
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
            var expected = new DiagnosticResult(
                Option<DiagnosticResultLocation>.Some(new DiagnosticResultLocation("Test0.cs", 5, 28, 38)),
                DiagnosticSeverity.Error,
                "StaticMethodMustHaveAtLeastOneParameter");

            DiagnosticVerifier.VerifyDiagnostics<StaticMethodMustHaveAtLeastOneParameterAnalyzer>(test, expected);
        }
    }
}