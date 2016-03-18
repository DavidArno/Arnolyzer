using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using static Arnolyzer.SyntacticAnalyzers.Factories.LocalizableStringFactory;

namespace Arnolyzer.SyntacticAnalyzers
{
    public class AnalyzerDetails
    {
        public AnalyzerDetails(string className, 
                               string category,
                               string titleResourceName,
                               string descriptionResourceName,
                               string messageFormatResourceName,
                               IList<Type> suppressionAttributes)
        {
            var decomposedDetails = DecomposeDetailsFromClassName(className);
            Code = decomposedDetails.Item1;
            Name = decomposedDetails.Item2;
            DiagnosticId = decomposedDetails.Item3;
            Category = category;
            SuppressionAttributes = suppressionAttributes;
            Title = LocalizableResourceString(titleResourceName);
            Description = LocalizableResourceString(descriptionResourceName);
            MessageFormat = LocalizableResourceString(messageFormatResourceName);
        }

        public string Code { get; }
        public string Name { get; }
        public string DiagnosticId { get; }
        public string Category { get; }
        public IList<Type> SuppressionAttributes { get; }
        public LocalizableString Title { get; }
        public LocalizableString Description { get; }
        public LocalizableString MessageFormat { get; }


        private static Tuple<string, string, string> DecomposeDetailsFromClassName(string className)
        {
            var nameWithoutAnalyzer = className.Replace("Analyzer", "");
            return new Tuple<string, string, string>(className.Substring(0, 6),
                                                     DeriveNiceNameFromClassName(nameWithoutAnalyzer),
                                                     nameWithoutAnalyzer);
        }

        private static string DeriveNiceNameFromClassName(string className)
        {
            var nameWithoutCode = className.Substring(6);
            var nameWithSpaces = Regex.Replace(nameWithoutCode, @"([^_])([A-Z])", "$1 $2");
            return nameWithSpaces.Replace("_", "-");
        }
    }
}
