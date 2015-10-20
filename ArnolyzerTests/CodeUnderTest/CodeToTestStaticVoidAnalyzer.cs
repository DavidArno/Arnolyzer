namespace Arnolyzer.Test.CodeUnderTest
{
    public static class CodeToTestStaticVoidAnalyzer
    {
        public static void DoNothing() { }

        public static bool ReturnTrue() => true;
        public static void StillDoNothing() { }
    }
}
