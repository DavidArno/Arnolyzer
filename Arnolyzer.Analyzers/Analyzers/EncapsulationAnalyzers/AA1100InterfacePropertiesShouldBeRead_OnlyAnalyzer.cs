using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Arnolyzer.RuleExceptionAttributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SuccincT.Options;
using static Arnolyzer.Analyzers.CommonFunctions;

namespace Arnolyzer.Analyzers.EncapsulationAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AA1100InterfacePropertiesShouldBeRead_OnlyAnalyzer : DiagnosticAnalyzer, IAnalyzerDetailsReporter
    {
        private static readonly IList<Type> SuppressionAttributes = new List<Type> {typeof(MutablePropertyAttribute)};

        private static readonly AnalyzerDetails AA1100Details =
            new AnalyzerDetails(nameof(AA1100InterfacePropertiesShouldBeRead_OnlyAnalyzer),
                                AnalyzerCategories.EncapsulationAnalyzers,
                                DefaultState.EnabledByDefault,
                                DiagnosticSeverity.Error,
                                nameof(Resources.AA1100InterfacePropertiesShouldBeReadOnlyTitle),
                                nameof(Resources.AA1100InterfacePropertiesShouldBeReadOnlyDescription),
                                nameof(Resources.AA1100InterfacePropertiesShouldBeReadOnlyMessageFormat),
                                SuppressionAttributes);

        public AnalyzerDetails GetAnalyzerDetails() => AA1100Details;

        private static readonly DiagnosticDescriptor Rule = AA1100Details.GetDiagnosticDescriptor();

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context) =>
            context.RegisterSyntaxTreeAction(AnalyzeSyntaxTree);

        [HasSideEffects]
        private static void AnalyzeSyntaxTree(SyntaxTreeAnalysisContext context)
        {
            var syntaxRoot = context.Tree.GetRoot(context.CancellationToken);
            var interfaceDeclarations =
                syntaxRoot.DescendantNodes(DoNotDescendIntoTypeDeclarations).Where(NodeIsInterfaceDeclaration).ToList();

            var propertyDeclarations = from node in interfaceDeclarations
                                       from property in node.DescendantNodes()
                                                            .Where(NodeIsPropertyDeclaration)
                                                            .Cast<PropertyDeclarationSyntax>()
                                       where !PropertyHasIgnoreRuleAttribute(property,
                                                                             SuppressionAttributes)
                                       let interfaceNode = node as InterfaceDeclarationSyntax
                                       select new
                                       {
                                           interfaceName = interfaceNode.Identifier.Text,
                                           property
                                       };

            foreach (var propertyDeclaration in propertyDeclarations)
            {
                propertyDeclaration.property.DescendantNodesAndTokens()
                                   .Where(n => n.IsKind(SyntaxKind.SetKeyword))
                                   .TryFirst()
                                   .Match()
                                   .Some()
                                   .Do(setter => context.ReportDiagnostic(Diagnostic.Create(Rule,
                                                                                            setter.AsToken()
                                                                                                  .GetLocation(),
                                                                                            propertyDeclaration.property
                                                                                                               .Identifier,
                                                                                            propertyDeclaration
                                                                                                .interfaceName)))
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