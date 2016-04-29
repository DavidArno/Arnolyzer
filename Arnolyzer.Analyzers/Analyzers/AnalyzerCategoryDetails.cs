namespace Arnolyzer.Analyzers
{
    public class AnalyzerCategoryDetails
    {
        public AnalyzerCategoryDetails(string name, string code, string description)
        {
            Name = name;
            Code = code;
            Description = description;
        }

        public string Name { get; }
        public string Code { get; }
        public string Description { get; }
    }
}
