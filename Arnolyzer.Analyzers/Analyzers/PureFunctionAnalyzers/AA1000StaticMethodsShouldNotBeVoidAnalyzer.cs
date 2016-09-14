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
    public class AA1000StaticMethodsShouldNotBeVoidAnalyzer : DiagnosticAnalyzer, IAnalyzerDetailsReporter
    {
        private static readonly IList<Type> SuppressionAttributes = new List<Type>
        {
            typeof(HasSideEffectsAttribute),
            typeof(MutatesParameterAttribute)
        };

        private static readonly AnalyzerDetails AA1000Details =
            new AnalyzerDetails(nameof(AA1000StaticMethodsShouldNotBeVoidAnalyzer),
                                AnalyzerCategories.PureFunctionAnalyzers,
                                DefaultState.EnabledByDefault,
                                DiagnosticSeverity.Error,
                                nameof(Resources.AA1000StaticMethodsShouldNotBeVoidTitle),
                                nameof(Resources.AA1000StaticMethodsShouldNotBeVoidDescription),
                                nameof(Resources.AA1000StaticMethodsShouldNotBeVoidMessageFormat),
                                SuppressionAttributes);

        private SettingsHandler _settingsHandler;

        public AnalyzerDetails GetAnalyzerDetails() => AA1000Details;

        private static readonly DiagnosticDescriptor Rule = AA1000Details.GetDiagnosticDescriptor();

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            _settingsHandler = SettingsHandler.CreateHandler();
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }

        [MutatesParameter]
        private void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var methodSymbol = (IMethodSymbol)context.Symbol;

            if (methodSymbol.IsStatic &&
                methodSymbol.ReturnsVoid &&
                methodSymbol.MethodKind != MethodKind.PropertySet &&
                !CommonFunctions.SkipSymbolAnalysis(methodSymbol, _settingsHandler, SuppressionAttributes))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, methodSymbol.Locations[0], methodSymbol.Name));
            }
        }
    }
}