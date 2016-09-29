using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Arnolyzer;
using Arnolyzer.Analyzers;
using Arnolyzer.RuleExceptionAttributes;
using static System.Environment;
using static System.Reflection.BindingFlags;
using static ArnolyzerDocumentationGenerator.ImplementationStatus;

namespace ArnolyzerDocumentationGenerator
{
    internal static class TemplateProcessors
    {
        public static string ProcessAnalyzerTemplateText(string template, 
                                                         AnalyzerDetails details,
                                                         ImplementationStatus status,
                                                         ExtraWordsContents extraWords,
                                                         string headerTemplate,
                                                         Func<string, string> linkCreator)
        {
            return template.Replace("%HEADER%", ProcessHeaderTemplate(headerTemplate, details.NameWithCode))
                           .Replace("%NAME%", details.Name)
                           .Replace("%NAME-AND-CODE%", details.NameWithCode)
                           .Replace("%STATUS%", status == Implemented ? "Implemented" : "Planned for a future release")
                           .Replace("%CODE%", details.Title.ToString())
                           .Replace("%ID%", details.DiagnosticId)
                           .Replace("%DESCRIPTION%", details.Description.ToString())
                           .Replace("%CATEGORY%", details.Category.Name)
                           .Replace("%CATEGORY-LINK%", $"{linkCreator(CategoryNameLink(details.Category))}")
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

        public static string ProcessHomeTemplateText(string template,
                                                     IEnumerable<AnalyzerDetails> implementedAnalyzersDetails,
                                                     IEnumerable<AnalyzerDetails> plannedAnalyzerDetails,
                                                     string headerTemplate,
                                                     string releasetemplate,
                                                     Func<string, string> linkCreator)
        {
            return template.Replace("%DATE%", DateTime.Now.ToString("dd MMM yyyy"))
                           .Replace("%IMPLEMENTED-LIST%",
                                    GenerateAllAnalyzerLinksList(implementedAnalyzersDetails, linkCreator))
                           .Replace("%PLANNED-LIST%", GenerateAllAnalyzerLinksList(plannedAnalyzerDetails, linkCreator))
                           .Replace("%CATEGORY-LIST%", GenerateCategoryLinksList(linkCreator))
                           .Replace("%HEADER%", ProcessHeaderTemplate(headerTemplate, "Home"))
                           .Replace("%RELEASE-AND-DEVELOPMENT%", ProcessReleaseTemplate(releasetemplate, linkCreator));
        }

        public static string ProcessCategoryTemplateText(string template,
                                                         AnalyzerCategoryDetails category,
                                                         IEnumerable<AnalyzerDetails> implementedAnalyzersDetails,
                                                         IEnumerable<AnalyzerDetails> plannedAnalyzerDetails,
                                                         string headerTemplate,
                                                         Func<string, string> linkCreator)
        {
            return template.Replace("%NAME%", category.Name)
                           .Replace("%HEADER%", ProcessHeaderTemplate(headerTemplate, category.Name))
                           .Replace("%DESCRIPTION%", category.Description)
                           .Replace("%IMPLEMENTED-LIST%",
                                    GenerateSpecificAnalyzerLinksList(implementedAnalyzersDetails,
                                                                      category,
                                                                      "implemented yet",
                                                                      linkCreator))
                           .Replace("%PLANNED-LIST%",
                                    GenerateSpecificAnalyzerLinksList(plannedAnalyzerDetails,
                                                                      category,
                                                                      "planned",
                                                                      linkCreator));
        }

        public static string ProcessPreviousReleasesTemplateText(string template, string headerTemplate) =>
            template.Replace("%VERSION-HISTORY%", GenerateVersionHistory(ArnolyzerVersion.Version))
                    .Replace("%HEADER%", ProcessHeaderTemplate(headerTemplate, "Previous releases"));

        private static string ProcessHeaderTemplate(string template, string title) => template.Replace("%TITLE%", title);

        private static string ProcessReleaseTemplate(string template, Func<string, string> linkCreator) =>
            template.Replace("%CURRENT-VERSION%", ArnolyzerVersion.Version)
                    .Replace("%CURRENT-RELEASE-NOTES%", FetchReleaseDetails(ArnolyzerVersion.Version))
                    .Replace("%PREVIOUS-RELEASE-NOTES%", GeneratePreviousReleaseNotesLink(linkCreator))
                    .Replace("%LAST-RELEASE%", ArnolyzerVersion.LastRelease)
                    .Replace("%LAST-RELEASE-NOTES%", FetchReleaseDetails(ArnolyzerVersion.LastRelease))
                    .Replace("%DATE%", DateTime.Now.ToString("dd MMM yyyy"));


        private static string GenerateAllAnalyzerLinksList(IEnumerable<AnalyzerDetails> analyzerDetails,
                                                           Func<string, string> linkCreator)
        {
            return GenerateAnalyzerLinksList(analyzerDetails,
                                             category => true,
                                             category => $"**{category.Name}**{NewLine}",
                                             linkCreator);
        }

        private static string GenerateSpecificAnalyzerLinksList(IEnumerable<AnalyzerDetails> analyzersDetails,
                                                                AnalyzerCategoryDetails category,
                                                                string description,
                                                                Func<string, string> linkCreator)
        {
            var links = GenerateAnalyzerLinksList(analyzersDetails,
                                                  cat => cat.Name == category.Name,
                                                  cat => "",
                                                  linkCreator);
            return links == "" ? $"There are no analyzers {description} for this category." : links;
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
            suppressionAttributes
                .Aggregate("", (previous, attribute) => $"{previous}{NewLine}{NewLine}{FormatAttribute(attribute)}");

        private static string FormatAttribute(Type attribute)
        {
            
                var instance = CreateAttributeInstanceUsingPrivateConstructorIfRequired(attribute);
                var name = attribute.Name.Replace("Attribute", "");
                var description = instance.AttributeDescription.Replace("Attribute", " attribute");
                return $"**[{name}]**<br/>{description}";
        }

        private static IAttributeDescriber CreateAttributeInstanceUsingPrivateConstructorIfRequired(Type attribute)
        {
            try
            {
                return (IAttributeDescriber)Activator.CreateInstance(attribute);
            }
            catch (MissingMethodException)
            {
                var constructor = attribute.GetConstructor(NonPublic|Instance, null, new Type[0], null);
                return (IAttributeDescriber)constructor.Invoke(null);
            }
        }

        private static string GenerateVersionHistory(string currentVersion) =>
            Directory.GetFiles(@"..\..\..\Arnolyzer.Analyzers\ReleaseNotes")
                     .Where(f => !f.EndsWith($"Release.{currentVersion}.md"))
                     .OrderBy(f => f)
                     .Reverse()
                     .Aggregate("", (previous, file) => $"{previous}{NewLine}{NewLine}{DescribeRelease(file)}");

        private static string DescribeRelease(string file) =>
            $"**Release {ExtractVersionFromFile(file)}**{NewLine}{File.ReadAllText(file)}";

        private static string ExtractVersionFromFile(string fileName)
        {
            var splitParts = fileName.Split('.');
            var partsCount = splitParts.Length;
            return $"{splitParts[partsCount - 4]}.{splitParts[partsCount - 3]}.{splitParts[partsCount - 2]}";
        }

        private static string FetchReleaseDetails(string version) =>
            File.ReadAllText($@"..\..\..\Arnolyzer.Analyzers\ReleaseNotes\Release.{version}.md");

        private static string GenerateAnalyzerLinksList(IEnumerable<AnalyzerDetails> analyzersDetails,
                                                        Func<AnalyzerCategoryDetails, bool> matchCategory,
                                                        Func<AnalyzerCategoryDetails, string> categoryDescription,
                                                        Func<string, string> linkCreator)
        {
            var builder = new StringBuilder();
            foreach (var analyzerGroup in from analyzer in analyzersDetails
                                          where matchCategory(analyzer.Category)
                                          group analyzer by analyzer.Category
                                          into g
                                          orderby g.Key.Code
                                          select g)
            {
                builder.Append(categoryDescription(analyzerGroup.Key));
                foreach (var analyzer in analyzerGroup.OrderBy(a => a.NameWithCode))
                {
                    builder.Append(CreateAnalyzerLink(analyzer, linkCreator));
                }

                builder.Append(NewLine);
            }

            return builder.ToString();
        }

        private static string CreateAnalyzerLink(AnalyzerDetails analyzer, Func<string, string> linkCreator) =>
            $"* [{analyzer.NameWithCode}]({linkCreator(analyzer.DiagnosticId)}){NewLine}";

        private static string GenerateCategoryLinksList(Func<string, string> linkCreator) =>
            string.Join(NewLine,
                        AnalyzerCategories.AnalyzerCategoryList.OrderBy(c => c.Name)
                                          .Select(category => CreateCategoryPageLink(category, linkCreator)));

        private static string CreateCategoryPageLink(AnalyzerCategoryDetails category,
                                                     Func<string, string> linkCreator)
        {
            return $"* [{category.Name}]({linkCreator(CategoryNameLink(category))})";
        }

        private static string GeneratePreviousReleaseNotesLink(Func<string, string> linkCreator) =>
            $"[Previous releases]({linkCreator("PreviousReleases")})";

        private static string CategoryNameLink(AnalyzerCategoryDetails category) => category.Name.Replace(" ", "");
    }
}