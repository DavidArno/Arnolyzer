using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Arnolyzer.Analyzers;

namespace ArnolyzerDocumentationGenerator
{
    public static class AnalyzerDetailsGenerator
    {
        public static IList<AnalyzerDetails> GetDetailsOfImplementedAnalyzers() =>
            Assembly.Load(new AssemblyName("ArnolyzerAnalyzers"))
                    .DefinedTypes
                    .Where(TypeImplementsInterface<IAnalyzerDetailsReporter>)
                    .Select(type => CreateAnalyzerInstance<IAnalyzerDetailsReporter>(type).GetAnalyzerDetails()).ToList();

        public static IList<AnalyzerDetails> GetDetailsOfPlannedAnalyzers() =>
            Assembly.Load(new AssemblyName("ArnolyzerAnalyzers"))
                    .DefinedTypes
                    .Where(TypeImplementsInterface<IPlannedAnalyzerDetailsReporter>)
                    .Select(type => CreateAnalyzerInstance<IPlannedAnalyzerDetailsReporter>(type).GetAnalyzerDetails()).ToList();

        private static bool TypeImplementsInterface<T>(TypeInfo type) =>
            typeof(T).IsAssignableFrom(type) && !type.IsInterface;

        private static T CreateAnalyzerInstance<T>(Type analyzer) =>
            ((T)Activator.CreateInstance(analyzer));
    }
}