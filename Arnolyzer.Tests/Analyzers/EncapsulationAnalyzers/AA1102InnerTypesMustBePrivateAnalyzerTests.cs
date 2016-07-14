using System;
using Arnolyzer.Analyzers.EncapsulationAnalyzers;
using Arnolyzer.Tests.DiagnosticVerification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuccincT.Options;

namespace Arnolyzer.Tests.Analyzers.EncapsulationAnalyzers
{
    [TestClass]
    public class AA1102InnerTypesMustBePrivateAnalyzerTests
    {
        [TestMethod]
        public void NoCode_ShouldYieldNoDiagnostics() =>
            DiagnosticVerifier.VerifyDiagnostics<AA1102InnerTypesMustBePrivateAnalyzer>(@"..\..\CodeUnderTest\EmptyFile.cs");

        [TestMethod]
        public void NonPrivateInnerTypes_YieldDiagnostics()
        {
            var commonExpected =
                new DiagnosticResultCommonProperties(new AA1102InnerTypesMustBePrivateAnalyzer());
            var expected1 =
                new DiagnosticResult(commonExpected,
                                     String.Format(Resources.AA1102InnerTypesMustBePrivateMessageFormat, "Interface1"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(7, 28, 38)));

            var expected2 =
                new DiagnosticResult(commonExpected,
                                     String.Format(Resources.AA1102InnerTypesMustBePrivateMessageFormat, "Enum1"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(8, 21, 26)));

            var expected3 =
                new DiagnosticResult(commonExpected,
                                     String.Format(Resources.AA1102InnerTypesMustBePrivateMessageFormat, "Class2"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(9, 22, 28)));

            DiagnosticVerifier.VerifyDiagnostics<AA1102InnerTypesMustBePrivateAnalyzer>(
                @"..\..\CodeUnderTest\CodeToTestInnerTypesMustBePrivate.cs",
                expected1,
                expected2,
                expected3);
        }
    }
}