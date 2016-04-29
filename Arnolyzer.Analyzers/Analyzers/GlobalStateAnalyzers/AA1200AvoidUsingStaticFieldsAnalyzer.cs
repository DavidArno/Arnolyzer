using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Arnolyzer.Analyzers.GlobalStateAnalyzers
{
    // ReSharper disable once UnusedMember.Global - Used by the documentation generator to document this planned analyzer
    public class AA1200AvoidUsingStaticFieldsAnalyzer : IPlannedAnalyzerDetailsReporter
    {
        private static readonly IList<Type> SuppressionAttributes = new List<Type>();

        private static readonly AnalyzerDetails AA2101Details =
            new AnalyzerDetails(nameof(AA1200AvoidUsingStaticFieldsAnalyzer),
                                AnalyzerCategories.GlobalStateAnalyzers,
                                DefaultState.EnabledByDefault,
                                DiagnosticSeverity.Error,
                                nameof(Resources.AA1200AvoidUsingStaticFieldsTitle),
                                nameof(Resources.AA1200AvoidUsingStaticFieldsDescription),
                                nameof(Resources.AA1200AvoidUsingStaticFieldsMessageFormat),
                                SuppressionAttributes);

        public AnalyzerDetails GetAnalyzerDetails() => AA2101Details;
    }
}