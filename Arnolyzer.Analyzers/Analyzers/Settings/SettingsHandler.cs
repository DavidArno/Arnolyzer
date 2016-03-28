using System;
using System.Collections.Immutable;
using System.IO;
using Microsoft.CodeAnalysis;
using SuccincT.Options;
using YamlDotNet.Serialization;

namespace Arnolyzer.Analyzers.Settings
{
    public class SettingsHandler
    {
        private readonly string _settingsFileName;
        private readonly string _arnolyzerHome;
        private readonly ImmutableArray<AdditionalText> _additionalFiles;

        private SettingsDetails _mergedSettings;

        public SettingsHandler(string settingsFileName,
                               string arnolyzerHome,
                               ImmutableArray<AdditionalText> additionalFiles)
        {
            _settingsFileName = settingsFileName;
            _arnolyzerHome = arnolyzerHome;
            _additionalFiles = additionalFiles;
        }

        public SettingsDetails GetArnolyzerSettingsForProject(string filePath)
        {
            if (_mergedSettings != null) { return _mergedSettings; }

            var projectSettings = GetProjectSpecificSettings(_additionalFiles, _settingsFileName);
            var solutionSettings = AddSolutionWideSettings(filePath, projectSettings, _settingsFileName);
            _mergedSettings = AddGlobalSettings(solutionSettings, _arnolyzerHome);
            return _mergedSettings;
        }

        private static SettingsDetails GetProjectSpecificSettings(ImmutableArray<AdditionalText> additionalFiles,
                                                                  string settingsFileName)
        {
            return LoadProjectSettingsIfExistsOrDefaultIfNot(additionalFiles, settingsFileName);
        }

        private static SettingsDetails LoadProjectSettingsIfExistsOrDefaultIfNot(ImmutableArray<AdditionalText> files,
                                                                                 string settingsFileName)
        {
            return files.FirstOrNone(f => f.Path.EndsWith(settingsFileName))
                        .Match<SettingsDetails>()
                        .Some().Do(f => LoadSettingsFromFile(f.Path))
                        .Else(new SettingsDetails())
                        .Result();
        }

        private static SettingsDetails AddSolutionWideSettings(string filePath,
                                                               SettingsDetails projectSettings,
                                                               string settingsFileName)
        {
            if (projectSettings.DoNotTraverse) { return projectSettings; }

            var solutionSettings = LoadSolutionSettingsIfExistsOrDefaultIfNot(filePath, settingsFileName);
            return SettingsDetails.Merge(projectSettings, solutionSettings);
        }

        private static SettingsDetails LoadSolutionSettingsIfExistsOrDefaultIfNot(string filePath,
                                                                                  string settingsFileName)
        {
            var directory = new FileInfo(filePath).Directory;
            do
            {
                var solutionFile = directory.GetFiles().FirstOrNone(f => f.Name.EndsWith(".sln"));
                if (solutionFile.HasValue)
                {
                    return directory.GetFiles().FirstOrNone(f => f.Name.EndsWith(settingsFileName))
                                    .Match<SettingsDetails>()
                                    .Some().Do(f => LoadSettingsFromFile(f.FullName))
                                    .Else(new SettingsDetails())
                                    .Result();
                }

                directory = directory.Parent;
            } while (directory != null);

            return new SettingsDetails();
        }

        private static SettingsDetails AddGlobalSettings(SettingsDetails solutionSettings, string arnolyzerHome)
        {
            if (solutionSettings.IgnoreArnolyzerHome) { return solutionSettings; }

            var globalSettings = LoadGloablSettingsIfExistsOrDefaultIfNot(arnolyzerHome);
            return SettingsDetails.Merge(solutionSettings, globalSettings);
        }

        private static SettingsDetails LoadGloablSettingsIfExistsOrDefaultIfNot(string arnolyzerHome)
        {
            var path = Environment.GetEnvironmentVariable(arnolyzerHome);
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