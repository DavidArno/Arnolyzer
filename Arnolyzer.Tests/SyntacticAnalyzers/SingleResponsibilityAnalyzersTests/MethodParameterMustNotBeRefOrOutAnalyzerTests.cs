using System;
using System.IO;
using Arnolyzer.SyntacticAnalyzers;
using Arnolyzer.SyntacticAnalyzers.SingleResponsibilityAnalyzers;
using Arnolyzer.Tests.DiagnosticVerification;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuccincT.Options;

namespace Arnolyzer.Tests.SyntacticAnalyzers.SingleResponsibilityAnalyzersTests
{
    [TestClass]
    public class MethodParameterMustNotBeRefOrOutAnalyzerTests
    {
        [TestMethod]
        public void NoCode_ShouldYieldNoDiagnostics() =>
            DiagnosticVerifier.VerifyDiagnostics<MethodParameterMustNotBeRefOrOutAnalyzer>("");

        [TestMethod]
        public void MethodsWithRefOrOutParams_YieldsDiagnostics()
        {
            var test = File.ReadAllText(@"..\..\CodeUnderTest\CodeToTestDetectingOutAndRefParameters.cs");
            var commonExpected =
                new DiagnosticResultCommonProperties(Resources.MethodParameterMustNotBeRefOrOutTitle,
                                                     Resources.MethodParameterMustNotBeRefOrOutDescription,
                                                     DiagnosticSeverity.Error,
                                                     AnalyzerCategories.SingleResponsibiltyAnalyzers,
                                                     MethodParameterMustNotBeRefOrOutAnalyzer.DiagnosticId);

            var expected1 = new DiagnosticResult(commonExpected,
                                                 String.Format(Resources.MethodParameterMustNotBeRefOrOutMessageFormat,
                                                        "p",
                                                        "UsesRefParameter",
                                                        "ref"),
                                                 Option<DiagnosticLocation>.Some(new DiagnosticLocation(11, 45, 54)));

            var expected2 = new DiagnosticResult(commonExpected,
                                                 String.Format(Resources.MethodParameterMustNotBeRefOrOutMessageFormat,
                                                        "p",
                                                        "UsesOutParameter",
                                                        "out"),
                                                 Option<DiagnosticLocation>.Some(new DiagnosticLocation(17, 45, 54)));

            DiagnosticVerifier.VerifyDiagnostics<MethodParameterMustNotBeRefOrOutAnalyzer>(test, expected1, expected2);
        }
    }
}