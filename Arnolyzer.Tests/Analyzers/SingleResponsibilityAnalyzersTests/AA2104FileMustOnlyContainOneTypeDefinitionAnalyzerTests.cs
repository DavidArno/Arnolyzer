using System;
using Arnolyzer.Analyzers.SingleResponsibilityAnalyzers;
using Arnolyzer.Tests.DiagnosticVerification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuccincT.Options;

namespace Arnolyzer.Tests.Analyzers.SingleResponsibilityAnalyzersTests
{
    [TestClass]
    public class AA2104FileMustOnlyContainOneTypeDefinitionAnalyzerTests
    {
        [TestMethod]
        public void NoCode_ShouldYieldNoDiagnostics() =>
            DiagnosticVerifier.VerifyDiagnostics<AA2104FileMustOnlyContainOneTypeDefinitionAnalyzer>(TestFiles.EmptyFile);

        [TestMethod]
        public void MethodsWithRefOrOutParams_YieldsDiagnostics()
        {
            var commonExpected =
                new DiagnosticResultCommonProperties(new AA2104FileMustOnlyContainOneTypeDefinitionAnalyzer());
            var expected1 =
                new DiagnosticResult(commonExpected,
                                     String.Format(Resources.AA2104FileMustOnlyContainOneTypeDefinitionMessageFormat, "Class1"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(3, 18, 24)));

            var expected2 =
                new DiagnosticResult(commonExpected,
                                     String.Format(Resources.AA2104FileMustOnlyContainOneTypeDefinitionMessageFormat, "Struct2"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(11, 21, 28)));

            var expected3 =
                new DiagnosticResult(commonExpected,
                                     String.Format(Resources.AA2104FileMustOnlyContainOneTypeDefinitionMessageFormat, "Interface3"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(19, 22, 32)));

            var expected4 =
                new DiagnosticResult(commonExpected,
                                     String.Format(Resources.AA2104FileMustOnlyContainOneTypeDefinitionMessageFormat, "Enum3"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(21, 19, 24)));

            DiagnosticVerifier.VerifyDiagnostics<AA2104FileMustOnlyContainOneTypeDefinitionAnalyzer>(
                TestFiles.CodeToTestFileMustOnlyContainOneTypeDefinition,
                expected1,
                expected2,
                expected3,
                expected4);
        }
    }
}