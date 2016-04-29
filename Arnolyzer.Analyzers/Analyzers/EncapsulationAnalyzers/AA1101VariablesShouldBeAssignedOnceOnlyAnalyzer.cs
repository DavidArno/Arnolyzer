using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Arnolyzer.Analyzers.EncapsulationAnalyzers
{
    // ReSharper disable once UnusedMember.Global - Used by the documentation generator to document this planned analyzer
    public class AA1101VariablesShouldBeAssignedOnceOnlyAnalyzer : IPlannedAnalyzerDetailsReporter
    {
        private static readonly IList<Type> SuppressionAttributes = new List<Type>();

        private static readonly AnalyzerDetails AA1001Details =
            new AnalyzerDetails(nameof(AA1101VariablesShouldBeAssignedOnceOnlyAnalyzer),
                                AnalyzerCategories.EncapsulationAndImmutabilityAnalyzers,
                                DefaultState.EnabledByDefault,
                                DiagnosticSeverity.Error,
                                nameof(Resources.AA1101VariablesShouldBeAssignedOnceOnlyTitle),
                                nameof(Resources.AA1101VariablesShouldBeAssignedOnceOnlyDescription),
                                nameof(Resources.AA1101VariablesShouldBeAssignedOnceOnlyMessageFormat),
                                SuppressionAttributes);

        public AnalyzerDetails GetAnalyzerDetails() => AA1001Details;
    }
}