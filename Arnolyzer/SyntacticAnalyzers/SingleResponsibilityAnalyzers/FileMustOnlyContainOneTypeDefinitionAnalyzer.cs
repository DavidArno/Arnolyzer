using System.Collections.Immutable;
using System.Linq;
using Arnolyzer.Factories;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Arnolyzer.SyntacticAnalyzers.SingleResponsibilityAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class FileMustOnlyContainOneTypeDefinitionAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "FileMustOnlyContainOneTypeDefinition";

        private static readonly LocalizableString Title =
            LocalizableStringFactory.LocalizableResourceString(nameof(Resources.FileMustOnlyContainOneTypeDefinitionTitle));

        private static readonly LocalizableString MessageFormat =
            LocalizableStringFactory.LocalizableResourceString(
                nameof(Resources.FileMustOnlyContainOneTypeDefinitionMessageFormat));

        private static readonly LocalizableString Description =
            LocalizableStringFactory.LocalizableResourceString(
                nameof(Resources.FileMustOnlyContainOneTypeDefinitionDescription));

        private static readonly DiagnosticDescriptor Rule =
            DiagnosticDescriptorFactory.EnabledByDefaultErrorDescriptor(AnalyzerCategories.SingleResponsibiltyAnalyzers, 
                                                                        DiagnosticId, 
                                                                        Title, 
                                                                        MessageFormat,
                                                                        Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
            => context.RegisterSyntaxTreeAction(AnalyzeSyntaxTree);

        private static void AnalyzeSyntaxTree(SyntaxTreeAnalysisContext context)
        {
            var syntaxRoot = context.Tree.GetRoot(context.CancellationToken);
            var typeDeclarations = syntaxRoot.DescendantNodes(IgnoreNodesInsideClassDeclarations).Where(NodeIsTypeDeclaration).ToList();

            if (typeDeclarations.Count() <= 1) {  return; }

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
