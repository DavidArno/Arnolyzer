using System;
using Arnolyzer.Analyzers.EncapsulationAnalyzers;
using Arnolyzer.Tests.DiagnosticVerification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuccincT.Options;

namespace Arnolyzer.Tests.Analyzers.EncapsulationAnalyzers
{
    [TestClass]
    public class AA1100InterfacePropertiesShouldBeRead_OnlyAnalyzerTests
    {
        [TestMethod]
        public void NoCode_ShouldYieldNoDiagnostics() =>
            DiagnosticVerifier.VerifyDiagnostics<AA1100InterfacePropertiesShouldBeRead_OnlyAnalyzer>(
                @"..\..\CodeUnderTest\EmptyFile.cs");

        [TestMethod]
        public void InterfacePropertiesWithSetters_YieldsDiagnostics()
        {
            var commonExpected =
                new DiagnosticResultCommonProperties(new AA1100InterfacePropertiesShouldBeRead_OnlyAnalyzer());

            var expected1 =
                new DiagnosticResult(commonExpected,
                                     String.Format(Resources.AA1100InterfacePropertiesShouldBeReadOnlyMessageFormat,
                                            "Property2",
                                            "IContainsSetters"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(9, 26, 29)));

            var expected2 =
                new DiagnosticResult(commonExpected,
                                     String.Format(Resources.AA1100InterfacePropertiesShouldBeReadOnlyMessageFormat,
                                            "Property3",
                                            "IContainsSetters"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(11, 30, 33)));

            var expected3 =
                new DiagnosticResult(commonExpected,
                                     String.Format(Resources.AA1100InterfacePropertiesShouldBeReadOnlyMessageFormat,
                                            "Property4",
                                            "IContainsSetters"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(12, 26, 29)));

            DiagnosticVerifier.VerifyDiagnostics<AA1100InterfacePropertiesShouldBeRead_OnlyAnalyzer>(
                @"..\..\CodeUnderTest\CodeToTestInterfacePropertySetters.cs",
                expected1,
                expected2,
                expected3);
        }
    }
}