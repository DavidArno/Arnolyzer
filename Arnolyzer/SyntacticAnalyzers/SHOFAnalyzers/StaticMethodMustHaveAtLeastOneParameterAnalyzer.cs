using System.Collections.Immutable;
using Arnolyzer.Factories;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using static Arnolyzer.Factories.LocalizableStringFactory;
using static Arnolyzer.SyntacticAnalyzers.AnalyzerCategories;

namespace Arnolyzer.SHOFAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class StaticMethodMustHaveAtLeastOneParameterAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "StaticMethodMustHaveAtLeastOneParameter";

        private static readonly LocalizableString Title = 
            LocalizableResourceString(nameof(Resources.StaticMethodMustHaveAtLeastOneParameterTitle));
        private static readonly LocalizableString MessageFormat = 
            LocalizableResourceString(nameof(Resources.StaticMethodMustHaveAtLeastOneParameterMessageFormat));
        private static readonly LocalizableString Description = 
            LocalizableResourceString(nameof(Resources.StaticMethodMustHaveAtLeastOneParameterDescription));

        private static readonly DiagnosticDescriptor Rule =
            DiagnosticDescriptorFactory.EnabledByDefaultErrorDescriptor(ShofAnalyzers,
                                                                        DiagnosticId,
                                                                        Title,
                                                                        MessageFormat,
                                                                        Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context) => context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var methodSymbol = (IMethodSymbol)context.Symbol;

            if (methodSymbol.IsStatic && methodSymbol.Parameters.IsEmpty)
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, methodSymbol.Locations[0], methodSymbol.Name));
            }
        }
    }
}
