using System.Collections.Immutable;
using System.Linq;
using Arnolyzer.RuleExceptionAttributes;
using Arnolyzer.SyntacticAnalyzers.Factories;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SuccincT.Options;

namespace Arnolyzer.SyntacticAnalyzers.LiskovSubstitutionPrincipleAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DoNotUseNotImplementedExceptionAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "DoNotUseNotImplementedException";

        private static readonly LocalizableString Title =
            LocalizableStringFactory.LocalizableResourceString(nameof(Resources.DoNotUseNotImplementedExceptionTitle));

        private static readonly LocalizableString MessageFormat =
            LocalizableStringFactory.LocalizableResourceString(nameof(Resources.DoNotUseNotImplementedExceptionMessageFormat));

        private static readonly LocalizableString Description =
            LocalizableStringFactory.LocalizableResourceString(nameof(Resources.DoNotUseNotImplementedExceptionDescription));

        private static readonly DiagnosticDescriptor Rule =
            DiagnosticDescriptorFactory.EnabledByDefaultErrorDescriptor(AnalyzerCategories.EncapsulationAnalyzers,
                                                                        DiagnosticId,
                                                                        Title,
                                                                        MessageFormat,
                                                                        Description);

        

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterCompilationStartAction(
                compileContext =>
                {
                    var notImplementedExceptionName = compileContext.Compilation.GetTypeByMetadataName("System.NotImplementedException");
                    compileContext.RegisterSyntaxNodeAction(
                        symbolContext => AnalyzeCatchDeclarationSyntax(symbolContext, notImplementedExceptionName), 
                        SyntaxKind.ThrowStatement);
                });
        }

        [HasSideEffects]
        private static void AnalyzeCatchDeclarationSyntax(SyntaxNodeAnalysisContext context, 
                                                          INamedTypeSymbol notImplementedExceptionType)
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
                    if (identifierType.Symbol.Equals(notImplementedExceptionType))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Rule, identifier.GetLocation()));
                    }
                })
                .Some().Do(t =>
                {
                    var identifier = t.Parent as IdentifierNameSyntax;
                    var identiferType = context.SemanticModel.GetTypeInfo(identifier).Type;
                    if (identiferType.Equals(notImplementedExceptionType))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Rule, identifier.GetLocation()));
                    }
                })
                .None().Do(() => { })
                .Exec();
        }
    }
}