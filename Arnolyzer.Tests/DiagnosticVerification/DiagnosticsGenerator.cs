using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Arnolyzer.Tests.DiagnosticVerification
{
    internal static class DiagnosticsGenerator
    {
        public static Diagnostic[] GetSortedDiagnostics(string[] sources, DiagnosticAnalyzer analyzer) =>
            GetSortedDiagnosticsFromDocuments(analyzer, DocumentSetCreator.CreateDocumentSetFromSources(sources));

        private static Diagnostic[] GetSortedDiagnosticsFromDocuments(DiagnosticAnalyzer analyzer, Document[] documents)
        {
            var projects = (from document in documents select document.Project);
            var diagnostics = projects.Select(project => project.GetCompilationAsync()
                                                                .Result
                                                                .WithAnalyzers(ImmutableArray.Create(analyzer)))
                                      .SelectMany(compilationWithAnalyzers =>
                                                  compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync().Result);

            return SortDiagnostics(diagnostics);
        }

        private static Diagnostic[] SortDiagnostics(IEnumerable<Diagnostic> diagnostics) =>
            diagnostics.OrderBy(d => d.Location.SourceSpan.Start).ToArray();
    }
}