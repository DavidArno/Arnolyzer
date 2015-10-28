using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Arnolyzer.Test.DiagnosticVerification
{
    internal static class DocumentSetCreator
    {
        private static readonly MetadataReference CorlibReference =
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
        private static readonly MetadataReference SystemCoreReference =
            MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location);

        private const string DefaultFilePathPrefix = "Test";
        private const string CSharpDefaultFileExt = "cs";
        private const string TestProjectName = "TestProject";

        public static Document[] CreateDocumentSetFromSources(string[] sources)
        {
            var documents = CreateProject(sources).Documents.ToArray();

            if (sources.Length != documents.Length)
            {
                throw new SystemException("Amount of sources did not match amount of Documents created");
            }

            return documents;
        }

        private static Project CreateProject(IEnumerable<string> sources)
        {
            var projectId = ProjectId.CreateNewId(TestProjectName);

            var solution = new AdhocWorkspace()
                .CurrentSolution
                .AddProject(projectId, TestProjectName, TestProjectName, LanguageNames.CSharp)
                .AddMetadataReference(projectId, CorlibReference)
                .AddMetadataReference(projectId, SystemCoreReference);

            var count = 0;
            foreach (var source in sources)
            {
                var newFileName = $"{DefaultFilePathPrefix}{count++}.{CSharpDefaultFileExt}";
                var documentId = DocumentId.CreateNewId(projectId, newFileName);
                solution = solution.AddDocument(documentId, newFileName, SourceText.From(source));
            }
            return solution.GetProject(projectId);
        }
    }
}