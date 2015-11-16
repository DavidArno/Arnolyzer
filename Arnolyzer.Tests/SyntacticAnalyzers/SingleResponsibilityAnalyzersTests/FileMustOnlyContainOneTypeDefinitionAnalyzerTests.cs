﻿using System;
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
    public class FileMustOnlyContainOneTypeDefinitionAnalyzerTests
    {
        [TestMethod]
        public void NoCode_ShouldYieldNoDiagnostics() =>
            DiagnosticVerifier.VerifyDiagnostics<MethodParameterMustNotBeRefOrOutAnalyzer>("");

        [TestMethod]
        public void MethodsWithRefOrOutParams_YieldsDiagnostics()
        {
            var test = File.ReadAllText(@"..\..\CodeUnderTest\CodeToTestFileMustOnlyContainOneTypeDefinition.cs");
            var commonExpected =
                new DiagnosticResultCommonProperties(Resources.FileMustOnlyContainOneTypeDefinitionTitle,
                                                     Resources.FileMustOnlyContainOneTypeDefinitionDescription,
                                                     DiagnosticSeverity.Error,
                                                     AnalyzerCategories.SingleResponsibiltyAnalyzers,
                                                     FileMustOnlyContainOneTypeDefinitionAnalyzer.DiagnosticId);
            var expected1 =
                new DiagnosticResult(commonExpected,
                                     String.Format(Resources.FileMustOnlyContainOneTypeDefinitionMessageFormat, "Class1"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(3, 18, 24)));

            var expected2 =
                new DiagnosticResult(commonExpected,
                                     String.Format(Resources.FileMustOnlyContainOneTypeDefinitionMessageFormat, "Struct2"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(11, 21, 28)));

            var expected3 =
                new DiagnosticResult(commonExpected,
                                     String.Format(Resources.FileMustOnlyContainOneTypeDefinitionMessageFormat, "Interface3"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(19, 22, 32)));

            var expected4 =
                new DiagnosticResult(commonExpected,
                                     String.Format(Resources.FileMustOnlyContainOneTypeDefinitionMessageFormat, "Enum3"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(21, 19, 24)));

            DiagnosticVerifier.VerifyDiagnostics<FileMustOnlyContainOneTypeDefinitionAnalyzer>(test,
                                                                                               expected1,
                                                                                               expected2,
                                                                                               expected3,
                                                                                               expected4);
        }
    }
}