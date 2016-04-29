using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Arnolyzer.Analyzers.PureFunctionAnalyzers
{
    // ReSharper disable once UnusedMember.Global - Used by the documentation generator to document this planned analyzer
    public class AA1003StaticMethodsShouldNotCreateStateAnalyzer : IPlannedAnalyzerDetailsReporter
    {
        private static readonly IList<Type> SuppressionAttributes = new List<Type>();

        private static readonly AnalyzerDetails AA1003Details =
            new AnalyzerDetails(nameof(AA1003StaticMethodsShouldNotCreateStateAnalyzer),
                                AnalyzerCategories.PureFunctionAnalyzers,
                                DefaultState.EnabledByDefault,
                                DiagnosticSeverity.Error,
                                nameof(Resources.AA1003StaticMethodsShouldNotCreateStateTitle),
                                nameof(Resources.AA1003StaticMethodsShouldNotCreateStateDescription),
                                nameof(Resources.AA1003StaticMethodsShouldNotCreateStateMessageFormat),
                                SuppressionAttributes);

        public AnalyzerDetails GetAnalyzerDetails() => AA1003Details;
    }
}