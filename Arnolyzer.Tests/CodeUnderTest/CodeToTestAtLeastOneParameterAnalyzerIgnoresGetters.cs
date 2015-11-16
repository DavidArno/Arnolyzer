namespace Arnolyzer.Tests.CodeUnderTest
{
    public static class CodeToTestAtLeastOneParameterAnalyzerIgnoresGetters
    {
        public static bool Prop1 => false;

        public static bool Prop2 { get; set; }

        private static bool Prop3 { get { return true; } }

        public static bool ReturnsSelf(bool flag) => flag;
    }
}
