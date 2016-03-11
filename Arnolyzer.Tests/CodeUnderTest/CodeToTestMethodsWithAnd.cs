namespace Arnolyzer.Tests.CodeUnderTest
{
    public static class StaticClassWithMethodsContainingAnd
    {
        public static void OKName() { }

        public static void DoSomethingAndSomethingElse() { }
    }

    public class ClassWithMethodsContainingAnd
    {
        public void OKName() { }

        public static void DoSomethingAndSomethingElse() { }

        public void AThingAndAnotherThing() { }

        private struct AndAStruct
        {
            public void AndAndAnd() { }
        }
    }

    public interface InterfaceWithMethodsContainingAnd
    {
        void OKName();
        void AThingAndAnotherThing();
    }
}
