using System.Collections.Immutable;
using System.Linq;
using Arnolyzer.Factories;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using static Arnolyzer.Factories.LocalizableStringFactory;

namespace Arnolyzer.SyntacticAnalyzers.EncapsulationAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class InterfacePropertiesShouldBeReadonlyAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "InterfacePropertiesShouldBeReadonly";

        private static readonly LocalizableString Title =
            LocalizableResourceString(nameof(Resources.InterfacePropertiesShouldBeReadonlyTitle));

        private static readonly LocalizableString MessageFormat =
            LocalizableResourceString(nameof(Resources.InterfacePropertiesShouldBeReadonlyMessageFormat));

        private static readonly LocalizableString Description =
            LocalizableResourceString(nameof(Resources.InterfacePropertiesShouldBeReadonlyDescription));

        private static readonly DiagnosticDescriptor Rule =
            DiagnosticDescriptorFactory.EnabledByDefaultErrorDescriptor(AnalyzerCategories.EncapsulationAnalyzers,
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
            var interfaceDeclarations =
                syntaxRoot.DescendantNodes(DoNotDescendIntoTypeDeclarations).Where(NodeIsInterfaceDeclaration).ToList();

            var propertyDeclarations = from node in interfaceDeclarations
                                       from property in node.DescendantNodes().Where(NodeIsPropertyDeclaration)
                                       let interfaceNode = node as InterfaceDeclarationSyntax
                                       select new
                                       {
                                           interfaceName = interfaceNode.Identifier.Text,
                                           property = property as PropertyDeclarationSyntax
                                       };

            foreach (var propertyDeclaration in propertyDeclarations)
            {
                var setterList = propertyDeclaration.property.DescendantNodesAndTokens()
                                                    .Where(n => n.IsKind(SyntaxKind.SetKeyword))
                                                    .ToList();

                if (setterList.Any())
                {
                    var setter = setterList[0].AsToken();
                    context.ReportDiagnostic(Diagnostic.Create(Rule,
                                                               setter.GetLocation(),
                                                               propertyDeclaration.property.Identifier,
                                                               propertyDeclaration.interfaceName));
                }
            }
        }

        private static bool DoNotDescendIntoTypeDeclarations(SyntaxNode node)
        {
            var kind = node?.Kind();
            return kind != SyntaxKind.ClassDeclaration &&
                   kind != SyntaxKind.StructDeclaration;
        }

        private static bool NodeIsInterfaceDeclaration(SyntaxNode node)
        {
            var kind = node?.Kind();
            return kind == SyntaxKind.InterfaceDeclaration;
        }

        private static bool NodeIsPropertyDeclaration(SyntaxNode node)
        {
            var kind = node?.Kind();
            return kind == SyntaxKind.PropertyDeclaration;
        }
    }
}