using Arnolyzer.Analyzers;
using Arnolyzer.Analyzers.EncapsulationAnalyzers;
using Arnolyzer.Tests.DiagnosticVerification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuccincT.Options;
using static System.String;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Arnolyzer.Tests.SyntacticAnalyzers.EncapsulationAnalyzers
{
    [TestClass]
    public class AA1103ClassPropertiesShouldBePubliclyRead_OnlyAnalyzer_Tests
    {
        [TestMethod]
        public void AnalyzerCorrectlySetsItsDetails()
        {
            var details = new AA1103ClassPropertiesShouldBePubliclyRead_OnlyAnalyzer().GetAnalyzerDetails();
            AreEqual(AnalyzerCategories.EncapsulationAndImmutabilityAnalyzers, details.Category);
            AreEqual("Class Properties Should Be Publicly Read-Only", details.Name);
            AreEqual("AA1103ClassPropertiesShouldBePubliclyReadOnly", details.DiagnosticId);
            AreEqual(Resources.AA1103ClassPropertiesShouldBePubliclyReadOnlyTitle, details.Title.ToString());
            AreEqual(Resources.AA1103ClassPropertiesShouldBePubliclyReadOnlyDescription, details.Description.ToString());
            AreEqual(Resources.AA1103ClassPropertiesShouldBePubliclyReadOnlyMessageFormat, details.MessageFormat.ToString());
        }

        [TestMethod]
        public void NoCode_ShouldYieldNoDiagnostics() =>
            DiagnosticVerifier.VerifyDiagnostics<AA1103ClassPropertiesShouldBePubliclyRead_OnlyAnalyzer>(
                @"..\..\CodeUnderTest\EmptyFile.cs");

        [TestMethod]
        public void ClassPropertiesWithSetters_YieldsDiagnostics()
        {
            var commonExpected =
                new DiagnosticResultCommonProperties(new AA1103ClassPropertiesShouldBePubliclyRead_OnlyAnalyzer());
            var expected1 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.AA1103ClassPropertiesShouldBePubliclyReadOnlyMessageFormat,
                                            "Property2",
                                            "ContainsSetters"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(10, 38, 41)));

            var expected2 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.AA1103ClassPropertiesShouldBePubliclyReadOnlyMessageFormat,
                                            "Property7",
                                            "ContainsSetters"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(20, 13, 16)));

            DiagnosticVerifier.VerifyDiagnostics<AA1103ClassPropertiesShouldBePubliclyRead_OnlyAnalyzer>(
                @"..\..\CodeUnderTest\CodeToTestClassPropertySetters.cs",
                expected1,
                expected2);
        }
    }
}