using System.Collections.Immutable;
using System.Linq;
using Arnolyzer.Factories;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Arnolyzer.SyntacticAnalyzers.SingleResponsibilityAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MethodParameterMustNotBeRefOrOutAnalyzer : DiagnosticAnalyzer
    {
        private const string DiagnosticId = "MethodParameterMustNotBeRefOrOut";

        private static readonly LocalizableString Title = 
            LocalizableStringFactory.LocalizableResourceString(nameof(Resources.MethodParameterMustNotBeRefOrOutTitle));
        private static readonly LocalizableString MessageFormat = 
            LocalizableStringFactory.LocalizableResourceString(nameof(Resources.MethodParameterMustNotBeRefOrOutMessageFormat));
        private static readonly LocalizableString Description = 
            LocalizableStringFactory.LocalizableResourceString(nameof(Resources.MethodParameterMustNotBeRefOrOutDescription));
        private const string Category = "Single Responsibilty Analyzers";

        private static readonly DiagnosticDescriptor Rule = 
            DiagnosticDescriptorFactory.EnabledByDefaultErrorDescriptor(Category, DiagnosticId, Title, MessageFormat, Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context) => context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var methodSymbol = (IMethodSymbol)context.Symbol;

            foreach (var parameter in methodSymbol.Parameters.Where(parameter => parameter.RefKind != RefKind.None))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, parameter.Locations[0], parameter.Name, methodSymbol.Name, RefOrOut(parameter.RefKind)));
            }
        }

        private static string RefOrOut(RefKind refKind) => refKind == RefKind.Ref ? "ref" : "out";
    }
}
