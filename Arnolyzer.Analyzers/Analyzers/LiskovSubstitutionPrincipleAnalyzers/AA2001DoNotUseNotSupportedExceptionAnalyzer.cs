using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Arnolyzer.Analyzers.LiskovSubstitutionPrincipleAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AA2001DoNotUseNotSupportedExceptionAnalyzer : DiagnosticAnalyzer, IAnalyzerDetailsReporter
    {
        private static readonly IList<Type> SuppressionAttributes = new List<Type>();

        private static readonly AnalyzerDetails AA2001Details =
            new AnalyzerDetails(nameof(AA2001DoNotUseNotSupportedExceptionAnalyzer),
                                AnalyzerCategories.LiskovSubstitutionAnalyzers,
                                DefaultState.EnabledByDefault,
                                DiagnosticSeverity.Error,
                                nameof(Resources.AA2001DoNotUseNotSupportedExceptionTitle),
                                nameof(Resources.AA2001DoNotUseNotSupportedExceptionDescription),
                                nameof(Resources.AA2001DoNotUseNotSupportedExceptionMessageFormat),
                                SuppressionAttributes);

        public AnalyzerDetails GetAnalyzerDetails() => AA2001Details;

        private static readonly DiagnosticDescriptor Rule = AA2001Details.GetDiagnosticDescriptor();

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterCompilationStartAction(
                compileContext =>
                {
                    var notSupportedExceptionName = compileContext.Compilation.GetTypeByMetadataName("System.NotSupportedException");
                    compileContext.RegisterSyntaxNodeAction(
                        symbolContext => LSPViolatingExceptionReporter.DetectAndReportLSPViolatingException(symbolContext, notSupportedExceptionName, Rule), 
                        SyntaxKind.ThrowStatement);
                });
        }
    }
}