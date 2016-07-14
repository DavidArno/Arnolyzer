using System;
using System.Linq;
using Arnolyzer.Analyzers.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Arnolyzer.Tests.Analyzers.Settings
{
    [TestClass]
    public class SettingsHandlerTests
    {
        [TestMethod]
        public void SettingsFileSpecifiedViaAdditionFiles_IsCorrectlyLoaded()
        {
            var handler = SettingsHandler.CreateHandlerSpecifyingHome("");
            var settings =
                handler.GetArnolyzerSettingsForProject(
                    @"..\..\YamlTestFiles\IgnoreEnvironment\Project1\pretendSourceFile.txt");

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
            var handler = SettingsHandler.CreateHandlerSpecifyingHome("");
            var settings =
                handler.GetArnolyzerSettingsForProject(
                    @"..\..\YamlTestFiles\IgnoreEnvironment\Project2\pretendSourceFile.txt");

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
            Environment.SetEnvironmentVariable("SHT_1", @"..\..\YamlTestFiles\arnolyzer.yml");
            var handler = SettingsHandler.CreateHandlerSpecifyingHome("SHT_1");
            var settings =
                handler.GetArnolyzerSettingsForProject(
                    @"..\..\YamlTestFiles\IgnoreEnvironment\Project2\pretendSourceFile.txt");

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
            Environment.SetEnvironmentVariable("SHT_1", @"..\..\YamlTestFiles\arnolyzer.yml");
            var handler = SettingsHandler.CreateHandlerSpecifyingHome("SHT_1");
            var settings =
                handler.GetArnolyzerSettingsForProject(
                    @"..\..\YamlTestFiles\UseEnvironment\Project1\pretendSourceFile.txt");

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
            Environment.SetEnvironmentVariable("SHT_2", @"..\..\YamlTestFiles\arnolyzer.yml");
            var handler = SettingsHandler.CreateHandlerSpecifyingHome("SHT_3");
            var settings =
                handler.GetArnolyzerSettingsForProject(
                    @"..\..\YamlTestFiles\UseEnvironment\Project1\pretendSourceFile.txt");

            Assert.IsFalse(settings.IgnoreArnolyzerHome);
            Assert.IsFalse(settings.DoNotTraverse);
            Assert.AreEqual("*tmp.cs", settings.IgnorePaths.ToList()[0]);
            Assert.AreEqual("*.123", settings.IgnorePaths.ToList()[1]);
            Assert.AreEqual("1", settings.IgnorePaths.ToList()[2]);
            Assert.AreEqual("2", settings.IgnorePaths.ToList()[3]);
            Assert.AreEqual("3", settings.IgnorePaths.ToList()[4]);
            Assert.AreEqual(5, settings.IgnorePaths.Count());
        }
    }
}