using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Arnolyzer.RuleExceptionAttributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using static Arnolyzer.SyntacticAnalyzers.DefaultState;
using static Microsoft.CodeAnalysis.DiagnosticSeverity;

namespace Arnolyzer.SyntacticAnalyzers.SingleResponsibilityAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AA2104FileMustOnlyContainOneTypeDefinitionAnalyzer : DiagnosticAnalyzer, IAnalyzerDetailsReporter
    {
        private static readonly IList<Type> SuppressionAttributes = new List<Type>();

        private static readonly AnalyzerDetails AA2104Details =
            new AnalyzerDetails(nameof(AA2104FileMustOnlyContainOneTypeDefinitionAnalyzer),
                                AnalyzerCategories.SingleResponsibiltyAnalyzers,
                                EnabledByDefault,
                                Error,
                                nameof(Resources.AA2104FileMustOnlyContainOneTypeDefinitionTitle),
                                nameof(Resources.AA2104FileMustOnlyContainOneTypeDefinitionDescription),
                                nameof(Resources.AA2104FileMustOnlyContainOneTypeDefinitionMessageFormat),
                                SuppressionAttributes);

        public AnalyzerDetails GetAnalyzerDetails() => AA2104Details;

        private static readonly DiagnosticDescriptor Rule = AA2104Details.GetDiagnosticDescriptor();

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context) => context.RegisterSyntaxTreeAction(AnalyzeSyntaxTree);

        [MutatesParameter]
        private static void AnalyzeSyntaxTree(SyntaxTreeAnalysisContext context)
        {
            var syntaxRoot = context.Tree.GetRoot(context.CancellationToken);
            var typeDeclarations = syntaxRoot.DescendantNodes(IgnoreNodesInsideClassDeclarations).Where(NodeIsTypeDeclaration).ToList();

            if (typeDeclarations.Count <= 1) { return; }

            foreach (var node in typeDeclarations)
            {
                var identifier = ((BaseTypeDeclarationSyntax)node).Identifier;
                context.ReportDiagnostic(Diagnostic.Create(Rule, identifier.GetLocation(), identifier.Text));
            }
        }

        private static bool IgnoreNodesInsideClassDeclarations(SyntaxNode node)
        {
            var kind = node?.Kind();
            return kind != SyntaxKind.ClassDeclaration &&
                   kind != SyntaxKind.StructDeclaration;
        }

        private static bool NodeIsTypeDeclaration(SyntaxNode node)
        {
            var kind = node?.Kind();
            return kind == SyntaxKind.ClassDeclaration ||
                   kind == SyntaxKind.InterfaceDeclaration ||
                   kind == SyntaxKind.StructDeclaration ||
                   kind == SyntaxKind.EnumDeclaration;
        }
    }
}