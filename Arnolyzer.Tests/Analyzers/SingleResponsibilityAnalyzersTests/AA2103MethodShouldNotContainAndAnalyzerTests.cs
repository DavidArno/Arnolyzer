using Arnolyzer.Analyzers.SingleResponsibilityAnalyzers;
using Arnolyzer.Tests.DiagnosticVerification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuccincT.Options;
using static System.String;

namespace Arnolyzer.Tests.Analyzers.SingleResponsibilityAnalyzersTests
{
    [TestClass]
    public class AA2103MethodShouldNotContainAndAnalyzerTests
    {
        [TestMethod]
        public void NoCode_ShouldYieldNoDiagnostics() =>
            DiagnosticVerifier.VerifyDiagnostics<AA2103MethodShouldNotContainAndAnalyzer>(TestFiles.EmptyFile);

        [TestMethod]
        public void CodeInIgnoredFile_YieldsNoDiagnostics() =>
            DiagnosticVerifier.VerifyDiagnostics<AA2103MethodShouldNotContainAndAnalyzer>(
                TestFiles.FileThatShouldBeIgnored);

        [TestMethod]
        public void MethodsWithAnd_YieldsDiagnostics()
        {
            var commonExpected =
                new DiagnosticResultCommonProperties(new AA2103MethodShouldNotContainAndAnalyzer());
            var expected1 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.AA2103MethodShouldNotContainAndMessageFormat, "DoSomethingAndSomethingElse"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(7, 28, 55)));

            var expected2 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.AA2103MethodShouldNotContainAndMessageFormat, "DoSomethingAndSomethingElse"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(14, 28, 55)));

            var expected3 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.AA2103MethodShouldNotContainAndMessageFormat, "AThingAndAnotherThing"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(16, 21, 42)));

            var expected4 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.AA2103MethodShouldNotContainAndMessageFormat, "AndAndAnd"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(20, 25, 34)));

            var expected5 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.AA2103MethodShouldNotContainAndMessageFormat, "AThingAndAnotherThing"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(27, 14, 35)));

            DiagnosticVerifier.VerifyDiagnostics<AA2103MethodShouldNotContainAndAnalyzer>(
                TestFiles.CodeToTestMethodsWithAnd,
                expected1,
                expected2,
                expected3,
                expected4,
                expected5);
        }
    }
}