using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Arnolyzer;
using Arnolyzer.Analyzers;
using Arnolyzer.RuleExceptionAttributes;
using static System.Environment;
using static ArnolyzerDocumentationGenerator.ImplementationStatus;

namespace ArnolyzerDocumentationGenerator
{
    internal static class TemplateProcessors
    {
        public static string ProcessAnalyzerTemplateText(string template, 
                                                         AnalyzerDetails details,
                                                         ImplementationStatus status,
                                                         ExtraWordsContents extraWords)
        {
            return template.Replace("%NAME%", details.Name)
                           .Replace("%NAME-AND-CODE%", details.NameAndCode)
                           .Replace("%STATUS%", status == Implemented ? "Implemented" : "Planned for future release")
                           .Replace("%CODE%", details.Title.ToString())
                           .Replace("%ID%", details.DiagnosticId)
                           .Replace("%DESCRIPTION%", details.Description.ToString())
                           .Replace("%CATEGORY%", details.Category.Name)
                           .Replace("%SEVERITY%", details.SeverityText)
                           .Replace("%ENABLED-BY_DEFAULT%", details.EnabledByDefault ? "Yes" : "No")
                           .Replace("%CAUSE-WORDS%", extraWords.Cause)
                           .Replace("%PRE-CODEFIX-WORDS%", extraWords.PreCodeFix)
                           .Replace("%POST-CODEFIX-WORDS%", extraWords.PostCodeFix)
                           .Replace("%PRE-SUPPRESSION-WORDS%", extraWords.PreSuppression)
                           .Replace("%POST-SUPPRESSION-WORDS%", extraWords.PostSuppression)
                           .Replace("%CODEFIXES%", GenerateCodeFixMessage(status))
                           .Replace("%SUPPRESSIONS%", GenerateSuppressionMessage(details.SuppressionAttributes, status));
        }

        public static string ProcessGeneralTemplateText(string template, 
            IEnumerable<AnalyzerDetails> implementedAnalyzersDetails,
            IEnumerable<AnalyzerDetails> plannedAnalyzerDetails)
        {
            return template.Replace("%DATE%", DateTime.Now.ToString("dd MMM yyyy"))
                           .Replace("%CURRENT-VERSION%", ArnolyzerVersion.Version)
                           .Replace("%CURRENT-RELEASE-NOTES%", GenerateReleaseDetails(ArnolyzerVersion.Version))
                           .Replace("%IMPLEMENTED-LIST%", GenerateAnalyzerLinksList(implementedAnalyzersDetails))
                           .Replace("%PLANNED-LIST%", GenerateAnalyzerLinksList(plannedAnalyzerDetails))
                           .Replace("%CATEGORY-LIST%", GenerateCategoryLinksList())
                           .Replace("%PREVIOUS-RELEASE-NOTES%", GeneratePreviousReleaseNotesLink());
        }

        private static string GenerateCodeFixMessage(ImplementationStatus status) =>
            status == Planned
                ? "Not yet implemented."
                : "There currently aren't any implemented code-fixes for this rule.";


        private static string GenerateSuppressionMessage(IList<Type> suppressionAttributes,
                                                         ImplementationStatus status)
        {
            if (status == Planned) return "Not yet implemented.";

            return suppressionAttributes.Any()
                ? $"This rule can be suppressed using the following attributes: {DescribeEachAttribute(suppressionAttributes)}"
                : "This rule cannot be suppressed.";
        }

        private static string DescribeEachAttribute(IEnumerable<Type> suppressionAttributes) =>
            suppressionAttributes.Aggregate("", (previous, attribute) => $"{previous}{NewLine}{NewLine}{FormatAttribute(attribute)}");

        private static string FormatAttribute(Type attribute)
        {
            var instance = (IAttributeDescriber)Activator.CreateInstance(attribute);
            var name = attribute.Name.Replace("Attribute", "");
            var description = instance.AttributeDescription.Replace("Attribute", " attribute");
            return $"**[{name}]**{NewLine}{description}";
        }

        private static string GenerateReleaseDetails(string version) =>
            File.ReadAllText($@"..\..\..\Arnolyzer.Analyzers\ReleaseNotes\Release.{version}.md");


        private static string GenerateAnalyzerLinksList(IEnumerable<AnalyzerDetails> analyzersDetails)
        {
            var builder = new StringBuilder();
            foreach (var analyzerGroup in from analyzer in analyzersDetails
                                          group analyzer by analyzer.Category into g
                                          orderby g.Key.Code
                                          select g)
            {
                builder.Append($"**{analyzerGroup.Key.Name}**{NewLine}");
                foreach (var analyzer in analyzerGroup.OrderBy(a => a.NameAndCode))
                {
                    builder.Append(CreateAnalyzerLink(analyzer));
                }
                builder.Append(NewLine);
            }

            return builder.ToString();
        }

        private static string CreateAnalyzerLink(AnalyzerDetails analyzer) =>
            $"[{analyzer.NameAndCode}](https://github.com/DavidArno/Arnolyzer/wiki/{analyzer.DiagnosticId}.md){NewLine}";

        private static string GenerateCategoryLinksList() =>
            string.Join(NewLine,
                        AnalyzerCategories.AnalyzerCategoryList.OrderBy(c => c.Name).Select(CreateCategoryPageLink));

        private static string CreateCategoryPageLink(AnalyzerCategoryDetails category) =>
            $"* [{category.Name}](https://github.com/DavidArno/Arnolyzer/wiki/{category.Name.Replace(" ", "")}.md)";

        private static string GeneratePreviousReleaseNotesLink() =>
            "[Previous releases](https://github.com/DavidArno/Arnolyzer/wiki/PreviousReleases.md)";
    }
}