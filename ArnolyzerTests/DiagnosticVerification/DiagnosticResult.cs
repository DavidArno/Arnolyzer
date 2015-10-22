using Microsoft.CodeAnalysis;
using SuccincT.Options;

namespace Arnolyzer.Test.DiagnosticVerification
{
    public struct DiagnosticResult
    {
        public DiagnosticResult(Option<DiagnosticResultLocation> location, 
                                DiagnosticSeverity severity, 
                                string id)
        {

            Severity = severity;
            Id = id;
            Location = location;
        }

        public DiagnosticSeverity Severity { get; }

        public string Id { get; }

        public Option<DiagnosticResultLocation> Location { get; }
    }
}