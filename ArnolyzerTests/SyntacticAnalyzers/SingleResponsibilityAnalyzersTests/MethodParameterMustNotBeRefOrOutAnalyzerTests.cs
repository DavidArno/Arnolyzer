using System.IO;
using Arnolyzer.SyntacticAnalyzers;
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
        public void MethodsWithRefOrOutParams_YieldsDiagnostics()
        {
            var test = File.ReadAllText(@"..\..\CodeUnderTest\CodeToTestDetectingOutAndRefParameters.cs");
            var expected1 = new DiagnosticResult(
                Option<DiagnosticResultLocation>.Some(new DiagnosticResultLocation("Test0.cs", 11, 45, 54)),
                DiagnosticSeverity.Error,
                AnalyzerCategories.SingleResponsibiltyAnalyzers,
                MethodParameterMustNotBeRefOrOutAnalyzer.DiagnosticId);

            var expected2 = new DiagnosticResult(
                Option<DiagnosticResultLocation>.Some(new DiagnosticResultLocation("Test0.cs", 17, 45, 54)),
                DiagnosticSeverity.Error,
                AnalyzerCategories.SingleResponsibiltyAnalyzers,
                MethodParameterMustNotBeRefOrOutAnalyzer.DiagnosticId);

            DiagnosticVerifier.VerifyDiagnostics<MethodParameterMustNotBeRefOrOutAnalyzer>(test, expected1, expected2);
        }
    }
}