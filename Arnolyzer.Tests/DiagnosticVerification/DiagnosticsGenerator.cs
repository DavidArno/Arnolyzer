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
        public static Diagnostic[] GetSortedDiagnostics(string[] sources,
                                                        DiagnosticAnalyzer analyzer)
        {
            return GetSortedDiagnosticsFromDocuments(analyzer,
                                                     DocumentSetCreator.CreateDocumentSetFromSources(sources),
                                                     null);
        }

        public static Diagnostic[] GetSortedDiagnosticsUsingSettings(string[] sources,
                                                                     DiagnosticAnalyzer analyzer,
                                                                     string settingsPath)
        {
            var options = new AnalyzerOptions(ImmutableArray.Create<AdditionalText>(new SettingsFile(settingsPath)));
            return GetSortedDiagnosticsFromDocuments(analyzer,
                                                     DocumentSetCreator.CreateDocumentSetFromSources(sources),
                                                     options);
        }

        private static Diagnostic[] GetSortedDiagnosticsFromDocuments(DiagnosticAnalyzer analyzer,
                                                                      Document[] documents,
                                                                      AnalyzerOptions options)
        {
            var projects = (from document in documents select document.Project);

            var diagnostics = projects.Select(project => project.GetCompilationAsync()
                                                                .Result
                                                                .WithAnalyzers(ImmutableArray.Create(analyzer),
                                                                               options))
                                      .SelectMany(compilationWithAnalyzers =>
                                                  compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync().Result);

            return SortDiagnostics(diagnostics);
        }

        private static Diagnostic[] SortDiagnostics(IEnumerable<Diagnostic> diagnostics) =>
            diagnostics.OrderBy(d => d.Location.SourceSpan.Start).ToArray();

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