namespace Arnolyzer.Analyzers
{
    public static class AnalyzerCategories
    {
        public static readonly AnalyzerCategoryDetails PureFunctionAnalyzers = 
            new AnalyzerCategoryDetails("Pure-Function Analyzers", "AA10nn");

        public static readonly AnalyzerCategoryDetails EncapsulationAndImmutabilityAnalyzers =
            new AnalyzerCategoryDetails("Encapsulation and Immutability Analyzers", "AA11nn");

        public static readonly AnalyzerCategoryDetails GlobalStateAnalyzers =
            new AnalyzerCategoryDetails("Global State Analyzers", "AA12nn");

        public static readonly AnalyzerCategoryDetails LiskovSubstitutionAnalyzers =
            new AnalyzerCategoryDetails("Liskov Substitution Principle Analyzers", "AA20nn");

        public static readonly AnalyzerCategoryDetails SingleResponsibiltyAnalyzers = 
            new AnalyzerCategoryDetails("Single Responsibilty Analyzers", "AA21nn");
    }
}