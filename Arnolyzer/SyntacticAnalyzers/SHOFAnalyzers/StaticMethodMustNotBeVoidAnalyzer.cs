using System.Collections.Immutable;
using Arnolyzer.Factories;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using static Arnolyzer.Factories.LocalizableStringFactory;

namespace Arnolyzer.SyntacticAnalyzers.SHOFAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class StaticMethodMustNotBeVoidAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "StaticMethodMustNotBeVoid";

        private static readonly LocalizableString Title =
            LocalizableResourceString(nameof(Resources.StaticMethodMustNotBeVoidTitle));
        private static readonly LocalizableString MessageFormat =
            LocalizableResourceString(nameof(Resources.StaticMethodMustNotBeVoidMessageFormat));
        private static readonly LocalizableString Description =
            LocalizableResourceString(nameof(Resources.StaticMethodMustNotBeVoidDescription));

        private static readonly DiagnosticDescriptor Rule =
            DiagnosticDescriptorFactory.EnabledByDefaultErrorDescriptor(AnalyzerCategories.ShofAnalyzers,
                                                                        DiagnosticId,
                                                                        Title,
                                                                        MessageFormat,
                                                                        Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context) =>
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var methodSymbol = (IMethodSymbol)context.Symbol;

            if (methodSymbol.IsStatic && methodSymbol.ReturnsVoid)
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, methodSymbol.Locations[0], methodSymbol.Name));
            }
        }
    }
}