using System;
using System.Collections.Generic;
using System.IO;
using Arnolyzer.Analyzers;
using SuccincT.PatternMatchers.GeneralMatcher;
using static ArnolyzerDocumentationGenerator.AnalyzerDetailsGenerator;
using static ArnolyzerDocumentationGenerator.DocumentationTarget;
using static ArnolyzerDocumentationGenerator.ImplementationStatus;
using static ArnolyzerDocumentationGenerator.TemplateProcessors;

namespace ArnolyzerDocumentationGenerator
{
    public static class ArnolyzerDocumentationGenerator
    {
        private static void Main()
        {
            var implementedAnalyzersDetails = GetDetailsOfImplementedAnalyzers();
            var plannedAnalyzerDetails = GetDetailsOfPlannedAnalyzers();
            GenerateReadMe(implementedAnalyzersDetails, plannedAnalyzerDetails);
            GenerateWikiPages(implementedAnalyzersDetails, plannedAnalyzerDetails);
            GenerateWebsitePages(implementedAnalyzersDetails, plannedAnalyzerDetails);
            CopyNonTemplatePagesToDestinations();
        }

        private static void GenerateReadMe(IEnumerable<AnalyzerDetails> implementedAnalyzersDetails,
                                           IEnumerable<AnalyzerDetails> plannedAnalyzerDetails)
        {
            var releaseTemplate = File.ReadAllText(@"..\..\DocumentationTemplates\ReadmeAndWikiReleasesTemplate.md");
            GenerateHomePage(implementedAnalyzersDetails,
                             plannedAnalyzerDetails,
                             ReadMe,
                             "README",
                             "",
                             releaseTemplate,
                             page => $@"https://github.com/DavidArno/Arnolyzer/wiki/{page}.md");
        }


        private static void GenerateWikiPages(IList<AnalyzerDetails> implementedAnalyzersDetails,
                                              IList<AnalyzerDetails> plannedAnalyzerDetails)
        {
            var headerTemplate = File.ReadAllText(@"..\..\DocumentationTemplates\WikiHeaderTemplate.md");
            var releaseTemplate = File.ReadAllText(@"..\..\DocumentationTemplates\ReadmeAndWikiReleasesTemplate.md");
            Func<string, string> linkCreator = page => $"{page}.md";

            GenerateAnalyzerDocuments(implementedAnalyzersDetails, Implemented, Wiki, headerTemplate, linkCreator);
            GenerateAnalyzerDocuments(plannedAnalyzerDetails, Planned, Wiki, headerTemplate, linkCreator);
            GenerateHomePage(implementedAnalyzersDetails,
                             plannedAnalyzerDetails,
                             Wiki,
                             "Home",
                             headerTemplate,
                             releaseTemplate,
                             linkCreator);

            GenerateCategoryPages(implementedAnalyzersDetails, plannedAnalyzerDetails, Wiki, headerTemplate, linkCreator);
            GeneratePreviousReleasesPage(Wiki, headerTemplate);
        }

        private static void GenerateWebsitePages(IList<AnalyzerDetails> implementedAnalyzersDetails,
                                                 IList<AnalyzerDetails> plannedAnalyzerDetails)
        {
            var headerTemplate = File.ReadAllText(@"..\..\DocumentationTemplates\WebPageHeaderTemplate.md");
            var releaseTemplate = File.ReadAllText(@"..\..\DocumentationTemplates\IndexReleasesTemplate.md");
            Func<string, string> linkCreator = page => $"{page}.html";

            GenerateAnalyzerDocuments(implementedAnalyzersDetails, Implemented, Website, headerTemplate, linkCreator);
            GenerateAnalyzerDocuments(plannedAnalyzerDetails, Planned, Website, headerTemplate, linkCreator);
            GenerateHomePage(implementedAnalyzersDetails,
                             plannedAnalyzerDetails,
                             Website,
                             "index",
                             headerTemplate,
                             releaseTemplate,
                             linkCreator);
            GenerateCategoryPages(implementedAnalyzersDetails,
                                  plannedAnalyzerDetails,
                                  Website,
                                  headerTemplate,
                                  linkCreator);
            GeneratePreviousReleasesPage(Website, headerTemplate);
        }

        private static void CopyNonTemplatePagesToDestinations()
        {
            CopyFile(@"..\..\DocumentationTemplates\Installation.md", @"..\..\..\..\Arnolyzer_pages\Installation.md");
            CopyFile(@"..\..\DocumentationTemplates\Contributing.md", @"..\..\..\..\Arnolyzer.wiki\Contributing.md");
        }

        private static void CopyFile(string sourceFile, string destinationFile)
        {
            var text = File.ReadAllText(sourceFile);
            File.WriteAllText(destinationFile, text);
        }

        private static void GenerateAnalyzerDocuments(IEnumerable<AnalyzerDetails> analyzersDetails,
                                                      ImplementationStatus status,
                                                      DocumentationTarget target,
                                                      string headerTemplate,
                                                      Func<string, string> linkCreator)
        {
            var template = File.ReadAllText(@"..\..\DocumentationTemplates\AnalyzerTemplate.md");
            foreach (var details in analyzersDetails)
            {
                var analyzerName = details.DiagnosticId;
                Console.WriteLine($"Generating {analyzerName}.md");

                var extraWords = CreateExtraWordsSet(analyzerName);
                var processedContents =
                    ProcessAnalyzerTemplateText(template, details, status, extraWords, headerTemplate, linkCreator);

                File.WriteAllText($"{DocumentPath(target)}{analyzerName}.md", processedContents);
            }
        }

        private static void GenerateHomePage(IEnumerable<AnalyzerDetails> implementedAnalyzersDetails,
                                             IEnumerable<AnalyzerDetails> plannedAnalyzerDetails,
                                             DocumentationTarget target,
                                             string fileName,
                                             string headerTemplate,
                                             string versionTemplate,
                                             Func<string, string> linkCreator)
        {
            var template = File.ReadAllText(@"..\..\DocumentationTemplates\HomeTemplate.md");
            var processedTemplate = ProcessHomeTemplateText(template,
                                                            implementedAnalyzersDetails,
                                                            plannedAnalyzerDetails,
                                                            headerTemplate,
                                                            versionTemplate,
                                                            linkCreator);
            File.WriteAllText($"{DocumentPath(target)}{fileName}.md", processedTemplate);
        }

        private static void GenerateCategoryPages(IList<AnalyzerDetails> implementedAnalyzersDetails,
                                                  IList<AnalyzerDetails> plannedAnalyzerDetails,
                                                  DocumentationTarget target,
                                                  string headerTemplate,
                                                  Func<string, string> linkCreator)
        {
            var template = File.ReadAllText(@"..\..\DocumentationTemplates\CategoryTemplate.md");
            foreach (var category in AnalyzerCategories.AnalyzerCategoryList)
            {
                var processedContents = ProcessCategoryTemplateText(template,
                                                                    category,
                                                                    implementedAnalyzersDetails,
                                                                    plannedAnalyzerDetails,
                                                                    headerTemplate,
                                                                    linkCreator);
                File.WriteAllText($"{DocumentPath(target)}{category.Name.Replace(" ", "")}.md", processedContents);
            }
        }

        private static void GeneratePreviousReleasesPage(DocumentationTarget target, string headerTemplate)
        {
            var template = File.ReadAllText(@"..\..\DocumentationTemplates\PreviousReleasesTemplate.md");
            var processedTemplate = ProcessPreviousReleasesTemplateText(template, headerTemplate);
            File.WriteAllText($"{DocumentPath(target)}PreviousReleases.md", processedTemplate);
        }

        private static string DocumentPath(DocumentationTarget target) =>
            target.Match().To<string>()
                  .With(ReadMe).Do(@"..\..\..\")
                  .With(Wiki).Do(@"..\..\..\..\Arnolyzer.wiki\")
                  .Else(@"..\..\..\..\Arnolyzer_pages\")
                  .Result();

        private static ExtraWordsContents CreateExtraWordsSet(string analyzerName) => 
            new ExtraWordsContents(FileContentsIfExists($"{analyzerName}.cause.txt"),
                                   FileContentsIfExists($"{analyzerName}.preCodeFix.txt"),
                                   FileContentsIfExists($"{analyzerName}.postCodeFix.txt"),
                                   FileContentsIfExists($"{analyzerName}.preSuppression.txt"),
                                   FileContentsIfExists($"{analyzerName}.postSuppression.txt"));

        private static string FileContentsIfExists(string fileName)
        {
            try
            {
                return File.ReadAllText($@"..\..\DocumentationTemplates\{fileName}");
            }
            catch (FileNotFoundException)
            {
                if (fileName.Contains("cause"))
                {
                    Console.WriteLine($"-----> {fileName} not found.");
                }
                return "";
            }
        }
    }
}
