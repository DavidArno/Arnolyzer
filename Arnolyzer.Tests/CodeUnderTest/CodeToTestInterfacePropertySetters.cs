using Arnolyzer.RuleExceptionAttributes;

namespace Arnolyzer.Tests.CodeUnderTest
{
    public interface IContainsSetters
    {
        void SomeMethod();
        int Property1 { get; }
        bool Property2 { set; }
        void AnotherMethod();
        int Property3 { get; set; }
        bool Property4 { set; get; }

        [MutableProperty]
        bool Property5 { set; get; }
    }
}
