using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Arnolyzer.Analyzers.Settings;
using Arnolyzer.RuleExceptionAttributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Arnolyzer.Analyzers.PureFunctionAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AA1001StaticMethodsShouldHaveAtLeastOneParameterAnalyzer : DiagnosticAnalyzer, IAnalyzerDetailsReporter
    {
        private static readonly IList<Type> SuppressionAttributes = new List<Type>
        {
            typeof(HasSideEffectsAttribute),
            typeof(ConstantValueProviderAttribute)
        };

        private static readonly AnalyzerDetails AA1001Details =
            new AnalyzerDetails(nameof(AA1001StaticMethodsShouldHaveAtLeastOneParameterAnalyzer),
                                AnalyzerCategories.PureFunctionAnalyzers,
                                DefaultState.EnabledByDefault,
                                DiagnosticSeverity.Error,
                                nameof(Resources.AA1001StaticMethodsShouldHaveAtLeastOneParameterTitle),
                                nameof(Resources.AA1001StaticMethodsShouldHaveAtLeastOneParameterDescription),
                                nameof(Resources.AA1001StaticMethodsShouldHaveAtLeastOneParameterMessageFormat),
                                SuppressionAttributes);

        private SettingsHandler _settingsHandler;

        public AnalyzerDetails GetAnalyzerDetails() => AA1001Details;

        private static readonly DiagnosticDescriptor Rule = AA1001Details.GetDiagnosticDescriptor();

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            _settingsHandler = SettingsHandler.CreateHandler();
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }

        [MutatesParameter]
        private void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var methodSymbol = (IMethodSymbol) context.Symbol;

            if (methodSymbol.IsStatic &&
                methodSymbol.Parameters.IsEmpty &&
                methodSymbol.MethodKind != MethodKind.PropertyGet &&
                !CommonFunctions.SkipSymbolAnalysis(methodSymbol, _settingsHandler, SuppressionAttributes))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, methodSymbol.Locations[0], methodSymbol.Name));
            }
        }
    }
}