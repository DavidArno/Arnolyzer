using System.Collections.Immutable;
using Arnolyzer.SyntacticAnalyzers.Factories;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using static Arnolyzer.SyntacticAnalyzers.LiskovSubstitutionPrincipleAnalyzers.LSPViolatingExceptionReporter;

namespace Arnolyzer.SyntacticAnalyzers.LiskovSubstitutionPrincipleAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DoNotUseNotSupportedExceptionAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "DoNotUseNotSupportedException";

        private static readonly LocalizableString Title =
            LocalizableStringFactory.LocalizableResourceString(nameof(Resources.DoNotUseNotSupportedExceptionTitle));

        private static readonly LocalizableString MessageFormat =
            LocalizableStringFactory.LocalizableResourceString(nameof(Resources.DoNotUseNotSupportedExceptionMessageFormat));

        private static readonly LocalizableString Description =
            LocalizableStringFactory.LocalizableResourceString(nameof(Resources.DoNotUseNotSupportedExceptionDescription));

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
                    var notSupportedExceptionName = compileContext.Compilation.GetTypeByMetadataName("System.NotSupportedException");
                    compileContext.RegisterSyntaxNodeAction(
                        symbolContext => DetectAndReportLSPViolatingException(symbolContext, notSupportedExceptionName, Rule), 
                        SyntaxKind.ThrowStatement);
                });
        }

    }
}