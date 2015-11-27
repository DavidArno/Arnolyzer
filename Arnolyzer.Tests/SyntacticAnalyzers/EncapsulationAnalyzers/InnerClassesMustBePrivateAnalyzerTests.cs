using System.IO;
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
    public class InnerTypesMustBePrivateAnalyzerTests
    {
        [TestMethod]
        public void NoCode_ShouldYieldNoDiagnostics() =>
            DiagnosticVerifier.VerifyDiagnostics<InnerTypesMustBePrivateAnalyzer>("");

        [TestMethod]
        public void MethodsWithRefOrOutParams_YieldsDiagnostics()
        {
            var test = File.ReadAllText(@"..\..\CodeUnderTest\CodeToTestInnerTypesMustBePrivate.cs");
            var commonExpected =
                new DiagnosticResultCommonProperties(Resources.InnerTypesMustBePrivateTitle,
                                                     Resources.InnerTypesMustBePrivateDescription,
                                                     DiagnosticSeverity.Error,
                                                     AnalyzerCategories.EncapsulationAnalyzers,
                                                     InnerTypesMustBePrivateAnalyzer.DiagnosticId);
            var expected1 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.InnerTypesMustBePrivateMessageFormat, "Interface1"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(7, 28, 38)));

            var expected2 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.InnerTypesMustBePrivateMessageFormat, "Enum1"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(8, 21, 26)));

            var expected3 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.InnerTypesMustBePrivateMessageFormat, "Class2"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(9, 22, 28)));

            DiagnosticVerifier.VerifyDiagnostics<InnerTypesMustBePrivateAnalyzer>(test,
                                                                                  expected1,
                                                                                  expected2,
                                                                                  expected3);
        }
    }
}