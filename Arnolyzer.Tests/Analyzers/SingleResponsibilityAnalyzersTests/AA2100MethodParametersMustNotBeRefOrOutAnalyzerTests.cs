using System;
using Arnolyzer.Analyzers.SingleResponsibilityAnalyzers;
using Arnolyzer.Tests.DiagnosticVerification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuccincT.Options;

namespace Arnolyzer.Tests.Analyzers.SingleResponsibilityAnalyzersTests
{
    [TestClass]
    public class AA2100MethodParametersMustNotBeRefOrOutAnalyzerTests
    {
        [TestMethod]
        public void NoCode_ShouldYieldNoDiagnostics() =>
            DiagnosticVerifier.VerifyDiagnostics<AA2100MethodParametersMustNotBeRefOrOutAnalyzer>(TestFiles.EmptyFile);

        [TestMethod]
        public void MethodsWithRefOrOutParams_YieldsDiagnostics()
        {
            var commonExpected =
                new DiagnosticResultCommonProperties(new AA2100MethodParametersMustNotBeRefOrOutAnalyzer());

            var expected1 = new DiagnosticResult(commonExpected,
                                                 String.Format(Resources.AA2100MethodParametersMustNotBeRefOrOutMessageFormat,
                                                        "p",
                                                        "UsesRefParameter",
                                                        "ref"),
                                                 Option<DiagnosticLocation>.Some(new DiagnosticLocation(11, 45, 54)));

            var expected2 = new DiagnosticResult(commonExpected,
                                                 String.Format(Resources.AA2100MethodParametersMustNotBeRefOrOutMessageFormat,
                                                        "p",
                                                        "UsesOutParameter",
                                                        "out"),
                                                 Option<DiagnosticLocation>.Some(new DiagnosticLocation(17, 45, 54)));

            DiagnosticVerifier.VerifyDiagnostics<AA2100MethodParametersMustNotBeRefOrOutAnalyzer>(
                TestFiles.CodeToTestDetectingOutAndRefParameters,
                expected1,
                expected2);
        }
    }
}