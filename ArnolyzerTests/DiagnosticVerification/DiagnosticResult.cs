using Microsoft.CodeAnalysis;
using SuccincT.Options;

namespace Arnolyzer.Test.DiagnosticVerification
{
    public struct DiagnosticResult
    {
        public DiagnosticResult(Option<DiagnosticResultLocation> location, 
                                DiagnosticSeverity severity, 
                                string category,
                                string id)
        {
            Location = location;
            Severity = severity;
            Category = category;
            Id = id;
        }

        public DiagnosticSeverity Severity { get; }

        public string Category { get; }

        public string Id { get; }

        public Option<DiagnosticResultLocation> Location { get; }
    }
}