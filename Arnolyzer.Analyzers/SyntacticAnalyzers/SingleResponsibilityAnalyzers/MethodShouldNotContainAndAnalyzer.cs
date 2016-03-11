using System.Collections.Immutable;
using Arnolyzer.RuleExceptionAttributes;
using Arnolyzer.SyntacticAnalyzers.Factories;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using static Arnolyzer.SyntacticAnalyzers.Factories.DiagnosticDescriptorFactory;

namespace Arnolyzer.SyntacticAnalyzers.SingleResponsibilityAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class MethodShouldNotContainAndAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MethodShouldNotContainAnd";

        private static readonly LocalizableString Title =
            LocalizableStringFactory.LocalizableResourceString(nameof(Resources.MethodShouldNotContainAndTitle));
        private static readonly LocalizableString MessageFormat =
            LocalizableStringFactory.LocalizableResourceString(nameof(Resources.MethodShouldNotContainAndMessageFormat));
        private static readonly LocalizableString Description =
            LocalizableStringFactory.LocalizableResourceString(nameof(Resources.MethodShouldNotContainAndDescription));

        private static readonly DiagnosticDescriptor Rule =
            EnabledByDefaultWarningDescriptor(AnalyzerCategories.SingleResponsibiltyAnalyzers,
                                              DiagnosticId,
                                              Title,
                                              MessageFormat,
                                              Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context) => context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);

        [MutatesParameter]
        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var methodSymbol = (IMethodSymbol)context.Symbol;

            if (methodSymbol.Name.Contains("And"))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, methodSymbol.Locations[0], methodSymbol.Name));
            }
        }
    }
}