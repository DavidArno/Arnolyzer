using Arnolyzer.RuleExceptionAttributes;
using System;

namespace Arnolyzer.Tests.CodeUnderTest
{
    public class CodeToTestBooleanVariableReassignments
    {
        public void F()
        {
            var b = true;
            b ^= true;
            b |= false;
            b &= false;
        }
    }
}