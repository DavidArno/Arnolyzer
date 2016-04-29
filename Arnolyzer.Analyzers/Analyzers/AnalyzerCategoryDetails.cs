namespace Arnolyzer.Analyzers
{
    public class AnalyzerCategoryDetails
    {
        public AnalyzerCategoryDetails(string name, string code)
        {
            Name = name;
            Code = code;
        }

        public string Name { get; }
        public string Code { get; }
    }
}
