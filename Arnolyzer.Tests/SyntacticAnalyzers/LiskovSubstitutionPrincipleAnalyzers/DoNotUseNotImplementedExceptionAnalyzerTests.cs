using Arnolyzer.SyntacticAnalyzers;
using Arnolyzer.SyntacticAnalyzers.LiskovSubstitutionPrincipleAnalyzers;
using Arnolyzer.Tests.DiagnosticVerification;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuccincT.Options;
using static System.String;

namespace Arnolyzer.Tests.SyntacticAnalyzers.LiskovSubstitutionPrincipleAnalyzers
{
    [TestClass]
    public class DoNotUseNotImplementedExceptionAnalyzerTests
    {
        [TestMethod]
        public void NoCode_ShouldYieldNoDiagnostics() =>
            DiagnosticVerifier.VerifyDiagnostics<DoNotUseNotImplementedExceptionAnalyzer>(
                @"..\..\CodeUnderTest\EmptyFile.cs");

        [TestMethod]
        public void InterfacePropertiesWithSetters_YieldsDiagnostics()
        {
            var commonExpected =
                new DiagnosticResultCommonProperties(Resources.DoNotUseNotImplementedExceptionTitle,
                                                     Resources.DoNotUseNotImplementedExceptionDescription,
                                                     DiagnosticSeverity.Error,
                                                     AnalyzerCategories.EncapsulationAnalyzers,
                                                     DoNotUseNotImplementedExceptionAnalyzer.DiagnosticId);
            var expected1 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.DoNotUseNotImplementedExceptionMessageFormat),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(18, 23, 46)));

            var expected2 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.DoNotUseNotImplementedExceptionMessageFormat),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(23, 19, 22)));

            var expected3 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.DoNotUseNotImplementedExceptionMessageFormat),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(34, 19, 21)));

            var expected4 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.DoNotUseNotImplementedExceptionMessageFormat),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(39, 19, 32)));

            DiagnosticVerifier.VerifyDiagnostics<DoNotUseNotImplementedExceptionAnalyzer>(
                @"..\..\CodeUnderTest\CodeToTestLSPViolatingExceptions.cs",
                expected1,
                expected2,
                expected3,
                expected4);
        }
    }
}