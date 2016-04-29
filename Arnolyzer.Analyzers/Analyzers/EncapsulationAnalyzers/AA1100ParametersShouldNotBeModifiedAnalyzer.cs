using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Arnolyzer.Analyzers.EncapsulationAnalyzers
{
    // ReSharper disable once UnusedMember.Global - Used by the documentation generator to document this planned analyzer
    public class AA1100ParametersShouldNotBeModifiedAnalyzer : IPlannedAnalyzerDetailsReporter
    {
        private static readonly IList<Type> SuppressionAttributes = new List<Type>();

        private static readonly AnalyzerDetails AA1002Details =
            new AnalyzerDetails(nameof(AA1100ParametersShouldNotBeModifiedAnalyzer),
                                AnalyzerCategories.EncapsulationAndImmutabilityAnalyzers,
                                DefaultState.EnabledByDefault,
                                DiagnosticSeverity.Error,
                                nameof(Resources.AA1100ParametersShouldNotBeModifiedTitle),
                                nameof(Resources.AA1100ParametersShouldNotBeModifiedDescription),
                                nameof(Resources.AA1100ParametersShouldNotBeModifiedMessageFormat),
                                SuppressionAttributes);

        public AnalyzerDetails GetAnalyzerDetails() => AA1002Details;
    }
}