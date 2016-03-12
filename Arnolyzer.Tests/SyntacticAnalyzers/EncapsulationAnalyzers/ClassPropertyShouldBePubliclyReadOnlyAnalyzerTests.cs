using Arnolyzer.SyntacticAnalyzers;
using Arnolyzer.SyntacticAnalyzers.EncapsulationAnalyzers;
using Arnolyzer.Tests.DiagnosticVerification;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuccincT.Options;
using static System.String;

namespace Arnolyzer.Tests.SyntacticAnalyzers.EncapsulationAnalyzers
{
    [TestClass]
    public class ClassPropertyShouldBePubliclyReadOnlyAnalyzerTests
    {
        [TestMethod]
        public void NoCode_ShouldYieldNoDiagnostics() =>
            DiagnosticVerifier.VerifyDiagnostics<ClassPropertyShouldBePubliclyReadOnlyAnalyzer>(
                @"..\..\CodeUnderTest\EmptyFile.cs");

        [TestMethod]
        public void ClassPropertiesWithSetters_YieldsDiagnostics()
        {
            var commonExpected =
                new DiagnosticResultCommonProperties(Resources.ClassPropertyShouldBePubliclyReadOnlyTitle,
                                                     Resources.ClassPropertyShouldBePubliclyReadOnlyDescription,
                                                     DiagnosticSeverity.Error,
                                                     AnalyzerCategories.EncapsulationAnalyzers,
                                                     ClassPropertyShouldBePubliclyReadOnlyAnalyzer.DiagnosticId);
            var expected1 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.ClassPropertyShouldBePubliclyReadOnlyMessageFormat,
                                            "Property2",
                                            "ContainsSetters"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(8, 38, 41)));

            var expected2 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.ClassPropertyShouldBePubliclyReadOnlyMessageFormat,
                                            "Property7",
                                            "ContainsSetters"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(18, 13, 16)));

            DiagnosticVerifier.VerifyDiagnostics<ClassPropertyShouldBePubliclyReadOnlyAnalyzer>(
                @"..\..\CodeUnderTest\CodeToTestClassPropertySetters.cs",
                expected1,
                expected2);
        }
    }
}