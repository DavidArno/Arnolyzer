using Arnolyzer.Analyzers;
using Microsoft.CodeAnalysis;

namespace Arnolyzer.Tests.DiagnosticVerification
{
    internal class DiagnosticResultCommonProperties
    {
        public DiagnosticResultCommonProperties(IAnalyzerDetailsReporter analyzer)
        {
            var details = analyzer.GetAnalyzerDetails();
            Severity = details.Severity;
            Title = details.Title.ToString();
            Description = details.Description.ToString();
            Category = details.Category.Name;
            Id = details.DiagnosticId;
        }

        public string Title { get; }

        public string Description { get; }

        public DiagnosticSeverity Severity { get; }

        public string Category { get; }

        public string Id { get; }
    }
}