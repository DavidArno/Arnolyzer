using System;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Arnolyzer.Tests.DiagnosticVerification
{
    internal static class DocumentSetCreator
    {
        private static readonly MetadataReference CorlibReference =
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
        private static readonly MetadataReference SystemCoreReference =
            MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location);

        private const string TestProjectName = "TestProject";

        public static Document CreateDocumentSetFromSources(string filePath)
        {
            var documents = CreateProject(filePath).Documents.ToArray();

            if (documents.Length != 1)
            {
                throw new SystemException($"Expected one document from the filePath, but got {documents.Length}");
            }

            return documents[0];
        }

        private static Project CreateProject(string filePath)
        {
            var projectId = ProjectId.CreateNewId(TestProjectName);
            var source = File.ReadAllText(filePath);
            var fileName = Path.GetFileName(filePath);
            var documentId = DocumentId.CreateNewId(projectId, fileName);

            var solution = new AdhocWorkspace()
                .CurrentSolution
                .AddProject(projectId, TestProjectName, TestProjectName, LanguageNames.CSharp)
                .AddMetadataReference(projectId, CorlibReference)
                .AddMetadataReference(projectId, SystemCoreReference)
                .AddDocument(documentId, fileName, SourceText.From(source));

            return solution.GetProject(projectId);
        }
    }
}