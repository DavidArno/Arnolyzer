using System.IO;
using Arnolyzer.SyntacticAnalyzers;
using Arnolyzer.SyntacticAnalyzers.SHOFAnalyzers;
using Arnolyzer.Test.DiagnosticVerification;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuccincT.Options;
using static System.String;

namespace Arnolyzer.Test.SyntacticAnalyzers.SHOFAnalyzersTests
{
    [TestClass]
    public class StaticMethodMustHaveAtLeastOneParameterTests
    {
        //No diagnostics expected to show up
        [TestMethod]
        public void NoCode_ShouldYieldNoDiagnostics() =>
            DiagnosticVerifier.VerifyDiagnostics<StaticMethodMustHaveAtLeastOneParameterAnalyzer>("");

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void StaticMethodWithNoParams_YieldsADiagnostic()
        {
            var test = File.ReadAllText(@"..\..\CodeUnderTest\CodeToTestAtLeastOneParameterAnalyzer.cs");
            var commonExpected =
                new DiagnosticResultCommonProperties(Resources.StaticMethodMustHaveAtLeastOneParameterTitle,
                                                     Resources.StaticMethodMustHaveAtLeastOneParameterDescription,
                                                     DiagnosticSeverity.Error,
                                                     AnalyzerCategories.ShofAnalyzers,
                                                     StaticMethodMustHaveAtLeastOneParameterAnalyzer.DiagnosticId);
            var expected =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.StaticMethodMustHaveAtLeastOneParameterMessageFormat,
                                            "ReturnTrue"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(5, 28, 38)));

            DiagnosticVerifier.VerifyDiagnostics<StaticMethodMustHaveAtLeastOneParameterAnalyzer>(test, expected);
        }
    }
}