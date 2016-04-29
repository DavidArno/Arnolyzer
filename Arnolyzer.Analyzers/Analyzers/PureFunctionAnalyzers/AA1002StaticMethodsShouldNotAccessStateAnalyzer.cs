using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Arnolyzer.Analyzers.PureFunctionAnalyzers
{
    // ReSharper disable once UnusedMember.Global - Used by the documentation generator to document this planned analyzer
    public class AA1002StaticMethodsShouldNotAccessStateAnalyzer : IPlannedAnalyzerDetailsReporter
    {
        private static readonly IList<Type> SuppressionAttributes = new List<Type>();

        private static readonly AnalyzerDetails AA1003Details =
            new AnalyzerDetails(nameof(AA1002StaticMethodsShouldNotAccessStateAnalyzer),
                                AnalyzerCategories.PureFunctionAnalyzers,
                                DefaultState.EnabledByDefault,
                                DiagnosticSeverity.Error,
                                nameof(Resources.AA1002StaticMethodsShouldNotAccessStateTitle),
                                nameof(Resources.AA1002StaticMethodsShouldNotAccessStateDescription),
                                nameof(Resources.AA1002StaticMethodsShouldNotAccessStateMessageFormat),
                                SuppressionAttributes);

        public AnalyzerDetails GetAnalyzerDetails() => AA1003Details;
    }
}