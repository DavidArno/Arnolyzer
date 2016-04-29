using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Arnolyzer.Analyzers.ImmutabilityAnalyzers
{
    // ReSharper disable once UnusedMember.Global - Used by the documentation generator to document this planned analyzer
    public class AA1301VariablesShouldBeAssignedOnceOnlyAnalyzer : IPlannedAnalyzerDetailsReporter
    {
        private static readonly IList<Type> SuppressionAttributes = new List<Type>();

        private static readonly AnalyzerDetails AA1301Details =
            new AnalyzerDetails(nameof(AA1301VariablesShouldBeAssignedOnceOnlyAnalyzer),
                                AnalyzerCategories.ImmutabilityAnalyzers,
                                DefaultState.EnabledByDefault,
                                DiagnosticSeverity.Error,
                                nameof(Resources.AA1301VariablesShouldBeAssignedOnceOnlyTitle),
                                nameof(Resources.AA1301VariablesShouldBeAssignedOnceOnlyDescription),
                                nameof(Resources.AA1301VariablesShouldBeAssignedOnceOnlyMessageFormat),
                                SuppressionAttributes);

        public AnalyzerDetails GetAnalyzerDetails() => AA1301Details;
    }
}