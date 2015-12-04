using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace Arnolyzer.Tests.DiagnosticVerification
{
    internal static class DiagnosticsGenerator
    {
        public static IOrderedEnumerable<Diagnostic> GetSortedDiagnostics(string filePath,
                                                                          DiagnosticAnalyzer analyzer,
                                                                          string settingsPath)
        {
            var options = new AnalyzerOptions(ImmutableArray.Create<AdditionalText>(new SettingsFile(settingsPath)));
            return GetSortedDiagnosticsFromDocuments(analyzer,
                                                     DocumentSetCreator.CreateDocumentSetFromSources(filePath),
                                                     options);
        }

        private static IOrderedEnumerable<Diagnostic> GetSortedDiagnosticsFromDocuments(DiagnosticAnalyzer analyzer,
                                                                                        TextDocument document,
                                                                                        AnalyzerOptions options)
        {
            var diagnostics = document.Project.GetCompilationAsync()
                                      .Result
                                      .WithAnalyzers(ImmutableArray.Create(analyzer),
                                                     options)
                                      .GetAnalyzerDiagnosticsAsync().Result;

            return SortDiagnostics(diagnostics);
        }

        private static IOrderedEnumerable<Diagnostic> SortDiagnostics(IEnumerable<Diagnostic> diagnostics) =>
            diagnostics.OrderBy(d => d.Location.SourceSpan.Start);

        private sealed class SettingsFile : AdditionalText
        {
            private readonly string _path;

            public SettingsFile(string path)
            {
                _path = path;
            }

            public override string Path => _path;

            public override SourceText GetText(CancellationToken cancellationToken = new CancellationToken())
            {
                return SourceText.From(File.ReadAllText(_path));
            }
        }
    }
}