using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using static Arnolyzer.Test.DiagnosticVerification.DocumentSetCreator;

namespace Arnolyzer.Test.DiagnosticVerification
{
    internal static class DiagnosticsGenerator
    {
        public static Diagnostic[] GetSortedDiagnostics(string[] sources, DiagnosticAnalyzer analyzer) =>
            GetSortedDiagnosticsFromDocuments(analyzer, CreateDocumentSetFromSources(sources));

        private static Diagnostic[] GetSortedDiagnosticsFromDocuments(DiagnosticAnalyzer analyzer, Document[] documents)
        {
            var projects = new HashSet<Project>();
            foreach (var document in documents)
            {
                projects.Add(document.Project);
            }

            var diagnostics = new List<Diagnostic>();
            foreach (var project in projects)
            {
                var compilationWithAnalyzers = project.GetCompilationAsync().Result.WithAnalyzers(ImmutableArray.Create(analyzer));
                var diags = compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync().Result;
                foreach (var diag in diags)
                {
                    if (diag.Location == Location.None || diag.Location.IsInMetadata)
                    {
                        diagnostics.Add(diag);
                    }
                    else
                    {
                        for (int i = 0; i < documents.Length; i++)
                        {
                            var document = documents[i];
                            var tree = document.GetSyntaxTreeAsync().Result;
                            if (tree == diag.Location.SourceTree)
                            {
                                diagnostics.Add(diag);
                            }
                        }
                    }
                }
            }

            var results = SortDiagnostics(diagnostics);
            diagnostics.Clear();
            return results;
        }

        private static Diagnostic[] SortDiagnostics(IEnumerable<Diagnostic> diagnostics) =>
            diagnostics.OrderBy(d => d.Location.SourceSpan.Start).ToArray();
    }
}
