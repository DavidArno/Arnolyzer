using Arnolyzer.SyntacticAnalyzers;
using Microsoft.CodeAnalysis;

namespace Arnolyzer.Tests.DiagnosticVerification
{
    internal class DiagnosticResultCommonProperties
    {
        public DiagnosticResultCommonProperties(string title,
                                                string description,
                                                DiagnosticSeverity severity,
                                                string category,
                                                string id)
        {
            Title = title;
            Description = description;
            Severity = severity;
            Category = category;
            Id = id;
        }

        public DiagnosticResultCommonProperties(IAnalyzerDetailsReporter analyzer)
        {
            var details = analyzer.GetAnalyzerDetails();
            Severity = details.Severity;
            Title = details.Title.ToString();
            Description = details.Description.ToString();
            Category = details.Category;
            Id = details.DiagnosticId;
        }

        public string Title { get; }

        public string Description { get; }

        public DiagnosticSeverity Severity { get; }

        public string Category { get; }

        public string Id { get; }
    }
}