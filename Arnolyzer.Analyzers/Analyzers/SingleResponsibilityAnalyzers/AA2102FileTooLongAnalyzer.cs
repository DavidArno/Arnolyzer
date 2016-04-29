using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Arnolyzer.Analyzers.SingleResponsibilityAnalyzers
{
    // ReSharper disable once UnusedMember.Global - Used by the documentation generator to document this planned analyzer
    public class AA2102FileTooLongAnalyzer : IPlannedAnalyzerDetailsReporter
    {
        private static readonly IList<Type> SuppressionAttributes = new List<Type>();

        private static readonly AnalyzerDetails AA2101Details =
            new AnalyzerDetails(nameof(AA2102FileTooLongAnalyzer),
                                AnalyzerCategories.SingleResponsibiltyAnalyzers,
                                DefaultState.EnabledByDefault,
                                DiagnosticSeverity.Error,
                                nameof(Resources.AA2102FileTooLongTitle),
                                nameof(Resources.AA2102FileTooLongDescription),
                                nameof(Resources.AA2102FileTooLongMessageFormat),
                                SuppressionAttributes);

        public AnalyzerDetails GetAnalyzerDetails() => AA2101Details;
    }
}