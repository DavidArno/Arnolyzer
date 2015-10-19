using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Arnolyzer.SHOFAnalyzers;
using Arnolyzer.Test.Helpers;
using Arnolyzer.Test.Verifiers;

namespace Arnolyzer.Test
{
    [TestClass]
    public class UnitTest : CodeFixVerifier
    {

        //No diagnostics expected to show up
        [TestMethod]
        public void TestMethod1()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void TestMethod2()
        {
            var test = File.ReadAllText(@"..\..\CodeUnderTest\CodeToTestStaticVoidAnalyzer.cs");
            var expected1 = new DiagnosticResult
            {
                Id = "StaticMethodMustNotBeVoid",
                Severity = DiagnosticSeverity.Error,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 5, 28) }
            };

            var expected2 = new DiagnosticResult
            {
                Id = "StaticMethodMustNotBeVoid",
                Severity = DiagnosticSeverity.Error,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 8, 28) }
            };

            VerifyCSharpDiagnostic(test, expected1, expected2);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer() => new StaticMethodMustNotBeVoidAnalyzer();
    }
}