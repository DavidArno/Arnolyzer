namespace Arnolyzer.Tests.CodeUnderTest
{
    public class CodeThatShouldBeIgnored
    {
        public static void DoNothing() { }

        public static bool ReturnTrue() => true;

        public static void AndStillDoNothing() { }

        public int Prop1 { get; set; }
    }
}