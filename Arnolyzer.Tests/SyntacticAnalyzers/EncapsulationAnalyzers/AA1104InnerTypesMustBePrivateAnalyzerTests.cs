using Arnolyzer.Analyzers.EncapsulationAnalyzers;
using Arnolyzer.Tests.DiagnosticVerification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuccincT.Options;
using static System.String;

namespace Arnolyzer.Tests.SyntacticAnalyzers.EncapsulationAnalyzers
{
    [TestClass]
    public class AA1104InnerTypesMustBePrivateAnalyzerTests
    {
        [TestMethod]
        public void NoCode_ShouldYieldNoDiagnostics() =>
            DiagnosticVerifier.VerifyDiagnostics<AA1104InnerTypesMustBePrivateAnalyzer>(@"..\..\CodeUnderTest\EmptyFile.cs");

        [TestMethod]
        public void NonPrivateInnerTypes_YieldDiagnostics()
        {
            var commonExpected =
                new DiagnosticResultCommonProperties(new AA1104InnerTypesMustBePrivateAnalyzer());
            var expected1 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.AA1104InnerTypesMustBePrivateMessageFormat, "Interface1"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(7, 28, 38)));

            var expected2 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.AA1104InnerTypesMustBePrivateMessageFormat, "Enum1"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(8, 21, 26)));

            var expected3 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.AA1104InnerTypesMustBePrivateMessageFormat, "Class2"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(9, 22, 28)));

            DiagnosticVerifier.VerifyDiagnostics<AA1104InnerTypesMustBePrivateAnalyzer>(
                @"..\..\CodeUnderTest\CodeToTestInnerTypesMustBePrivate.cs",
                expected1,
                expected2,
                expected3);
        }
    }
}