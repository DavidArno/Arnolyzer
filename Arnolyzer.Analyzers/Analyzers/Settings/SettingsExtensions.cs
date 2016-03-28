using Arnolyzer.RuleExceptionAttributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Arnolyzer.Analyzers.Settings
{
    internal static class SettingsExtensions
    {
        private static SettingsHandler _instance;

        [HasSideEffects]
        public static void InitialiseArnolyzerSettings(this AnalyzerOptions options)
        {
            if (_instance == null)
            {
                _instance = new SettingsHandler("arnolyzer.yaml", "ARNOLYZER_HOME", options.AdditionalFiles);
            }
        }

        public static SettingsDetails ArnolyzerSettings(this SyntaxTree symbol) =>
            _instance.GetArnolyzerSettingsForProject(symbol.FilePath);
    }
}