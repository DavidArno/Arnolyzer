using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Arnolyzer.RuleExceptionAttributes;
using Arnolyzer.SyntacticAnalyzers;
using static System.Environment;

namespace ArnolyzerDocumentationGenerator
{
    public static class ArnolyzerDocumentationGenerator
    {
        private static void Main()
        {
            var type = typeof(IAnalyzerDetailsReporter);
            foreach (var implementation in Assembly.Load(new AssemblyName("ArnolyzerAnalyzers")).DefinedTypes
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface))
            {
                Console.WriteLine(implementation.Name);
                var implementationName = implementation.Name;
                var rawContents = File.ReadAllText($@"..\..\..\Arnolyzer.Analyzers\DocumentationTemplates\{implementationName}.mdt");
                var details = ((IAnalyzerDetailsReporter)Activator.CreateInstance(implementation)).GetAnalyzerDetails();
                var processedContents = rawContents
                    .Replace("%NAME%", details.Name)
                    .Replace("%CODE%", details.Code)
                    .Replace("%ID%", details.DiagnosticId)
                    .Replace("%DESCRIPTION%", details.Description.ToString())
                    .Replace("%CATEGORY%", details.Category)
                    .Replace("%CODEFIXES%", "There currently aren't any implemented code-fixes for this rule.")
                    .Replace("%SUPPRESSIONS%", GenerateSuppressionMessages(details.SuppressionAttributes));

                File.WriteAllText($@"..\..\..\..\Arnolyzer.wiki\{implementationName}.md",
                                  processedContents);
            }
        }

        private static string GenerateSuppressionMessages(IList<Type> suppressionAttributes) => 
            suppressionAttributes.Any()
                ? $"This rule can be suppressed using the following attributes: {DescribeEachAttribute(suppressionAttributes)}"
                : "";

        private static string DescribeEachAttribute(IEnumerable<Type> suppressionAttributes) => 
            suppressionAttributes.Aggregate("", (previous, attribute) => $"{previous}{NewLine}{NewLine}{FormatAttribute(attribute)}");

        private static string FormatAttribute(Type attribute)
        {
            var instance = (IAttributeDescriber) Activator.CreateInstance(attribute);
            var name = attribute.Name.Replace("Attribute", "");
            var description = instance.AttributeDescription.Replace("Attribute", " attribute");
            return $"[{name}]{NewLine}{description}";
        }
    }
}
