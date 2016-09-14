using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Arnolyzer.Analyzers.Settings;
using Arnolyzer.RuleExceptionAttributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using static Arnolyzer.Analyzers.CommonFunctions;

namespace Arnolyzer.Analyzers.GlobalStateAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AA1201AvoidUsingStaticPropertiesAnalyzer : DiagnosticAnalyzer, IAnalyzerDetailsReporter
    {
        private static readonly IList<Type> SuppressionAttributes = new List<Type> { typeof(GlobalStateAttribute) };

        private static readonly AnalyzerDetails AA2101Details =
            new AnalyzerDetails(nameof(AA1201AvoidUsingStaticPropertiesAnalyzer),
                                AnalyzerCategories.GlobalStateAnalyzers,
                                DefaultState.EnabledByDefault,
                                DiagnosticSeverity.Error,
                                nameof(Resources.AA1201AvoidUsingStaticPropertiesTitle),
                                nameof(Resources.AA1201AvoidUsingStaticPropertiesDescription),
                                nameof(Resources.AA1201AvoidUsingStaticPropertiesMessageFormat),
                                SuppressionAttributes);

        public AnalyzerDetails GetAnalyzerDetails() => AA2101Details;
        private SettingsHandler _settingsHandler;
        private static readonly DiagnosticDescriptor Rule = AA2101Details.GetDiagnosticDescriptor();

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            _settingsHandler = SettingsHandler.CreateHandler();
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Property);
        }

        [MutatesParameter]
        private void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var symbol = (IPropertySymbol)context.Symbol;
            if (symbol.IsStatic &&
                !symbol.IsReadOnly &&
                !SkipSymbolAnalysis(symbol, _settingsHandler, SuppressionAttributes))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule,
                                                           symbol.Locations[0],
                                                           symbol.Name,
                                                           symbol.ContainingType.Name));
            }
        }
    }
}