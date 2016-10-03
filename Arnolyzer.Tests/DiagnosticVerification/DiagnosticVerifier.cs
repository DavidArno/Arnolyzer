using System;
using System.Linq;
using Arnolyzer.RuleExceptionAttributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Arnolyzer.Tests.DiagnosticVerification
{
    internal static class DiagnosticVerifier
    {
        private const string SettingsFile = @"..\..\arnolyzer.yml";

        [HasSideEffects]
        public static void VerifyDiagnostics<T>(string filePath, params DiagnosticResult[] expected)
            where T : DiagnosticAnalyzer, new()
        {
            var analyzer = new T();
            var diagnostics = DiagnosticsGenerator.GetSortedDiagnostics(filePath, analyzer, SettingsFile);
            VerifyDiagnosticResults(diagnostics, analyzer, expected);
        }

        private static void VerifyDiagnosticResults(IOrderedEnumerable<Diagnostic> actualResults,
                                                    DiagnosticAnalyzer analyzer,
                                                    params DiagnosticResult[] expectedResults)
        {
            var analyzerName = analyzer.GetType().Name;
            var actualResultsArray = actualResults.ToArray();

            VerifyCorrectNumberOfDiagnostics(analyzerName, actualResultsArray, expectedResults);

            for (var i = 0; i < expectedResults.Length; i++)
            {
                var actual = actualResultsArray[i];
                var expected = expectedResults[i];
                var diagnosticNum = i + 1;

                VerifyLocationOfDiagnostic(analyzerName, diagnosticNum, expected, actual);
                VerifyCategoryOfDiagnostic(analyzerName, diagnosticNum, expected, actual);
                VerifyIdOfDiagnostic(analyzerName, diagnosticNum, expected, actual);
                VerifyTitleOfDiagnostic(analyzerName, diagnosticNum, expected, actual);
                VerifyDescriptionOfDiagnostic(analyzerName, diagnosticNum, expected, actual);
                VerifyMessageOfDiagnostic(analyzerName, diagnosticNum, expected, actual);
                VerifySeverityOfDiagnostic(analyzerName, diagnosticNum, expected, actual);
            }
        }

        private static void VerifyCorrectNumberOfDiagnostics(string analyzerName,
                                                             Diagnostic[] actualResults,
                                                             DiagnosticResult[] expectedResults)
        {
            var expectedCount = expectedResults.Length;
            var actualCount = actualResults.Length;

            IsTrue(expectedCount == actualCount,
                          GenerateAssertMessage(analyzerName,
                                                "",
                                                "Mismatch between number of diagnostics returned",
                                                () => expectedCount.ToString(),
                                                () => actualCount.ToString()));
        }

        private static void VerifyLocationOfDiagnostic(string analyzerName,
                                                       int diagnosticNum,
                                                       DiagnosticResult expected,
                                                       Diagnostic actual)
        {
            IsTrue(expected.Location.HasValue && actual.Location != Location.None,
                   GenerateAssertMessage(analyzerName,
                                         expected.CommonProperties.Id,
                                         $"Expected diagnostic {diagnosticNum} with no location",
                                         () => "No location",
                                         () => FormatLocation(CreateCoordsFromActual(actual.Location))));

            var expectedCoords = CreateCoordsFromExpected(expected.Location.Value);
            var actualCoords = CreateCoordsFromActual(actual.Location);
            IsTrue(Equals(expectedCoords, actualCoords),
                   GenerateAssertMessage(analyzerName,
                                         expected.CommonProperties.Id,
                                         $"Expected and actual diagnostic {diagnosticNum} locations differ",
                                         () => FormatLocation(CreateCoordsFromExpected(expected.Location.Value)),
                                         () => FormatLocation(CreateCoordsFromActual(actual.Location))));
        }

        private static void VerifyIdOfDiagnostic(string analyzerName, int diagnosticNum, DiagnosticResult expected, Diagnostic actual)
        {
            IsTrue(Equals(expected.CommonProperties.Id, actual.Id),
                          GenerateAssertMessage(analyzerName,
                                                expected.CommonProperties.Id,
                                                $"The diagnostic {diagnosticNum}'s ID is different to that expected",
                                                () => expected.CommonProperties.Id,
                                                () => actual.Id));
        }

        private static void VerifyCategoryOfDiagnostic(string analyzerName,
                                                       int diagnosticNum,
                                                       DiagnosticResult expected,
                                                       Diagnostic actual)
        {
            IsTrue(Equals(expected.CommonProperties.Category, actual.Descriptor.Category),
                   GenerateAssertMessage(analyzerName,
                                         expected.CommonProperties.Id,
                                         $"The diagnostic {diagnosticNum}'s category is different to that expected",
                                         () => expected.CommonProperties.Category,
                                         () => actual.Descriptor.Category));
        }

        private static void VerifyTitleOfDiagnostic(string analyzerName,
                                                    int diagnosticNum,
                                                    DiagnosticResult expected,
                                                    Diagnostic actual)
        {
            IsTrue(Equals(expected.CommonProperties.Title, actual.Descriptor.Title.ToString()),
                   GenerateAssertMessage(analyzerName,
                                         expected.CommonProperties.Id,
                                         $"The diagnostic {diagnosticNum}'s title is different to that expected",
                                         () => expected.CommonProperties.Title,
                                         () => actual.Descriptor.Title.ToString()));
        }

        private static void VerifyDescriptionOfDiagnostic(string analyzerName,
                                                          int diagnosticNum,
                                                          DiagnosticResult expected,
                                                          Diagnostic actual)
        {
            IsTrue(Equals(expected.CommonProperties.Description, actual.Descriptor.Description.ToString()),
                   GenerateAssertMessage(analyzerName,
                                         expected.CommonProperties.Id,
                                         $"The diagnostic ({diagnosticNum})'s description is different to that expected",
                                         () => expected.CommonProperties.Description,
                                         () => actual.Descriptor.Description.ToString()));
        }

        private static void VerifyMessageOfDiagnostic(string analyzerName,
                                                      int diagnosticNum,
                                                      DiagnosticResult expected,
                                                      Diagnostic actual)
        {
            IsTrue(Equals(expected.Message, actual.GetMessage()),
                   GenerateAssertMessage(analyzerName,
                                         expected.CommonProperties.Id,
                                         $"The diagnostic ({diagnosticNum})'s message is different to that expected",
                                         () => expected.Message,
                                         () => actual.GetMessage()));
        }

        private static void VerifySeverityOfDiagnostic(string analyzerName,
                                                       int diagnosticNum,
                                                       DiagnosticResult expected,
                                                       Diagnostic actual)
        {
            IsTrue(Equals(expected.CommonProperties.Id, actual.Id),
                   GenerateAssertMessage(analyzerName,
                                         expected.CommonProperties.Id,
                                         $"The severity of diagnostic {diagnosticNum} is different to that expected",
                                         () => expected.CommonProperties.Severity.ToString(),
                                         () => actual.Severity.ToString()));
        }

        private static string GenerateAssertMessage(string analyzerName,
                                                    string diagnosticId,
                                                    string message,
                                                    Func<string> expectedResult,
                                                    Func<string> actualResult)
        {
            return $"\r\n{FormatDiagnosticDetails(analyzerName, diagnosticId)} - {message}\r\n" +
                   $"Expected: {expectedResult()}\r\n" +
                   $"Actual: {actualResult()}";
        }

        private static string FormatDiagnosticDetails(string analyzerName, string diagnosticId) =>
            $"{analyzerName}{SlashIfNeeded(diagnosticId)}{diagnosticId}";

        private static string SlashIfNeeded(string diagnosticId) => diagnosticId == "" ? "" : "/";

        private static string FormatLocation(Coords location) =>
            $"from line {location.StartLine}, col {location.StartColumn} " +
            $"to line {location.EndLine}, col {location.EndColumn}";

        private static Coords CreateCoordsFromExpected(DiagnosticLocation location) =>
            new Coords
            {
                StartLine = location.Line,
                StartColumn = location.StartColumn,
                EndLine = location.Line,
                EndColumn = location.EndColumn
            };

        private static Coords CreateCoordsFromActual(Location location)
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