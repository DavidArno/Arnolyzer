using SuccincT.Options;

namespace Arnolyzer.Test.DiagnosticVerification
{
    internal class DiagnosticResult
    {
        public DiagnosticResult(DiagnosticResultCommonProperties commonProperties,
                                string message,
                                Option<DiagnosticLocation> location)
        {
            CommonProperties = commonProperties;
            Message = message;
            Location = location;
        }

        public Option<DiagnosticLocation> Location { get; }

        public string Message { get; }

        public DiagnosticResultCommonProperties CommonProperties { get; }
    }
}