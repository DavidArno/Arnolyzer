using System.Collections.Immutable;
using System.Linq;
using Arnolyzer.RuleExceptionAttributes;
using Arnolyzer.SyntacticAnalyzers.Factories;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SuccincT.Options;

namespace Arnolyzer.SyntacticAnalyzers.EncapsulationAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ClassPropertyShouldBePubliclyReadOnlyAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "ClassPropertyShouldBePubliclyReadOnly";

        private static readonly LocalizableString Title =
            LocalizableStringFactory.LocalizableResourceString(nameof(Resources.ClassPropertyShouldBePubliclyReadOnlyTitle));

        private static readonly LocalizableString MessageFormat =
            LocalizableStringFactory.LocalizableResourceString(nameof(Resources.ClassPropertyShouldBePubliclyReadOnlyMessageFormat));

        private static readonly LocalizableString Description =
            LocalizableStringFactory.LocalizableResourceString(nameof(Resources.ClassPropertyShouldBePubliclyReadOnlyDescription));

        private static readonly DiagnosticDescriptor Rule =
            DiagnosticDescriptorFactory.EnabledByDefaultErrorDescriptor(AnalyzerCategories.EncapsulationAnalyzers,
                                                                        DiagnosticId,
                                                                        Title,
                                                                        MessageFormat,
                                                                        Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context) => context.RegisterSyntaxTreeAction(AnalyzeSyntaxTree);

        [MutatesParameter]
        private static void AnalyzeSyntaxTree(SyntaxTreeAnalysisContext context)
        {
            var syntaxRoot = context.Tree.GetRoot(context.CancellationToken);
            var classDeclarations =
                syntaxRoot.DescendantNodes(DoNotDescendIntoTypeDeclarations).
                Where(NodeIsPublicClassDeclaration).Cast<ClassDeclarationSyntax>().ToList();

            var propertyDeclarations = from node in classDeclarations
                                       from property in node.DescendantNodes().Where(NodeIsPropertyDeclaration).Cast<PropertyDeclarationSyntax>()
                                       where SyntaxNodeIsPublic(property.Modifiers)
                                       select new
                                       {
                                           className = node.Identifier.Text,
                                           property
                                       };

            foreach (var propertyDeclaration in propertyDeclarations)
            {
                propertyDeclaration.property.DescendantNodes()
                    .Where(p => p.IsKind(SyntaxKind.SetAccessorDeclaration)).Cast<AccessorDeclarationSyntax>()
                    .Where(s => s.Modifiers.Count == 0)
                    .FirstOrNone()
                    .Match()
                    .Some()
                    .Do(setter => context.ReportDiagnostic(
                        Diagnostic.Create(Rule,
                                          setter.Keyword.GetLocation(),
                                          propertyDeclaration.property.Identifier,
                                          propertyDeclaration.className)))
                    .IgnoreElse()
                    .Exec();
            }
        }

        private static bool DoNotDescendIntoTypeDeclarations(SyntaxNode node)
        {
            var kind = node?.Kind();
            return kind != SyntaxKind.ClassDeclaration &&
                   kind != SyntaxKind.StructDeclaration;
        }

        private static bool NodeIsPublicClassDeclaration(SyntaxNode node)
        {
            var kind = node?.Kind();
            return kind == SyntaxKind.ClassDeclaration && SyntaxNodeIsPublic(((BaseTypeDeclarationSyntax)node).Modifiers);
        }

        private static bool NodeIsPropertyDeclaration(SyntaxNode node)
        {
            var kind = node?.Kind();
            return kind == SyntaxKind.PropertyDeclaration;
        }

        private static bool SyntaxNodeIsPublic(SyntaxTokenList modifiers) =>
            modifiers.Count(t => t.Kind() == SyntaxKind.PublicKeyword) > 0;
    }
}