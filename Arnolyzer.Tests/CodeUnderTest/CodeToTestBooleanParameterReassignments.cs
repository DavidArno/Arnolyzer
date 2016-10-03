using Arnolyzer.RuleExceptionAttributes;
using System;

namespace Arnolyzer.Tests.CodeUnderTest
{
    public class CodeToTestBooleanParameterReassignments
    {
        public void F(bool b)
        {

            b ^= true;
            b |= false;
            b &= false;
        }
    }
}