using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Arnolyzer.Analyzers;
using static System.Environment;
using static ArnolyzerDocumentationGenerator.AnalyzerDetailsGenerator;
using static ArnolyzerDocumentationGenerator.ImplementationStatus;

namespace ArnolyzerDocumentationGenerator
{
    public static class ArnolyzerDocumentationGenerator
    {
        private static void Main()
        {
            var implementedAnalyzersDetails = GetDetailsOfImplementedAnalyzers();
            var plannedAnalyzerDetails = GetDetailsOfPlannedAnalyzers();
            GenerateAnalyzerDocuments(implementedAnalyzersDetails, Implemented);
            GenerateAnalyzerDocuments(plannedAnalyzerDetails, Planned);
            GenerateReadMe(implementedAnalyzersDetails, plannedAnalyzerDetails);
        }

        private static  void GenerateAnalyzerDocuments(IEnumerable<AnalyzerDetails> analyzersDetails, ImplementationStatus status)
        {
            var template = File.ReadAllText(@"..\..\..\Arnolyzer.Analyzers\DocumentationTemplates\AnalyzerTemplate.md");
            foreach (var details in analyzersDetails)
            {
                var analyzerName = details.DiagnosticId;
                Console.WriteLine($"Generating {analyzerName}.md");

                var extraWords = CreateExtraWordsSet(analyzerName);
                var processedContents = TemplateProcessors.ProcessAnalyzerTemplateText(template, details, status, extraWords);

                File.WriteAllText($@"..\..\..\..\Arnolyzer.wiki\{analyzerName}.md",
                                  processedContents);
            }
        }

        private static void GenerateReadMe(IEnumerable<AnalyzerDetails> implementedAnalyzersDetails,
                                           IEnumerable<AnalyzerDetails> plannedAnalyzerDetails)
        {
            var template = File.ReadAllText(@"..\..\..\Arnolyzer.Analyzers\DocumentationTemplates\README-template.md");
            File.WriteAllText(@"..\..\..\README.md", TemplateProcessors.ProcessGeneralTemplateText(template, implementedAnalyzersDetails, plannedAnalyzerDetails));
        }

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
                return File.ReadAllText($@"..\..\..\Arnolyzer.Analyzers\DocumentationTemplates\{fileName}");
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
