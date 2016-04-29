using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Arnolyzer.Analyzers.GlobalStateAnalyzers
{
    // ReSharper disable once UnusedMember.Global - Used by the documentation generator to document this planned analyzer
    public class AA1201AvoidUsingStaticPropertiesAnalyzer : IPlannedAnalyzerDetailsReporter
    {
        private static readonly IList<Type> SuppressionAttributes = new List<Type>();

        private static readonly AnalyzerDetails AA2101Details =
            new AnalyzerDetails(nameof(AA1201AvoidUsingStaticPropertiesAnalyzer),
                                AnalyzerCategories.GlobalStateAnalyzers,
                                DefaultState.EnabledByDefault,
                                DiagnosticSeverity.Error,
                                nameof(Resources.AA1201AvoidUsingStaticPropertiesTitle),
                                nameof(Resources.AA1201AvoidUsingStaticPropertiesDescription),
                                nameof(Resources.AA1201AvoidUsingStaticPropertiesMessageFormat),
                                SuppressionAttributes);

        public AnalyzerDetails GetAnalyzerDetails() => AA2101Details;
    }
}