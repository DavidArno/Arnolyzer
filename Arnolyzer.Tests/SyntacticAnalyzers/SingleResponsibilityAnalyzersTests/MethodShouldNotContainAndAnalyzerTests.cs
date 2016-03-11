using Arnolyzer.SyntacticAnalyzers;
using Arnolyzer.SyntacticAnalyzers.SingleResponsibilityAnalyzers;
using Arnolyzer.Tests.DiagnosticVerification;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuccincT.Options;
using static System.String;
using static Arnolyzer.Tests.SyntacticAnalyzers.TestFiles;

namespace Arnolyzer.Tests.SyntacticAnalyzers.SingleResponsibilityAnalyzersTests
{
    [TestClass]
    public class MethodShouldNotContainAndAnalyzerTests
    {
        [TestMethod]
        public void NoCode_ShouldYieldNoDiagnostics() =>
            DiagnosticVerifier.VerifyDiagnostics<MethodShouldNotContainAndAnalyzer>(EmptyFile);

        [TestMethod]
        public void MethodsWithAnd_YieldsDiagnostics()
        {
            var commonExpected =
                new DiagnosticResultCommonProperties(Resources.MethodShouldNotContainAndTitle,
                                                     Resources.MethodShouldNotContainAndDescription,
                                                     DiagnosticSeverity.Warning,
                                                     AnalyzerCategories.SingleResponsibiltyAnalyzers,
                                                     MethodShouldNotContainAndAnalyzer.DiagnosticId);
            var expected1 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.MethodShouldNotContainAndMessageFormat, "DoSomethingAndSomethingElse"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(7, 28, 55)));

            var expected2 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.MethodShouldNotContainAndMessageFormat, "DoSomethingAndSomethingElse"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(14, 28, 55)));

            var expected3 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.MethodShouldNotContainAndMessageFormat, "AThingAndAnotherThing"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(16, 21, 42)));

            var expected4 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.MethodShouldNotContainAndMessageFormat, "AndAndAnd"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(20, 25, 34)));

            var expected5 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.MethodShouldNotContainAndMessageFormat, "AThingAndAnotherThing"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(27, 14, 35)));

            DiagnosticVerifier.VerifyDiagnostics<MethodShouldNotContainAndAnalyzer>(
                CodeToTestMethodsWithAnd,
                expected1,
                expected2,
                expected3,
                expected4,
                expected5);
        }
    }
}