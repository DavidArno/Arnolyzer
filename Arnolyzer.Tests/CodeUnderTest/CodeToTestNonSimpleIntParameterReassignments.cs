using Arnolyzer.RuleExceptionAttributes;
using System;

namespace Arnolyzer.Tests.CodeUnderTest
{
    public class CodeToTestNonSimpleIntParameterReassignments
    {
        [MutableParameter("b")]
        public void F(int a, int b)
        {

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