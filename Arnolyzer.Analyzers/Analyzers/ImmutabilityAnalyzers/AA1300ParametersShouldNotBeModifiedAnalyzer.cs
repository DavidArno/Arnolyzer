using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Arnolyzer.Analyzers.Settings;
using Arnolyzer.RuleExceptionAttributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using static Arnolyzer.Analyzers.ImmutabilityAnalyzers.VariableMutations;

namespace Arnolyzer.Analyzers.ImmutabilityAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AA1300ParametersShouldNotBeModifiedAnalyzer : DiagnosticAnalyzer,
                                                                   IAnalyzerDetailsReporter,
                                                                   INamedItemSuppresionAttributeDetailsReporter

    {
        private static readonly IList<Type> SuppressionAttributes = new List<Type> { typeof(MutableParameterAttribute) };

        private static readonly AnalyzerDetails AA1300Details =
            new AnalyzerDetails(nameof(AA1300ParametersShouldNotBeModifiedAnalyzer),
                                AnalyzerCategories.ImmutabilityAnalyzers,
                                DefaultState.EnabledByDefault,
                                DiagnosticSeverity.Error,
                                nameof(Resources.AA1300ParametersShouldNotBeModifiedTitle),
                                nameof(Resources.AA1300ParametersShouldNotBeModifiedDescription),
                                nameof(Resources.AA1300ParametersShouldNotBeModifiedMessageFormat),
                                SuppressionAttributes);

        private static readonly NamedItemSuppresionAttributeDetails NamedItemSuppressionDetails =
            new NamedItemSuppresionAttributeDetails(nameof(AA1300ParametersShouldNotBeModifiedAnalyzer),
                                AnalyzerCategories.ImmutabilityAnalyzers,
                                DefaultState.EnabledByDefault,
                                DiagnosticSeverity.Warning,
                                nameof(Resources.AA1300ParametersShouldNotBeModifiedSuppresionMisuseTitle),
                                nameof(Resources.AA1300ParametersShouldNotBeModifiedSuppresionMisuseDescription),
                                nameof(Resources.AA1300ParametersShouldNotBeModifiedSuppresionMisuseMessageFormat));

        public AnalyzerDetails GetAnalyzerDetails() => AA1300Details;

        public IList<NamedItemSuppresionAttributeDetails> GetNamedItemSuppresionAttributeDetails() =>
            new List<NamedItemSuppresionAttributeDetails> { NamedItemSuppressionDetails };

        private SettingsHandler _settingsHandler;

        private static readonly DiagnosticDescriptor AnalyzerRule = AA1300Details.GetDiagnosticDescriptor();

        private static readonly DiagnosticDescriptor SuppressionMisuseRule =
            NamedItemSuppressionDetails.GetDiagnosticDescriptor();

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(AnalyzerRule, SuppressionMisuseRule);

        public override void Initialize(AnalysisContext context)
        {
            _settingsHandler = SettingsHandler.CreateHandler();
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }

        [MutatesParameter]
        private void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var symbol = (IMethodSymbol)context.Symbol;
            if (CommonFunctions.SkipSymbolAnalysisIgnoringAttributes(symbol, _settingsHandler)) return;

            var ignoredVariables = CommonFunctions.ItemsToIgnoreFromAttributes(symbol, SuppressionAttributes).ToList();
            var syntax = symbol.DeclaringSyntaxReferences[0].GetSyntaxAsync().Result;

            var identifiers = symbol.Parameters.Select(p => p.Name).ToList();

            foreach (var reassignment in GetAllNonIgnoredMutations(syntax, ignoredVariables, identifiers))
            {
                context.ReportDiagnostic(Diagnostic.Create(AnalyzerRule,
                                                           reassignment.Location,
                                                           reassignment.Name));
            }

            foreach (var ignoredSyntaxInfo in NonExistantIgnoredVariables(ignoredVariables, identifiers))
            {
                context.ReportDiagnostic(Diagnostic.Create(SuppressionMisuseRule,
                                                           ignoredSyntaxInfo.Location,
                                                           ignoredSyntaxInfo.Name));
            }
        }
    }
}