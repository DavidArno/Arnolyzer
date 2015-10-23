using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Arnolyzer.Test.DiagnosticVerification.DiagnosticsGenerator;

namespace Arnolyzer.Test.DiagnosticVerification
{
    internal static class DiagnosticVerifier
    {
        public static void VerifyDiagnostics<T>(string source, params DiagnosticResult[] expected) where T : DiagnosticAnalyzer, new()
        {
            var analyzer = new T();
            var diagnostics = GetSortedDiagnostics(new[] { source }, analyzer);
            VerifyDiagnosticResults(diagnostics, analyzer, expected);
        }

        private static void VerifyDiagnosticResults(Diagnostic[] actualResults, 
                                                    DiagnosticAnalyzer analyzer, 
                                                    params DiagnosticResult[] expectedResults)
        {
            VerifyCorrectNumberOfDiagnostics(actualResults, expectedResults);

            for (int i = 0; i < expectedResults.Count(); i++)
            {
                var actual = actualResults[i];
                var expected = expectedResults[i];

                VerifyLocationOfDiagnostic(analyzer, expected, actual);
                VerifyIdOfDiagnostic(analyzer, expected, actual);
                VerifySeverityOfDiagnostic(analyzer, expected, actual);
            }
        }

        private static void VerifyCorrectNumberOfDiagnostics(Diagnostic[] actualResults, DiagnosticResult[] expectedResults)
        {
            var expectedCount = expectedResults.Count();
            var actualCount = actualResults.Count();

            Assert.AreEqual(expectedCount,
                actualCount,
                $"Mismatch between number of diagnostics returned, expected {expectedCount}; actual {actualCount}");
        }

        private static void VerifyLocationOfDiagnostic(DiagnosticAnalyzer analyzer,
                                                       DiagnosticResult expected, 
                                                       Diagnostic actual)
        {
            if (!expected.Location.HasValue && actual.Location != Location.None)
            {
                Assert.Fail(GenerateAssertMessage(analyzer,
                                                  expected,
                                                  "Expected a diagnostic with no location",
                                                  () => "No location",
                                                  () => FormatLocation(CreateCoordsFromLocation(actual.Location))));
            }

            var expectedCoords = CreateCoordsFromExpectedLocation(expected.Location.Value);
            var actualCoords = CreateCoordsFromLocation(actual.Location);
            Assert.AreEqual(expectedCoords, 
                            actualCoords,
                            GenerateAssertMessage(analyzer,
                                                  expected,
                                                  "Expected and actual diagnostic locations differ",
                                                  () => FormatLocation(CreateCoordsFromExpectedLocation(expected.Location.Value)),
                                                  () => FormatLocation(CreateCoordsFromLocation(actual.Location))));

        }

        private static void VerifyIdOfDiagnostic(DiagnosticAnalyzer analyzer, DiagnosticResult expected, Diagnostic actual)
        {
            if (actual.Id != expected.Id)
            {
                Assert.AreEqual(expected.Id,
                                actual.Id,
                                GenerateAssertMessage(analyzer,
                                                      expected,
                                                      "The diagnostic is different to that expected",
                                                      () => expected.Id,
                                                      () => actual.Id));
            }
        }

        private static void VerifySeverityOfDiagnostic(DiagnosticAnalyzer analyzer, DiagnosticResult expected, Diagnostic actual)
        {
            if (actual.Severity != expected.Severity)
            {
                Assert.AreEqual(expected.Id,
                                actual.Id,
                                GenerateAssertMessage(analyzer,
                                                      expected,
                                                      "The severity of the diagnostic is different to that expected",
                                                      () => expected.Severity.ToString(),
                                                      () => actual.Severity.ToString()));
            }
        }

        private static string GenerateAssertMessage(DiagnosticAnalyzer analyzer,
                                                    DiagnosticResult diagnosticResult,
                                                    string message,
                                                    Func<string> expectedResult,
                                                    Func<string> actualResult) =>
                $"{FormatDiagnosticDetails(analyzer, diagnosticResult)} - {message}\r\n" +
                $"Expected: {expectedResult()}\r\n" +
                $"Actual: {actualResult()}";

        private static string FormatDiagnosticDetails(DiagnosticAnalyzer analyzer, DiagnosticResult diagnosticResult) => 
            $"{analyzer.GetType().Name}/{diagnosticResult.Id}";

        private static string FormatLocation(Coords location) => 
            $"from line {location.StartLine}, col {location.StartColumn} " +
            $"to line {location.EndLine}, col {location.EndColumn}";

        private static Coords CreateCoordsFromExpectedLocation(DiagnosticResultLocation location) => 
            new Coords
            {
                StartLine = location.Line,
                StartColumn = location.StartColumn,
                EndLine = location.Line,
                EndColumn = location.EndColumn
            };

        private static Coords CreateCoordsFromLocation(Location location)
        {
            var startPosition = location.GetLineSpan().StartLinePosition;
            var endPosition = location.GetLineSpan().EndLinePosition;
            return new Coords
            {
                StartLine = startPosition.Line + 1,
                StartColumn = startPosition.Character + 1,
                EndLine = endPosition.Line + 1,
                EndColumn = endPosition.Character + 1
            };
        }

        private struct Coords
        {
            public int StartLine;
            public int StartColumn;
            public int EndLine;
            public int EndColumn;
        }
    }
}
