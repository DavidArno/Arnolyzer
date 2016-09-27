using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Arnolyzer.Analyzers
{
    public class NamedItemSuppresionAttributeDetails
    {
        private readonly AnalyzerDetails _analyzerDetails;

        public NamedItemSuppresionAttributeDetails(string className, 
                               AnalyzerCategoryDetails category,
                               DefaultState defaultState,
                               DiagnosticSeverity severity,
                               string titleResourceName,
                               string descriptionResourceName,
                               string messageFormatResourceName)
        {
            _analyzerDetails = new AnalyzerDetails(className,
                                                   category,
                                                   defaultState,
                                                   severity,
                                                   titleResourceName,
                                                   descriptionResourceName,
                                                   messageFormatResourceName,
                                                   new List<Type>());
        }

        public string DiagnosticId => _analyzerDetails.DiagnosticId;
        public string CategoryName => _analyzerDetails.Category.Name;
        public LocalizableString Title => _analyzerDetails.Title;
        public LocalizableString Description => _analyzerDetails.Description;
        public DiagnosticSeverity Severity => _analyzerDetails.Severity;

        public DiagnosticDescriptor GetDiagnosticDescriptor() =>
            new DiagnosticDescriptor(_analyzerDetails.DiagnosticId,
                                     Title,
                                     _analyzerDetails.MessageFormat,
                                     CategoryName,
                                     Severity,
                                     _analyzerDetails.EnabledByDefault,
                                     Description);
    }
}
