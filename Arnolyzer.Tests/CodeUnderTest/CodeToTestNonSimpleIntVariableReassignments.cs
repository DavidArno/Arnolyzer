using Arnolyzer.RuleExceptionAttributes;
using System;

namespace Arnolyzer.Tests.CodeUnderTest
{
    public class CodeToTestNonSimpleIntVariableReassignments
    {
        [MutableVariable("b")]
        public void F()
        {
            var a = 1;
            a += 1;
            a -= 1;
            a *= 1;
            a /= 1;
            a %= 1;
            a <<= 1;
            a >>= 1;
            a--;
            a++;
            --a;
            ++a;
            var b = 1;
            b += 1;
            b -= 1;
            b *= 1;
            b /= 1;
            b %= 1;
            b <<= 1;
            b >>= 1;
            b--;
            b++;
            --b;
            ++b;
        }
    }
}