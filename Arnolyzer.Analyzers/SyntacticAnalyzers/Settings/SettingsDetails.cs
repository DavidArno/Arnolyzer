using System.Collections.Generic;
using System.Linq;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global - Class is configured via serialization
// ReSharper disable MemberCanBePrivate.Global - Class is configured via serialization
namespace Arnolyzer.SyntacticAnalyzers.Settings
{
    public class SettingsDetails
    {
        private string _ignorePathsRegex;

        public SettingsDetails() : this(false, false, new List<string>()) { }

        private SettingsDetails(bool doNotTraverse, bool ignoreArnolyzerHome, IEnumerable<string> ignorePaths)
        {
            DoNotTraverse = doNotTraverse;
            IgnoreArnolyzerHome = ignoreArnolyzerHome;
            IgnorePaths = ignorePaths;
        }

        public bool DoNotTraverse { get; set; }
        public bool IgnoreArnolyzerHome { get; set; }
        public IEnumerable<string> IgnorePaths { get; set; }

        public string IgnorePathsRegex =>
            _ignorePathsRegex ?? (_ignorePathsRegex = GenerateRegexFromRestorePaths(IgnorePaths));

        public static SettingsDetails Merge(SettingsDetails settings1, SettingsDetails settings2)
        {
            return new SettingsDetails(settings1.DoNotTraverse || settings2.DoNotTraverse,
                                       settings1.IgnoreArnolyzerHome || settings2.IgnoreArnolyzerHome,
                                       settings1.IgnorePaths.Concat(settings2.IgnorePaths).ToList());
        }

        private static string GenerateRegexFromRestorePaths(IEnumerable<string> ignorePaths)
        {
            return string.Join("|", ignorePaths.Select(p => $"(.*{ConvertPatternToRegex(p)})"));
        }

        private static string ConvertPatternToRegex(string pattern)
        {
            return pattern.Replace(@"\", @"\\")
                          .Replace("/", @"\.")
                          .Replace(".", @"\.")
                          .Replace("*", ".*")
                          .TrimStart('*');
        }
    }
}