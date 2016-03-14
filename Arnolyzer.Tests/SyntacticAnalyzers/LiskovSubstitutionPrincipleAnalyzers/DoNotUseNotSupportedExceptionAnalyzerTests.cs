﻿using Arnolyzer.SyntacticAnalyzers;
using Arnolyzer.SyntacticAnalyzers.LiskovSubstitutionPrincipleAnalyzers;
using Arnolyzer.Tests.DiagnosticVerification;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuccincT.Options;
using static System.String;

namespace Arnolyzer.Tests.SyntacticAnalyzers.LiskovSubstitutionPrincipleAnalyzers
{
    [TestClass]
    public class DoNotUseNotSupportedExceptionAnalyzerTests
    {
        [TestMethod]
        public void NoCode_ShouldYieldNoDiagnostics() =>
            DiagnosticVerifier.VerifyDiagnostics<DoNotUseNotSupportedExceptionAnalyzer>(
                @"..\..\CodeUnderTest\EmptyFile.cs");

        [TestMethod]
        public void ThrowingNotSupported_YieldsDiagnostics()
        {
            var commonExpected =
                new DiagnosticResultCommonProperties(Resources.DoNotUseNotSupportedExceptionTitle,
                                                     Resources.DoNotUseNotSupportedExceptionDescription,
                                                     DiagnosticSeverity.Error,
                                                     AnalyzerCategories.EncapsulationAnalyzers,
                                                     DoNotUseNotSupportedExceptionAnalyzer.DiagnosticId);
            var expected1 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.DoNotUseNotSupportedExceptionMessageFormat),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(47, 23, 44)));

            var expected2 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.DoNotUseNotSupportedExceptionMessageFormat),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(52, 19, 22)));

            var expected3 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.DoNotUseNotSupportedExceptionMessageFormat),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(58, 19, 21)));

            var expected4 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.DoNotUseNotSupportedExceptionMessageFormat),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(63, 19, 26)));

            DiagnosticVerifier.VerifyDiagnostics<DoNotUseNotSupportedExceptionAnalyzer>(
                @"..\..\CodeUnderTest\CodeToTestLSPViolatingExceptions.cs",
                expected1,
                expected2,
                expected3,
                expected4);
        }
    }
}