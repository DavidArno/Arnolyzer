using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Arnolyzer.RuleExceptionAttributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Arnolyzer.Analyzers.SingleResponsibilityAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class AA2100MethodParametersMustNotBeRefOrOutAnalyzer : DiagnosticAnalyzer, IAnalyzerDetailsReporter
    {
        private static readonly IList<Type> SuppressionAttributes = new List<Type>();

        private static readonly AnalyzerDetails AA2100Details =
            new AnalyzerDetails(nameof(AA2100MethodParametersMustNotBeRefOrOutAnalyzer),
                                AnalyzerCategories.SingleResponsibiltyAnalyzers,
                                DefaultState.EnabledByDefault,
                                DiagnosticSeverity.Error,
                                nameof(Resources.AA2100MethodParametersMustNotBeRefOrOutTitle),
                                nameof(Resources.AA2100MethodParametersMustNotBeRefOrOutDescription),
                                nameof(Resources.AA2100MethodParametersMustNotBeRefOrOutMessageFormat),
                                SuppressionAttributes);

        public AnalyzerDetails GetAnalyzerDetails() => AA2100Details;

        private static readonly DiagnosticDescriptor Rule = AA2100Details.GetDiagnosticDescriptor();

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
            => context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);

        [MutatesParameter]
        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var methodSymbol = (IMethodSymbol) context.Symbol;

            foreach (var parameter in methodSymbol.Parameters.Where(parameter => parameter.RefKind != RefKind.None))
            {
                var syntax = parameter.DeclaringSyntaxReferences[0].GetSyntax() as ParameterSyntax;

                context.ReportDiagnostic(Diagnostic.Create(Rule,
                                                           syntax.GetLocation(),
                                                           parameter.Name,
                                                           methodSymbol.Name,
                                                           RefOrOut(parameter.RefKind)));
            }
        }

        private static string RefOrOut(RefKind refKind) => refKind == RefKind.Ref ? "ref" : "out";
    }
}