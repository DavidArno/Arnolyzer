using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Arnolyzer.Analyzers.Settings;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Arnolyzer.Tests.SyntacticAnalyzers.Settings
{
    [TestClass]
    public class SettingsHandlerTests
    {
        [TestMethod]
        public void SettingsFileSpecifiedViaAdditionFiles_IsCorrectlyLoaded()
        {
            var additionalFiles = new List<AdditionalText>
            {
                new TestAdditionalText {ThePath = @"..\..\YamlTestFiles\IgnoreEnvironment\Project1\settings.yaml"},
                new TestAdditionalText {ThePath = @"..\..\YamlTestFiles\IgnoreEnvironment\Project1\randomfile.txt"}
            }.ToImmutableArray();
            var handler = new SettingsHandler("settings.yaml", "", additionalFiles);
            var settings = handler.GetArnolyzerSettingsForProject(@"..\..\YamlTestFiles\IgnoreEnvironment\Project1\pretendSourceFile.txt");

            Assert.IsTrue(settings.IgnoreArnolyzerHome);
            Assert.IsTrue(settings.DoNotTraverse);
            Assert.AreEqual("tmp.cs", settings.IgnorePaths.ToList()[0]);
            Assert.AreEqual("*.123", settings.IgnorePaths.ToList()[1]);
            Assert.AreEqual(2, settings.IgnorePaths.Count());
            Assert.AreEqual(@"(.*tmp\.cs)|(.*\.123)", settings.IgnorePathsRegex);
        }

        [TestMethod]
        public void SettingsFileThatAllowsTraversal_CorrectlyLoadsProjectAndSolutionSettings()
        {
            var additionalFiles = new List<AdditionalText>
            {
                new TestAdditionalText {ThePath = @"..\..\YamlTestFiles\IgnoreEnvironment\Project2\settings.yaml"}
            }.ToImmutableArray();
            var handler = new SettingsHandler("settings.yaml", "", additionalFiles);
            var settings = handler.GetArnolyzerSettingsForProject(@"..\..\YamlTestFiles\IgnoreEnvironment\Project2\pretendSourceFile.txt");

            Assert.IsTrue(settings.IgnoreArnolyzerHome);
            Assert.IsFalse(settings.DoNotTraverse);
            Assert.AreEqual("*.*", settings.IgnorePaths.ToList()[0]);
            Assert.AreEqual("1", settings.IgnorePaths.ToList()[1]);
            Assert.AreEqual("2", settings.IgnorePaths.ToList()[2]);
            Assert.AreEqual("3", settings.IgnorePaths.ToList()[3]);
            Assert.AreEqual(4, settings.IgnorePaths.Count());
            Assert.AreEqual(@"(.*\..*)|(.*1)|(.*2)|(.*3)", settings.IgnorePathsRegex);
        }

        [TestMethod]
        public void SettingsFileThatAllowsTraversalButNotEnvironment_OnlyLoadsProjectAndSolutionSettings()
        {
            Environment.SetEnvironmentVariable("SHT_1", @"..\..\YamlTestFiles\settings.yaml");
            var additionalFiles = new List<AdditionalText>
            {
                new TestAdditionalText {ThePath = @"..\..\YamlTestFiles\IgnoreEnvironment\Project2\settings.yaml"}
            }.ToImmutableArray();
            var handler = new SettingsHandler("settings.yaml", "SHT_1", additionalFiles);
            var settings = handler.GetArnolyzerSettingsForProject(@"..\..\YamlTestFiles\IgnoreEnvironment\Project2\pretendSourceFile.txt");

            Assert.IsTrue(settings.IgnoreArnolyzerHome);
            Assert.IsFalse(settings.DoNotTraverse);
            Assert.AreEqual("*.*", settings.IgnorePaths.ToList()[0]);
            Assert.AreEqual("1", settings.IgnorePaths.ToList()[1]);
            Assert.AreEqual("2", settings.IgnorePaths.ToList()[2]);
            Assert.AreEqual("3", settings.IgnorePaths.ToList()[3]);
            Assert.AreEqual(4, settings.IgnorePaths.Count());
            Assert.AreEqual(@"(.*\..*)|(.*1)|(.*2)|(.*3)", settings.IgnorePathsRegex);
        }

        [TestMethod]
        public void SettingsFileThatAllowsTraversalAndEnvironment_LoadsAllSettings()
        {
            Environment.SetEnvironmentVariable("SHT_1", @"..\..\YamlTestFiles\settings.yaml");
            var additionalFiles = new List<AdditionalText>
            {
                new TestAdditionalText {ThePath = @"..\..\YamlTestFiles\UseEnvironment\Project1\settings.yaml"}
            }.ToImmutableArray();
            var handler = new SettingsHandler("settings.yaml", "SHT_1", additionalFiles);
            var settings = handler.GetArnolyzerSettingsForProject(@"..\..\YamlTestFiles\UseEnvironment\Project1\pretendSourceFile.txt");

            Assert.IsTrue(settings.IgnoreArnolyzerHome);
            Assert.IsTrue(settings.DoNotTraverse);
            Assert.AreEqual("*tmp.cs", settings.IgnorePaths.ToList()[0]);
            Assert.AreEqual("*.123", settings.IgnorePaths.ToList()[1]);
            Assert.AreEqual("1", settings.IgnorePaths.ToList()[2]);
            Assert.AreEqual("2", settings.IgnorePaths.ToList()[3]);
            Assert.AreEqual("3", settings.IgnorePaths.ToList()[4]);
            Assert.AreEqual("e1", settings.IgnorePaths.ToList()[5]);
            Assert.AreEqual("e2", settings.IgnorePaths.ToList()[6]);
            Assert.AreEqual(7, settings.IgnorePaths.Count());
        }

        [TestMethod]
        public void IncorrectlySetEnvironmentVariable_OnlyLoadsProjectAndSolutionSettings()
        {
            Environment.SetEnvironmentVariable("SHT_2", @"..\..\YamlTestFiles\settings.yaml");
            var additionalFiles = new List<AdditionalText>
            {
                new TestAdditionalText {ThePath = @"..\..\YamlTestFiles\UseEnvironment\Project1\settings.yaml"}
            }.ToImmutableArray();
            var handler = new SettingsHandler("settings.yaml", "SHT_3", additionalFiles);
            var settings = handler.GetArnolyzerSettingsForProject(@"..\..\YamlTestFiles\UseEnvironment\Project1\pretendSourceFile.txt");

            Assert.IsFalse(settings.IgnoreArnolyzerHome);
            Assert.IsFalse(settings.DoNotTraverse);
            Assert.AreEqual("*tmp.cs", settings.IgnorePaths.ToList()[0]);
            Assert.AreEqual("*.123", settings.IgnorePaths.ToList()[1]);
            Assert.AreEqual("1", settings.IgnorePaths.ToList()[2]);
            Assert.AreEqual("2", settings.IgnorePaths.ToList()[3]);
            Assert.AreEqual("3", settings.IgnorePaths.ToList()[4]);
            Assert.AreEqual(5, settings.IgnorePaths.Count());
        }

        private class TestAdditionalText : AdditionalText
        {
            public string ThePath;
            public override SourceText GetText(CancellationToken cancellationToken = new CancellationToken()) => null;

            public override string Path => ThePath;
        }
    }
}