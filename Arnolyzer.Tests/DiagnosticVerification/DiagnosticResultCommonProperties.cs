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

        public DiagnosticResultCommonProperties(AnalyzerDetails analyzerDetails, DiagnosticSeverity severity)
        {
            Severity = severity;
            Title = analyzerDetails.Title.ToString();
            Description = analyzerDetails.Description.ToString();
            Category = analyzerDetails.Category;
            Id = analyzerDetails.DiagnosticId;
        }

        public string Title { get; }

        public string Description { get; }

        public DiagnosticSeverity Severity { get; }

        public string Category { get; }

        public string Id { get; }
    }
}