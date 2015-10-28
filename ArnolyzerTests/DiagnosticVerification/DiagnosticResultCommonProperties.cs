using Microsoft.CodeAnalysis;

namespace Arnolyzer.Test.DiagnosticVerification
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

        public string Title { get; }

        public string Description { get; }

        public DiagnosticSeverity Severity { get; }

        public string Category { get; }

        public string Id { get; }
    }
}