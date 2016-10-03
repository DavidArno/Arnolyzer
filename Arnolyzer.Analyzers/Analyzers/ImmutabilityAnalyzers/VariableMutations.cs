using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SuccincT.Functional;

namespace Arnolyzer.Analyzers.ImmutabilityAnalyzers
{
    internal static class VariableMutations
    {
        public static IEnumerable<NameAndLocation> GetAllNonIgnoredMutations(
            SyntaxNode syntax,
            IEnumerable<NameAndLocation> ignoredVariables,
            IEnumerable<string> identifiers)
        {
            return from assignment in GetAllMutations(syntax)
                   where ignoredVariables.All(ignored => ignored.Name != assignment.Name) &&
                         identifiers.Any(i => i == assignment.Name)
                   select assignment;
        }

        public static IEnumerable<NameAndLocation> NonExistantIgnoredVariables(
            IEnumerable<NameAndLocation> ignoredVariables,
            IEnumerable<string> identifiers)
        {
            return from ignoredVariable in ignoredVariables
                   where identifiers.All(identifier => identifier != ignoredVariable.Name)
                   select ignoredVariable;
        }

        private static IEnumerable<NameAndLocation> GetAllMutations(SyntaxNode syntax)
        {
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

            return assignments.Cons(prefixExpressions).Cons(postfixExpressions);
        }

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

        private static string GetIdentifierFromAssignmentExpression(AssignmentExpressionSyntax expression) =>
            GetIdentifierFromIdentifierNameSyntax((IdentifierNameSyntax)expression.Left);

        private static string GetIdentifierFromPrefixOperand(PrefixUnaryExpressionSyntax expression) =>
            GetIdentifierFromIdentifierNameSyntax((IdentifierNameSyntax)expression.Operand);

        private static string GetIdentifierFromPostfixOperand(PostfixUnaryExpressionSyntax expression) =>
            GetIdentifierFromIdentifierNameSyntax((IdentifierNameSyntax)expression.Operand);

        private static string GetIdentifierFromIdentifierNameSyntax(IdentifierNameSyntax syntax) =>
            syntax.Identifier.Value.ToString();
    }
}
