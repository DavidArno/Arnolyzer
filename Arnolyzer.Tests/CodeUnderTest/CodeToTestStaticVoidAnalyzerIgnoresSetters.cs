namespace Arnolyzer.Tests.CodeUnderTest
{
    public static class CodeToTestStaticVoidAnalyzerIgnoresSetters
    {
        public static bool ReturnTrue() => true;

        public static bool Prop1 => false;

        public static bool Prop2 { get; set; }

        private static bool Prop3 { set { ReturnsSelf(value); } }

        private static bool ReturnsSelf(bool x) => x;
    }
}
