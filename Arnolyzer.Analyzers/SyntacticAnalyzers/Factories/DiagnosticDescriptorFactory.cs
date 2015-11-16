using Microsoft.CodeAnalysis;

namespace Arnolyzer.SyntacticAnalyzers.Factories
{
    internal static class DiagnosticDescriptorFactory
    {
        public static DiagnosticDescriptor EnabledByDefaultErrorDescriptor(string category,
                                                                           string diagnosticId,
                                                                           LocalizableString title,
                                                                           LocalizableString messageFormat,
                                                                           LocalizableString description)
        {
            return new DiagnosticDescriptor(diagnosticId,
                                            title,
                                            messageFormat,
                                            category,
                                            DiagnosticSeverity.Error,
                                            true,
                                            description);
        }
    }
}