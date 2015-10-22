using System.IO;
using Arnolyzer.SyntacticAnalyzers.SingleResponsibilityAnalyzers;
using Arnolyzer.Test.DiagnosticVerification;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuccincT.Options;

namespace Arnolyzer.Test.SyntacticAnalyzers.SingleResponsibilityAnalyzersTests
{
    [TestClass]
    public class MethodParameterMustNotBeRefOrOutAnalyzerTests
    {

        [TestMethod]
        public void NoCode_ShouldYieldNoDiagnostics()
        {
            var test = @"";

            DiagnosticVerifier.VerifyDiagnostics<MethodParameterMustNotBeRefOrOutAnalyzer>(test);
        }

        [TestMethod]
        public void StaticMethodWithNoParams_YieldsADiagnostic()
        {
            var test = File.ReadAllText(@"..\..\CodeUnderTest\CodeToTestDetectingOutAndRefParameters.cs");
            var expected1 = new DiagnosticResult(
                Option<DiagnosticResultLocation>.Some(new DiagnosticResultLocation("Test0.cs", 5, 28, 28)),
                DiagnosticSeverity.Error,
                "MethodParameterMustNotBeRefOrOrOut");

            var expected2 = new DiagnosticResult(
                Option<DiagnosticResultLocation>.Some(new DiagnosticResultLocation("Test0.cs", 5, 28, 28)),
                DiagnosticSeverity.Error,
                "MethodParameterMustNotBeRefOrOut");

            DiagnosticVerifier.VerifyDiagnostics<MethodParameterMustNotBeRefOrOutAnalyzer>(test, expected1, expected2);
        }
    }
}