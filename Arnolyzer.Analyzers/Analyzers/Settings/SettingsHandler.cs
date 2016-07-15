using System;
using System.IO;
using Arnolyzer.RuleExceptionAttributes;
using SuccincT.Options;
using YamlDotNet.Serialization;
using static System.Environment;

namespace Arnolyzer.Analyzers.Settings
{
    public class SettingsHandler
    {
        private const string SettingsFileName = "arnolyzer.yml";

        private readonly string _settingsFileName;
        private readonly string _arnolyzerHome;

        private SettingsDetails _mergedSettings;

        [ConstantValueProvider]
        public static SettingsHandler CreateHandler() => new SettingsHandler(SettingsFileName, "ARNOLYZER_HOME");

        public static SettingsHandler CreateHandlerSpecifyingHome(string arnolyzerHome) => 
            new SettingsHandler(SettingsFileName, arnolyzerHome);

        private SettingsHandler(string settingsFileName,
                               string arnolyzerHome)
        {
            _settingsFileName = settingsFileName;
            _arnolyzerHome = arnolyzerHome;
        }

        public SettingsDetails GetArnolyzerSettingsForProject(string filePath)
        {
            if (_mergedSettings != null) { return _mergedSettings; }

            var projectSettings = GetProjectSpecificSettings(filePath, _settingsFileName);
            var solutionSettings = AddSolutionWideSettings(filePath, projectSettings, _settingsFileName);
            _mergedSettings = AddGlobalSettings(solutionSettings, _arnolyzerHome);
            return _mergedSettings;
        }

        private static SettingsDetails GetProjectSpecificSettings(string filePath,
                                                                  string settingsFileName)
        {
            return TraversePathToFindCollocatedSettings(new FileInfo(filePath).Directory,
                                                        settingsFileName,
                                                        ".csproj");
        }

        private static SettingsDetails AddSolutionWideSettings(string filePath,
                                                               SettingsDetails projectSettings,
                                                               string settingsFileName)
        {
            if (projectSettings.DoNotTraverse) { return projectSettings; }

            var solutionSettings = TraversePathToFindCollocatedSettings(new FileInfo(filePath).Directory,
                                                                        settingsFileName,
                                                                        ".sln");

            return SettingsDetails.Merge(projectSettings, solutionSettings);
        }

        private static SettingsDetails TraversePathToFindCollocatedSettings(DirectoryInfo directory,
                                                                            string settingsFileName,
                                                                            string collocatedFile)
        {
            if (directory == null) return new SettingsDetails();

            var solutionFile = directory.GetFiles().TryFirst(f => f.Name.EndsWith(collocatedFile));
            if (solutionFile.HasValue)
            {
                return directory.GetFiles()
                                .TryFirst(f => f.Name.EndsWith(settingsFileName))
                                .Match<SettingsDetails>()
                                .Some().Do(f => LoadSettingsFromFile(f.FullName))
                                .Else(new SettingsDetails())
                                .Result();
            }

            return TraversePathToFindCollocatedSettings(directory.Parent, settingsFileName, collocatedFile);
        }

        private static SettingsDetails AddGlobalSettings(SettingsDetails solutionSettings, string arnolyzerHome)
        {
            if (solutionSettings.IgnoreArnolyzerHome) { return solutionSettings; }

            var globalSettings = LoadGloablSettingsIfExistsOrDefaultIfNot(arnolyzerHome);
            return SettingsDetails.Merge(solutionSettings, globalSettings);
        }

        private static SettingsDetails LoadGloablSettingsIfExistsOrDefaultIfNot(string arnolyzerHome)
        {
            var path = GetEnvironmentVariable(arnolyzerHome);
            return LoadSettingsFromFile(path);
        }

        private static SettingsDetails LoadSettingsFromFile(string filePath)
        {
            try
            {
                var contents = new StreamReader(filePath);
                var deserializer = new Deserializer();
                return deserializer.Deserialize<SettingsDetails>(contents);
            }
            catch (Exception)
            {
                return new SettingsDetails();
            }
        }
    }
}