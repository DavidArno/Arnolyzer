using Arnolyzer.SyntacticAnalyzers.EncapsulationAnalyzers;
using Arnolyzer.Tests.DiagnosticVerification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuccincT.Options;
using static System.String;

namespace Arnolyzer.Tests.SyntacticAnalyzers.EncapsulationAnalyzers
{
    [TestClass]
    public class AA1102InterfacePropertiesMustBeRead_OnlyAnalyzerTests
    {
        [TestMethod]
        public void NoCode_ShouldYieldNoDiagnostics() =>
            DiagnosticVerifier.VerifyDiagnostics<AA1102InterfacePropertiesMustBeRead_OnlyAnalyzer>(
                @"..\..\CodeUnderTest\EmptyFile.cs");

        [TestMethod]
        public void InterfacePropertiesWithSetters_YieldsDiagnostics()
        {
            var commonExpected =
                new DiagnosticResultCommonProperties(new AA1102InterfacePropertiesMustBeRead_OnlyAnalyzer());

            var expected1 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.AA1102InterfacePropertiesMustBeReadOnlyMessageFormat,
                                            "Property2",
                                            "IContainsSetters"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(9, 26, 29)));

            var expected2 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.AA1102InterfacePropertiesMustBeReadOnlyMessageFormat,
                                            "Property3",
                                            "IContainsSetters"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(11, 30, 33)));

            var expected3 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.AA1102InterfacePropertiesMustBeReadOnlyMessageFormat,
                                            "Property4",
                                            "IContainsSetters"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(12, 26, 29)));

            DiagnosticVerifier.VerifyDiagnostics<AA1102InterfacePropertiesMustBeRead_OnlyAnalyzer>(
                @"..\..\CodeUnderTest\CodeToTestInterfacePropertySetters.cs",
                expected1,
                expected2,
                expected3);
        }
    }
}