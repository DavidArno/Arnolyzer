using System.Collections.Immutable;
using System.Linq;
using Arnolyzer.RuleExceptionAttributes;
using Arnolyzer.SyntacticAnalyzers.Factories;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Arnolyzer.SyntacticAnalyzers.EncapsulationAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class InnerTypesMustBePrivateAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "InnerTypesMustBePrivate";

        private static readonly LocalizableString Title =
            LocalizableStringFactory.LocalizableResourceString(nameof(Resources.InnerTypesMustBePrivateTitle));

        private static readonly LocalizableString MessageFormat =
            LocalizableStringFactory.LocalizableResourceString(nameof(Resources.InnerTypesMustBePrivateMessageFormat));

        private static readonly LocalizableString Description =
            LocalizableStringFactory.LocalizableResourceString(nameof(Resources.InnerTypesMustBePrivateDescription));

        private static readonly DiagnosticDescriptor Rule =
            DiagnosticDescriptorFactory.EnabledByDefaultErrorDescriptor(AnalyzerCategories.EncapsulationAnalyzers,
                                                                        DiagnosticId,
                                                                        Title,
                                                                        MessageFormat,
                                                                        Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
            => context.RegisterSyntaxTreeAction(AnalyzeSyntaxTree);

        [MutatesParameter]
        private static void AnalyzeSyntaxTree(SyntaxTreeAnalysisContext context)
        {
            var syntaxRoot = context.Tree.GetRoot(context.CancellationToken);
            var outerTypeDeclarations =
                syntaxRoot.DescendantNodes(DoNotDescendIntoTypeDeclarations).Where(NodeIsTypeDeclaration).ToList();

            var innerTypeDeclarations = (from node in outerTypeDeclarations
                                         from innerNode in node.DescendantNodes().Where(NodeIsTypeDeclaration)
                                         select innerNode).Cast<BaseTypeDeclarationSyntax>();

            foreach (var typeDeclaration in innerTypeDeclarations)
            {
                if (typeDeclaration.Modifiers.Count(t => t.Kind() == SyntaxKind.InternalKeyword ||
                                                         t.Kind() == SyntaxKind.PublicKeyword) > 0)
                {
                    var identifier = typeDeclaration.Identifier;
                    context.ReportDiagnostic(Diagnostic.Create(Rule, identifier.GetLocation(), identifier.Text));
                }
            }
        }

        private static bool DoNotDescendIntoTypeDeclarations(SyntaxNode node)
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