namespace Arnolyzer.Tests.CodeUnderTest
{
    public static class CodeThatShouldBeIgnored
    {
        public static void DoNothing() { }

        public static bool ReturnTrue() => true;

        public static void StillDoNothing() { }
    }
}