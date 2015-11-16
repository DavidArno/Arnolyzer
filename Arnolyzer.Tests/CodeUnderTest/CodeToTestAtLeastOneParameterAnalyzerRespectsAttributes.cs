using System;
using Arnolyzer.RuleExceptionAttributes;

namespace Arnolyzer.Tests.CodeUnderTest
{
    public static class CodeToTestAtLeastOneParameterAnalyzerRespectsAttributes
    {
        public static bool ReturnTrue() => true;

        [HasSideEffects]
        public static bool Is64Bit() => Environment.Is64BitProcess;
    }
}
