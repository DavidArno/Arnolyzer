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
        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var symbol = (IMethodSymbol)context.Symbol;
            var ignoredVariables = CommonFunctions.ItemsToIgnoreFromAttributes(symbol, SuppressionAttributes);
            var syntax = symbol.DeclaringSyntaxReferences[0].GetSyntaxAsync().Result;

            var identifiers = syntax.DescendantNodes()
                                    .Where(node => node.IsKind(SyntaxKind.VariableDeclarator))
                                    .Cast<VariableDeclaratorSyntax>()
                                    .Select(variable => variable.Identifier.Value.ToString());

            var assignments = syntax.DescendantNodes()
                                    .Where(node => node.IsKind(SyntaxKind.SimpleAssignmentExpression))
                                    .Cast<AssignmentExpressionSyntax>();

            foreach (var reassignment in from assignment in assignments
                                         let identifier =
                                            ((IdentifierNameSyntax) assignment.Left).Identifier.Value.ToString()
                                         where ignoredVariables.All(ignored => ignored.ToString().Trim('"') != identifier) &&
                                               identifiers.Any(i => i == identifier)
                                         select new {Identifier = identifier, Assignment = assignment})
            {
                context.ReportDiagnostic(Diagnostic.Create(AnalyzerRule,
                                                           reassignment.Assignment.GetLocation(),
                                                           reassignment.Identifier));
            }

            foreach (var ignoredSyntaxInfo in from ignoredVariable in ignoredVariables
                                              let ignoredIdentifier = ignoredVariable.ToString().Trim('"')
                                              where identifiers.All(identifier => identifier != ignoredIdentifier)
                                              select new
                                              {
                                                  Location = ignoredVariable.GetLocation(),
                                                  Identifier = ignoredIdentifier
                                              })
            {
                context.ReportDiagnostic(Diagnostic.Create(SuppressionMisuseRule,
                                                           ignoredSyntaxInfo.Location,
                                                           ignoredSyntaxInfo.Identifier));

            }
        }
    }
}