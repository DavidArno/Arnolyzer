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
    public class AA1103ClassPropertiesMustBePubliclyRead_OnlyAnalyzer : DiagnosticAnalyzer, IAnalyzerDetailsReporter
    {
        private static readonly IList<Type> SuppressionAttributes = new List<Type> { typeof(MutablePropertyAttribute) };

        private static readonly AnalyzerDetails AA1103Details =
            new AnalyzerDetails(nameof(AA1103ClassPropertiesMustBePubliclyRead_OnlyAnalyzer),
                                AnalyzerCategories.EncapsulationAndImmutabilityAnalyzers,
                                DefaultState.EnabledByDefault,
                                DiagnosticSeverity.Error, 
                                nameof(Resources.AA1103ClassPropertiesMustBePubliclyReadOnlyTitle),
                                nameof(Resources.AA1103ClassPropertiesMustBePubliclyReadOnlyDescription),
                                nameof(Resources.AA1103ClassPropertiesMustBePubliclyReadOnlyMessageFormat),
                                SuppressionAttributes);

        public AnalyzerDetails GetAnalyzerDetails() => AA1103Details;

        private static readonly DiagnosticDescriptor Rule = AA1103Details.GetDiagnosticDescriptor();

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context) => context.RegisterSyntaxTreeAction(AnalyzeSyntaxTree);

        [MutatesParameter]
        private static void AnalyzeSyntaxTree(SyntaxTreeAnalysisContext context)
        {
            var syntaxRoot = context.Tree.GetRoot(context.CancellationToken);
            var classDeclarations =
                syntaxRoot.DescendantNodes(DoNotDescendIntoTypeDeclarations).
                Where(NodeIsPublicClassDeclaration)
                .Cast<ClassDeclarationSyntax>().ToList();

            var propertyDeclarations = from node in classDeclarations
                                       from property in node.DescendantNodes().Where(NodeIsPropertyDeclaration).Cast<PropertyDeclarationSyntax>()
                                       where SyntaxNodeIsPublic(property.Modifiers) && 
                                             !CommonFunctions.PropertyHasIgnoreRuleAttribute(property, SuppressionAttributes)
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