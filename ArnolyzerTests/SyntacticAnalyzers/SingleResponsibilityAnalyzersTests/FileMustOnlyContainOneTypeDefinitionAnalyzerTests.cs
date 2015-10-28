using System.IO;
using Arnolyzer.SyntacticAnalyzers;
using Arnolyzer.SyntacticAnalyzers.SingleResponsibilityAnalyzers;
using Arnolyzer.Test.DiagnosticVerification;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuccincT.Options;

namespace Arnolyzer.Test.SyntacticAnalyzers.SingleResponsibilityAnalyzersTests
{
    [TestClass]
    public class FileMustOnlyContainOneTypeDefinitionAnalyzerTests
    {
        [TestMethod]
        public void NoCode_ShouldYieldNoDiagnostics()
        {
            var test = @"";

            DiagnosticVerifier.VerifyDiagnostics<MethodParameterMustNotBeRefOrOutAnalyzer>(test);
        }

        [TestMethod]
        public void MethodsWithRefOrOutParams_YieldsDiagnostics()
        {
            var test = File.ReadAllText(@"..\..\CodeUnderTest\CodeToTestFileMustOnlyContainOneTypeDefinition.cs");
            var expected1 = new DiagnosticResult(
                Option<DiagnosticResultLocation>.Some(new DiagnosticResultLocation("Test0.cs", 3, 18, 24)),
                DiagnosticSeverity.Error,
                AnalyzerCategories.SingleResponsibiltyAnalyzers,
                FileMustOnlyContainOneTypeDefinitionAnalyzer.DiagnosticId);

            var expected2 = new DiagnosticResult(
                Option<DiagnosticResultLocation>.Some(new DiagnosticResultLocation("Test0.cs", 11, 21, 28)),
                DiagnosticSeverity.Error,
                AnalyzerCategories.SingleResponsibiltyAnalyzers,
                FileMustOnlyContainOneTypeDefinitionAnalyzer.DiagnosticId);

            var expected3 = new DiagnosticResult(
                Option<DiagnosticResultLocation>.Some(new DiagnosticResultLocation("Test0.cs", 19, 22, 32)),
                DiagnosticSeverity.Error,
                AnalyzerCategories.SingleResponsibiltyAnalyzers,
                FileMustOnlyContainOneTypeDefinitionAnalyzer.DiagnosticId);

            var expected4 = new DiagnosticResult(
                Option<DiagnosticResultLocation>.Some(new DiagnosticResultLocation("Test0.cs", 21, 19, 24)),
                DiagnosticSeverity.Error,
                AnalyzerCategories.SingleResponsibiltyAnalyzers,
                FileMustOnlyContainOneTypeDefinitionAnalyzer.DiagnosticId);

            DiagnosticVerifier.VerifyDiagnostics<FileMustOnlyContainOneTypeDefinitionAnalyzer>(test, 
                                                                                               expected1, 
                                                                                               expected2,
                                                                                               expected3,
                                                                                               expected4);
        }
    }
}