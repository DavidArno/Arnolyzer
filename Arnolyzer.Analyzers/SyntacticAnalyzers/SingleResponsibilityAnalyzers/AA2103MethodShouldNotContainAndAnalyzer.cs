using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Arnolyzer.RuleExceptionAttributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using static Arnolyzer.SyntacticAnalyzers.DefaultState;
using static Microsoft.CodeAnalysis.DiagnosticSeverity;

namespace Arnolyzer.SyntacticAnalyzers.SingleResponsibilityAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class AA2103MethodShouldNotContainAndAnalyzer : DiagnosticAnalyzer, IAnalyzerDetailsReporter
    {
        private static readonly IList<Type> SuppressionAttributes = new List<Type>();

        private static readonly AnalyzerDetails AA2103Details =
            new AnalyzerDetails(nameof(AA2103MethodShouldNotContainAndAnalyzer),
                                AnalyzerCategories.SingleResponsibiltyAnalyzers,
                                EnabledByDefault,
                                Warning,
                                nameof(Resources.AA2103MethodShouldNotContainAndTitle),
                                nameof(Resources.AA2103MethodShouldNotContainAndDescription),
                                nameof(Resources.AA2103MethodShouldNotContainAndMessageFormat),
                                SuppressionAttributes);

        public AnalyzerDetails GetAnalyzerDetails() => AA2103Details;

        private static readonly DiagnosticDescriptor Rule = AA2103Details.GetDiagnosticDescriptor();

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