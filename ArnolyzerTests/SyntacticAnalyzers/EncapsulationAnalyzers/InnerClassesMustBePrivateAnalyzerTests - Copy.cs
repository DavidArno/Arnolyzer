﻿using System.IO;
using Arnolyzer.SyntacticAnalyzers;
using Arnolyzer.SyntacticAnalyzers.EncapsulationAnalyzers;
using Arnolyzer.Test.DiagnosticVerification;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuccincT.Options;
using static System.String;

namespace Arnolyzer.Test.SyntacticAnalyzers.EncapsulationAnalyzers
{
    [TestClass]
    public class InterfacePropertiesShouldBeReadonlyTests
    {
        [TestMethod]
        public void NoCode_ShouldYieldNoDiagnostics() =>
            DiagnosticVerifier.VerifyDiagnostics<InterfacePropertiesShouldBeReadonlyAnalyzer>("");

        [TestMethod]
        public void MethodsWithRefOrOutParams_YieldsDiagnostics()
        {
            var test = File.ReadAllText(@"..\..\CodeUnderTest\CodeToTestInterfacePropertySetters.cs");
            var commonExpected =
                new DiagnosticResultCommonProperties(Resources.InterfacePropertiesShouldBeReadonlyTitle,
                                                     Resources.InterfacePropertiesShouldBeReadonlyDescription,
                                                     DiagnosticSeverity.Error,
                                                     AnalyzerCategories.EncapsulationAnalyzers,
                                                     InterfacePropertiesShouldBeReadonlyAnalyzer.DiagnosticId);
            var expected1 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.InterfacePropertiesShouldBeReadonlyMessageFormat,
                                            "Property2",
                                            "IContainsSetters"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(7, 26, 29)));

            var expected2 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.InterfacePropertiesShouldBeReadonlyMessageFormat,
                                            "Property3",
                                            "IContainsSetters"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(9, 30, 33)));

            var expected3 =
                new DiagnosticResult(commonExpected,
                                     Format(Resources.InterfacePropertiesShouldBeReadonlyMessageFormat,
                                            "Property4",
                                            "IContainsSetters"),
                                     Option<DiagnosticLocation>.Some(new DiagnosticLocation(10, 26, 29)));

            DiagnosticVerifier.VerifyDiagnostics<InterfacePropertiesShouldBeReadonlyAnalyzer>(test,
                                                                                              expected1,
                                                                                              expected2,
                                                                                              expected3);
        }
    }
}