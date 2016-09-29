using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Arnolyzer.Analyzers.Settings;
using Arnolyzer.RuleExceptionAttributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SuccincT.Functional;

namespace Arnolyzer.Analyzers.ImmutabilityAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AA1301VariablesShouldBeAssignedOnceOnlyAnalyzer : DiagnosticAnalyzer,
                                                                   IAnalyzerDetailsReporter,
                                                                   INamedItemSuppresionAttributeDetailsReporter

    {
        private static readonly IList<Type> SuppressionAttributes = new List<Type> {typeof(MutableVariableAttribute)};

        private static readonly AnalyzerDetails AA1301Details =
            new AnalyzerDetails(nameof(AA1301VariablesShouldBeAssignedOnceOnlyAnalyzer),
                                AnalyzerCategories.ImmutabilityAnalyzers,
                                DefaultState.EnabledByDefault,
                                DiagnosticSeverity.Error,
                                nameof(Resources.AA1301VariablesShouldBeAssignedOnceOnlyTitle),
                                nameof(Resources.AA1301VariablesShouldBeAssignedOnceOnlyDescription),
                                nameof(Resources.AA1301VariablesShouldBeAssignedOnceOnlyMessageFormat),
                                SuppressionAttributes);

        private static readonly NamedItemSuppresionAttributeDetails NamedItemSuppressionDetails =
            new NamedItemSuppresionAttributeDetails(nameof(AA1301VariablesShouldBeAssignedOnceOnlyAnalyzer),
                                AnalyzerCategories.ImmutabilityAnalyzers,
                                DefaultState.EnabledByDefault,
                                DiagnosticSeverity.Warning,
                                nameof(Resources.AA1301VariablesShouldBeAssignedOnceOnlySuppresionMisuseTitle),
                                nameof(Resources.AA1301VariablesShouldBeAssignedOnceOnlySuppresionMisuseDescription),
                                nameof(Resources.AA1301VariablesShouldBeAssignedOnceOnlySuppresionMisuseMessageFormat));

        public AnalyzerDetails GetAnalyzerDetails() => AA1301Details;

        public IList<NamedItemSuppresionAttributeDetails> GetNamedItemSuppresionAttributeDetails() =>
            new List<NamedItemSuppresionAttributeDetails> { NamedItemSuppressionDetails };

        private SettingsHandler _settingsHandler;

        private static readonly DiagnosticDescriptor AnalyzerRule = AA1301Details.GetDiagnosticDescriptor();

        private static readonly DiagnosticDescriptor SuppressionMisuseRule =
            NamedItemSuppressionDetails.GetDiagnosticDescriptor();

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => 
            ImmutableArray.Create(AnalyzerRule, SuppressionMisuseRule);

        public override void Initialize(AnalysisContext context)
        {
            _settingsHandler = SettingsHandler.CreateHandler();
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }

        [MutatesParameter]
        private void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var symbol = (IMethodSymbol)context.Symbol;
            if (CommonFunctions.SkipSymbolAnalysisIgnoringAttributes(symbol, _settingsHandler)) return;

            var ignoredVariables = CommonFunctions.ItemsToIgnoreFromAttributes(symbol, SuppressionAttributes);
            var syntax = symbol.DeclaringSyntaxReferences[0].GetSyntaxAsync().Result;

            var identifiers = syntax.DescendantNodes()
                                    .Where(node => node.IsKind(SyntaxKind.VariableDeclarator))
                                    .Cast<VariableDeclaratorSyntax>()
                                    .Select(variable => variable.Identifier.Value.ToString());

            var assignments = syntax.DescendantNodes()
                                    .Where(NodeIsAssignmentExpression)
                                    .Cast<AssignmentExpressionSyntax>()
                                    .Select(expression => new NameAndLocation(
                                                GetIdentifierFromAssignmentExpression(expression),
                                                expression.GetLocation()));

            var prefixExpressions = syntax.DescendantNodes()
                                          .Where(NodeIsPrefixExpression)
                                          .Cast<PrefixUnaryExpressionSyntax>()
                                          .Select(expression => new NameAndLocation(
                                                      GetIdentifierFromPrefixOperand(expression),
                                                      expression.GetLocation()));

            var postfixExpressions = syntax.DescendantNodes()
                                           .Where(NodeIsPostfixExpression)
                                           .Cast<PostfixUnaryExpressionSyntax>()
                                           .Select(expression => new NameAndLocation(
                                                       GetIdentifierFromPostfixOperand(expression),
                                                       expression.GetLocation()));

            foreach (var reassignment in from assignment in assignments.Cons(prefixExpressions).Cons(postfixExpressions)
                                         where ignoredVariables.All(ignored => ignored.Name != assignment.Name) &&
                                               identifiers.Any(i => i == assignment.Name)
                                         select assignment)
            {
                context.ReportDiagnostic(Diagnostic.Create(AnalyzerRule,
                                                           reassignment.Location,
                                                           reassignment.Name));
            }

            foreach (var ignoredSyntaxInfo in from ignoredVariable in ignoredVariables
                                              where identifiers.All(identifier => identifier != ignoredVariable.Name)
                                              select ignoredVariable)
            {
                context.ReportDiagnostic(Diagnostic.Create(SuppressionMisuseRule,
                                                           ignoredSyntaxInfo.Location,
                                                           ignoredSyntaxInfo.Name));

            }
        }

        private static string GetIdentifierFromAssignmentExpression(AssignmentExpressionSyntax expression) =>
            GetIdentifierFromIdentifierNameSyntax((IdentifierNameSyntax) expression.Left);

        private static string GetIdentifierFromPrefixOperand(PrefixUnaryExpressionSyntax expression) =>
            GetIdentifierFromIdentifierNameSyntax((IdentifierNameSyntax)expression.Operand);

        private static string GetIdentifierFromPostfixOperand(PostfixUnaryExpressionSyntax expression) =>
            GetIdentifierFromIdentifierNameSyntax((IdentifierNameSyntax)expression.Operand);

        private static string GetIdentifierFromIdentifierNameSyntax(IdentifierNameSyntax syntax) =>
            syntax.Identifier.Value.ToString();

        private static bool NodeIsAssignmentExpression(SyntaxNode node) =>
            node.IsKind(SyntaxKind.SimpleAssignmentExpression) ||
            node.IsKind(SyntaxKind.AddAssignmentExpression) ||
            node.IsKind(SyntaxKind.SubtractAssignmentExpression) ||
            node.IsKind(SyntaxKind.MultiplyAssignmentExpression) ||
            node.IsKind(SyntaxKind.DivideAssignmentExpression) ||
            node.IsKind(SyntaxKind.ModuloAssignmentExpression) ||
            node.IsKind(SyntaxKind.AndAssignmentExpression) ||
            node.IsKind(SyntaxKind.ExclusiveOrAssignmentExpression) ||
            node.IsKind(SyntaxKind.OrAssignmentExpression) ||
            node.IsKind(SyntaxKind.LeftShiftAssignmentExpression) ||
            node.IsKind(SyntaxKind.RightShiftAssignmentExpression);

        private static bool NodeIsPrefixExpression(SyntaxNode node) =>
            node.IsKind(SyntaxKind.PreIncrementExpression) ||
            node.IsKind(SyntaxKind.PreDecrementExpression);

        private static bool NodeIsPostfixExpression(SyntaxNode node) =>
            node.IsKind(SyntaxKind.PostIncrementExpression) ||
            node.IsKind(SyntaxKind.PostDecrementExpression);
    }
}