namespace Arnolyzer.Test.CodeUnderTest
{
    public static class CodeToTestAtLeastOneParameterAnalyzer
    {
        public static bool ReturnTrue() => true;

        public static bool ReturnsSelf(bool flag) => flag;
    }
}
