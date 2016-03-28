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

namespace Arnolyzer.Analyzers.EncapsulationAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AA1102InterfacePropertiesMustBeRead_OnlyAnalyzer : DiagnosticAnalyzer, IAnalyzerDetailsReporter
    {
        private static readonly IList<Type> SuppressionAttributes = new List<Type> { typeof(MutablePropertyAttribute) };

        private static readonly AnalyzerDetails AA1102Details =
            new AnalyzerDetails(nameof(AA1102InterfacePropertiesMustBeRead_OnlyAnalyzer),
                                AnalyzerCategories.EncapsulationAndImmutabilityAnalyzers,
                                DefaultState.EnabledByDefault,
                                DiagnosticSeverity.Error,
                                nameof(Resources.AA1102InterfacePropertiesMustBeReadOnlyTitle),
                                nameof(Resources.AA1102InterfacePropertiesMustBeReadOnlyDescription),
                                nameof(Resources.AA1102InterfacePropertiesMustBeReadOnlyMessageFormat),
                                SuppressionAttributes);

        public AnalyzerDetails GetAnalyzerDetails() => AA1102Details;

        private static readonly DiagnosticDescriptor Rule = AA1102Details.GetDiagnosticDescriptor();

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
                                       from property in node.DescendantNodes().Where(NodeIsPropertyDeclaration).Cast<PropertyDeclarationSyntax>()
                                       where !CommonFunctions.PropertyHasIgnoreRuleAttribute(property, SuppressionAttributes)
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
                                   .FirstOrNone()
                                   .Match()
                                   .Some()
                                   .Do(setter => context.ReportDiagnostic(
                                       Diagnostic.Create(Rule,
                                                         setter.AsToken().GetLocation(),
                                                         propertyDeclaration.property.Identifier,
                                                         propertyDeclaration.interfaceName)))
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