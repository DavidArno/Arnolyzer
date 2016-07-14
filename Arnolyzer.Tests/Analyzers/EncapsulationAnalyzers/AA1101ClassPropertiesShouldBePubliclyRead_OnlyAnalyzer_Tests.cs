using Arnolyzer.Analyzers;
using Arnolyzer.Analyzers.EncapsulationAnalyzers;
using Arnolyzer.Tests.DiagnosticVerification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuccincT.Options;
using static System.String;

namespace Arnolyzer.Tests.Analyzers.EncapsulationAnalyzers
{
    [TestClass]
    public class AA1101ClassPropertiesShouldBePubliclyRead_OnlyAnalyzer_Tests
    {
        [TestMethod]
        public void AnalyzerCorrectlySetsItsDetails()
        {
            var details = new AA1101ClassPropertiesShouldBePubliclyRead_OnlyAnalyzer().GetAnalyzerDetails();
            Assert.AreEqual(AnalyzerCategories.EncapsulationAnalyzers, details.Category);
            Assert.AreEqual("Class Properties Should Be Publicly Read-Only", details.Name);
            Assert.AreEqual("AA1101ClassPropertiesShouldBePubliclyReadOnly", details.DiagnosticId);
            Assert.AreEqual(Resources.AA1101ClassPropertiesShouldBePubliclyReadOnlyTitle, details.Title.ToString());
            Assert.AreEqual(Resources.AA1101ClassPropertiesShouldBePubliclyReadOnlyDescription, details.Description.ToString());
            Assert.AreEqual(Resources.AA1101ClassPropertiesShouldBePubliclyReadOnlyMessageFormat, details.MessageFormat.ToString());
        }

        [TestMethod]
        public void NoCode_ShouldYieldNoDiagnostics() =>
            DiagnosticVerifier.VerifyDiagnostics<AA1101ClassPropertiesShouldBePubliclyRead_OnlyAnalyzer>(
                @"..\..\CodeUnderTest\EmptyFile.cs");

        [TestMethod]
        public void CodeInIgnoredFile_YieldsNoDiagnostics() =>
            DiagnosticVerifier.VerifyDiagnostics<AA1101ClassPropertiesShouldBePubliclyRead_OnlyAnalyzer>(
                TestFiles.FileThatShouldBeIgnored);

        [TestMethod]
        public void ClassPropertiesWithSetters_YieldsDiagnostics()
        {
            var commonExpected =
                new DiagnosticResultCommonProperties(new AA1101ClassPropertiesShouldBePubliclyRead_OnlyAnalyzer());
            var expected1 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.AA1101ClassPropertiesShouldBePubliclyReadOnlyMessageFormat,
                                            "Property2",
                                            "ContainsSetters"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(10, 38, 41)));

            var expected2 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.AA1101ClassPropertiesShouldBePubliclyReadOnlyMessageFormat,
                                            "Property7",
                                            "ContainsSetters"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(20, 13, 16)));

            DiagnosticVerifier.VerifyDiagnostics<AA1101ClassPropertiesShouldBePubliclyRead_OnlyAnalyzer>(
                @"..\..\CodeUnderTest\CodeToTestClassPropertySetters.cs",
                expected1,
                expected2);
        }
    }
}