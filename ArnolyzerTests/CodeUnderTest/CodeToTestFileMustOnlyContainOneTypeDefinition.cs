namespace Arnolyzer.Test.CodeUnderTest
{
    public class Class1
    {
        private class Class2 { }
        private struct Struct1 { }
        private interface Interface1 { }
        private enum Enum1 { True }
    }

    internal struct Struct2
    {
        private class Class3 { }
        private struct Struct3 { }
        private interface Interface2 { }
        private enum Enum2 { Dog }
    }

    public interface Interface3 {}

    internal enum Enum3 { Red }
}
