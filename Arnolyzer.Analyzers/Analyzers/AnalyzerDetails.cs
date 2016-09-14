using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Arnolyzer.Analyzers.Factories;
using Microsoft.CodeAnalysis;

namespace Arnolyzer.Analyzers
{
    public class AnalyzerDetails
    {
        private readonly DefaultState _defaultState;

        public AnalyzerDetails(string className, 
                               AnalyzerCategoryDetails category,
                               DefaultState defaultState,
                               DiagnosticSeverity severity,
                               string titleResourceName,
                               string descriptionResourceName,
                               string messageFormatResourceName,
                               IList<Type> suppressionAttributes)
        {
            var decomposedDetails = DecomposeDetailsFromClassName(className);
            var code = decomposedDetails.Item1;
            Name = decomposedDetails.Item2;
            NameWithCode = $"{decomposedDetails.Item3} - {Name}";
            Category = category;
            _defaultState = defaultState;
            Severity = severity;
            SuppressionAttributes = suppressionAttributes;

            Title = LocalizableStringFactory.LocalizableResourceString(titleResourceName);
            Description = LocalizableStringFactory.LocalizableResourceString(descriptionResourceName);
            MessageFormat = LocalizableStringFactory.LocalizableResourceString(messageFormatResourceName);
            DiagnosticId = Title.ToString().Replace("-", "");

            if (Title.ToString() != code)
            {
                throw new ArgumentException($@"Title resource value isn't of the correct format: should be {code}",
                                            nameof(titleResourceName));
            }
        }

        public string Name { get; }
        public string DiagnosticId { get; }
        public AnalyzerCategoryDetails Category { get; }
        public IList<Type> SuppressionAttributes { get; }
        public LocalizableString Title { get; }
        public LocalizableString Description { get; }
        public LocalizableString MessageFormat { get; }
        public DiagnosticSeverity Severity { get; }
        public string SeverityText => Severity.SeverityType();
        public bool EnabledByDefault => _defaultState.IsEnabledByDefault();
        public string NameWithCode { get; }

        public DiagnosticDescriptor GetDiagnosticDescriptor() =>
            new DiagnosticDescriptor(DiagnosticId,
                                     Title,
                                     MessageFormat,
                                     Category.Name,
                                     Severity,
                                     EnabledByDefault,
                                     Description);

        private static Tuple<string, string, string> DecomposeDetailsFromClassName(string className)
        {
            var nameWithoutAnalyzer = className.Replace("Analyzer", "");
            return new Tuple<string, string, string>(DeriveCodeFromClassName(nameWithoutAnalyzer),
                                                     DeriveNiceNameFromClassName(nameWithoutAnalyzer),
                                                     nameWithoutAnalyzer.Substring(0, 6));
        }

        private static string DeriveNiceNameFromClassName(string className)
        {
            var nameWithoutCode = className.Substring(6);
            var nameWithSpaces = Regex.Replace(nameWithoutCode, @"([^_])([A-Z])", "$1 $2");
            return nameWithSpaces.Replace("_", "-");
        }

        private static string DeriveCodeFromClassName(string className) => 
            className.Substring(0, 6) + "-" + className.Substring(6).Replace("_", "");
    }
}
