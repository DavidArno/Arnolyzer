using Arnolyzer.Analyzers.SingleResponsibilityAnalyzers;
using Arnolyzer.Tests.DiagnosticVerification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuccincT.Options;
using static System.String;
using static Arnolyzer.Tests.SyntacticAnalyzers.TestFiles;

namespace Arnolyzer.Tests.SyntacticAnalyzers.SingleResponsibilityAnalyzersTests
{
    [TestClass]
    public class AA2100MethodParametersMustNotBeRefOrOutAnalyzerTests
    {
        [TestMethod]
        public void NoCode_ShouldYieldNoDiagnostics() =>
            DiagnosticVerifier.VerifyDiagnostics<AA2100MethodParametersMustNotBeRefOrOutAnalyzer>(EmptyFile);

        [TestMethod]
        public void MethodsWithRefOrOutParams_YieldsDiagnostics()
        {
            var commonExpected =
                new DiagnosticResultCommonProperties(new AA2100MethodParametersMustNotBeRefOrOutAnalyzer());

            var expected1 = new DiagnosticResult(commonExpected,
                                                 Format(Resources.AA2100MethodParametersMustNotBeRefOrOutMessageFormat,
                                                        "p",
                                                        "UsesRefParameter",
                                                        "ref"),
                                                 Option<DiagnosticLocation>.Some(new DiagnosticLocation(11, 45, 54)));

            var expected2 = new DiagnosticResult(commonExpected,
                                                 Format(Resources.AA2100MethodParametersMustNotBeRefOrOutMessageFormat,
                                                        "p",
                                                        "UsesOutParameter",
                                                        "out"),
                                                 Option<DiagnosticLocation>.Some(new DiagnosticLocation(17, 45, 54)));

            DiagnosticVerifier.VerifyDiagnostics<AA2100MethodParametersMustNotBeRefOrOutAnalyzer>(
                CodeToTestDetectingOutAndRefParameters,
                expected1,
                expected2);
        }
    }
}