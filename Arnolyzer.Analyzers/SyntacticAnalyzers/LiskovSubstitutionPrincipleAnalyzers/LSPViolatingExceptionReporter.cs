﻿using System.Linq;
using Arnolyzer.RuleExceptionAttributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SuccincT.Options;
using INamedTypeSymbol = Microsoft.CodeAnalysis.INamedTypeSymbol;

namespace Arnolyzer.SyntacticAnalyzers.LiskovSubstitutionPrincipleAnalyzers
{
    internal static class LSPViolatingExceptionReporter
    {
        [HasSideEffects]
        public static void DetectAndReportLSPViolatingException(SyntaxNodeAnalysisContext context,
                                                                 INamedTypeSymbol exceptionType,
                                                                 DiagnosticDescriptor rule)
        {
            var throwStatement = context.Node as ThrowStatementSyntax;
            throwStatement.Expression.DescendantNodesAndTokens()
                .Where(t => t.IsKind(SyntaxKind.IdentifierName) || t.IsKind(SyntaxKind.IdentifierToken))
                .FirstOrNone()
                .Match()
                .Some().Where(t => t.IsNode).Do(t =>
                {
                    var identifier = t.AsNode() as IdentifierNameSyntax;
                    var identifierType = context.SemanticModel.GetSymbolInfo(identifier);
                    if (identifierType.Symbol.Equals(exceptionType))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(rule, identifier.GetLocation()));
                    }
                })
                .Some().Do(t =>
                {
                    var identifier = t.Parent as IdentifierNameSyntax;
                    var identiferType = context.SemanticModel.GetTypeInfo(identifier).Type;
                    if (identiferType.Equals(exceptionType))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(rule, identifier.GetLocation()));
                    }
                })
                .None().Do(() => { })
                .Exec();
        }
    }
}
