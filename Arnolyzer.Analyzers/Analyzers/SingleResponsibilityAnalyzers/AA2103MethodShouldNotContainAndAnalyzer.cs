using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Arnolyzer.Analyzers.Settings;
using Arnolyzer.RuleExceptionAttributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using static Arnolyzer.Analyzers.CommonFunctions;

namespace Arnolyzer.Analyzers.SingleResponsibilityAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class AA2103MethodShouldNotContainAndAnalyzer : DiagnosticAnalyzer, IAnalyzerDetailsReporter
    {
        private static readonly IList<Type> SuppressionAttributes = new List<Type> {typeof(HasSingleResponsibility)};

        private static readonly AnalyzerDetails AA2103Details =
            new AnalyzerDetails(nameof(AA2103MethodShouldNotContainAndAnalyzer),
                                AnalyzerCategories.SingleResponsibiltyAnalyzers,
                                DefaultState.EnabledByDefault,
                                DiagnosticSeverity.Warning,
                                nameof(Resources.AA2103MethodShouldNotContainAndTitle),
                                nameof(Resources.AA2103MethodShouldNotContainAndDescription),
                                nameof(Resources.AA2103MethodShouldNotContainAndMessageFormat),
                                SuppressionAttributes);

        private SettingsHandler _settingsHandler;

        public AnalyzerDetails GetAnalyzerDetails() => AA2103Details;

        private static readonly DiagnosticDescriptor Rule = AA2103Details.GetDiagnosticDescriptor();

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            _settingsHandler = SettingsHandler.CreateHandler();
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }

        private void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var methodSymbol = (IMethodSymbol)context.Symbol;
            if (methodSymbol.Name.Contains("And") &&
                !SkipSymbolAnalysis(methodSymbol, _settingsHandler, SuppressionAttributes))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, methodSymbol.Locations[0], methodSymbol.Name));
            }
        }
    }
}