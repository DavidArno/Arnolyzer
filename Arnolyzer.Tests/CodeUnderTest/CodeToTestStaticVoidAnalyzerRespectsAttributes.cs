using System;
using Arnolyzer.RuleExceptionAttributes;

namespace Arnolyzer.Tests.CodeUnderTest
{
    public static class CodeToTestStaticVoidAnalyzerRespectsAttributes
    {
        public static void DoNothing1() { }

        [MutatesParameter]
        public static void DoNothing2() { }

        [HasSideEffects]
        public static void DoNothing3() { }

        [Obsolete]
        public static void DoNothing4() { }
    }
}
