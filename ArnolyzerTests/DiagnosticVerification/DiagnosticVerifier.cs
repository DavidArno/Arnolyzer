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
            var analyzerName = analyzer.GetType().Name;
            VerifyCorrectNumberOfDiagnostics(analyzerName, actualResults, expectedResults);

            for (int i = 0; i < expectedResults.Count(); i++)
            {
                var actual = actualResults[i];
                var expected = expectedResults[i];

                VerifyLocationOfDiagnostic(analyzerName, expected, actual);
                VerifyIdOfDiagnostic(analyzerName, expected, actual);
                VerifySeverityOfDiagnostic(analyzerName, expected, actual);
            }
        }

        private static void VerifyCorrectNumberOfDiagnostics(string analyzerName, Diagnostic[] actualResults, DiagnosticResult[] expectedResults)
        {
            var expectedCount = expectedResults.Count();
            var actualCount = actualResults.Count();

            Assert.IsTrue(expectedCount == actualCount,
                          GenerateAssertMessage(analyzerName,
                                                "",
                                                "Mismatch between number of diagnostics returned",
                                                () => expectedCount.ToString(),
                                                () => actualCount.ToString()));
        }

        private static void VerifyLocationOfDiagnostic(string analyzerName,
                                                       DiagnosticResult expected, 
                                                       Diagnostic actual)
        {
            Assert.IsTrue(expected.Location.HasValue && actual.Location != Location.None,
                          GenerateAssertMessage(analyzerName,
                                                expected.Id,
                                                "Expected a diagnostic with no location",
                                                () => "No location",
                                                () => FormatLocation(CreateCoordsFromLocation(actual.Location))));

            var expectedCoords = CreateCoordsFromExpectedLocation(expected.Location.Value);
            var actualCoords = CreateCoordsFromLocation(actual.Location);
            Assert.IsTrue(Equals(expectedCoords, actualCoords),
                          GenerateAssertMessage(analyzerName,
                                                expected.Id,
                                                "Expected and actual diagnostic locations differ",
                                                () => FormatLocation(CreateCoordsFromExpectedLocation(expected.Location.Value)),
                                                () => FormatLocation(CreateCoordsFromLocation(actual.Location))));

        }

        private static void VerifyIdOfDiagnostic(string analyzerName, DiagnosticResult expected, Diagnostic actual)
        {
            Assert.IsTrue(Equals(expected.Id, actual.Id),
                          GenerateAssertMessage(analyzerName,
                                                expected.Id,
                                                "The diagnostic is different to that expected",
                                                () => expected.Id,
                                                () => actual.Id));
        }

        private static void VerifySeverityOfDiagnostic(string analyzerName, DiagnosticResult expected, Diagnostic actual)
        {
            Assert.IsTrue(Equals(expected.Id, actual.Id),
                          GenerateAssertMessage(analyzerName,
                                                expected.Id,
                                                "The severity of the diagnostic is different to that expected",
                                                () => expected.Severity.ToString(),
                                                () => actual.Severity.ToString()));
        }

        private static string GenerateAssertMessage(string analyzerName,
                                                    string diagnosticId,
                                                    string message,
                                                    Func<string> expectedResult,
                                                    Func<string> actualResult) =>
                $"\r\n{FormatDiagnosticDetails(analyzerName, diagnosticId)} - {message}\r\n" +
                $"Expected: {expectedResult()}\r\n" +
                $"Actual: {actualResult()}";

        private static string FormatDiagnosticDetails(string analyzerName, string diagnosticId) => 
            $"{analyzerName}{SlashIfNeeded(diagnosticId)}{diagnosticId}";

        private static string SlashIfNeeded(string diagnosticId) => diagnosticId == "" ? "" : "/";

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
