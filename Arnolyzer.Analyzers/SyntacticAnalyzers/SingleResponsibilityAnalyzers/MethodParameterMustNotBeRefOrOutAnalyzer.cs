using System.Collections.Immutable;
using System.Linq;
using Arnolyzer.RuleExceptionAttributes;
using Arnolyzer.SyntacticAnalyzers.Factories;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Arnolyzer.SyntacticAnalyzers.SingleResponsibilityAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MethodParameterMustNotBeRefOrOutAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MethodParameterMustNotBeRefOrOut";

        private static readonly LocalizableString Title =
            LocalizableStringFactory.LocalizableResourceString(nameof(Resources.MethodParameterMustNotBeRefOrOutTitle));
        private static readonly LocalizableString MessageFormat =
            LocalizableStringFactory.LocalizableResourceString(nameof(Resources.MethodParameterMustNotBeRefOrOutMessageFormat));
        private static readonly LocalizableString Description =
            LocalizableStringFactory.LocalizableResourceString(nameof(Resources.MethodParameterMustNotBeRefOrOutDescription));

        private static readonly DiagnosticDescriptor Rule =
            DiagnosticDescriptorFactory.EnabledByDefaultErrorDescriptor(AnalyzerCategories.SingleResponsibiltyAnalyzers,
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

            foreach (var parameter in methodSymbol.Parameters.Where(parameter => parameter.RefKind != RefKind.None))
            {
                var syntax = parameter.DeclaringSyntaxReferences[0].GetSyntax() as ParameterSyntax;

                context.ReportDiagnostic(Diagnostic.Create(Rule, syntax.GetLocation(), parameter.Name, methodSymbol.Name, RefOrOut(parameter.RefKind)));
            }
        }

        private static string RefOrOut(RefKind refKind) => refKind == RefKind.Ref ? "ref" : "out";
    }
}