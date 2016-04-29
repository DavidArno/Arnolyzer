using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Arnolyzer.Analyzers.ImmutabilityAnalyzers
{
    // ReSharper disable once UnusedMember.Global - Used by the documentation generator to document this planned analyzer
    public class AA1300ParametersShouldNotBeModifiedAnalyzer : IPlannedAnalyzerDetailsReporter
    {
        private static readonly IList<Type> SuppressionAttributes = new List<Type>();

        private static readonly AnalyzerDetails AA1300Details =
            new AnalyzerDetails(nameof(AA1300ParametersShouldNotBeModifiedAnalyzer),
                                AnalyzerCategories.ImmutabilityAnalyzers,
                                DefaultState.EnabledByDefault,
                                DiagnosticSeverity.Error,
                                nameof(Resources.AA1300ParametersShouldNotBeModifiedTitle),
                                nameof(Resources.AA1300ParametersShouldNotBeModifiedDescription),
                                nameof(Resources.AA1300ParametersShouldNotBeModifiedMessageFormat),
                                SuppressionAttributes);

        public AnalyzerDetails GetAnalyzerDetails() => AA1300Details;
    }
}