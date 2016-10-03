using Arnolyzer.RuleExceptionAttributes;
using System;

namespace Arnolyzer.Tests.CodeUnderTest
{
    public class CodeToTestParameterReassignment
    {
        private int _f;

        [MutableParameter("y", "z")]
        public void F1(int a, string b)
        {
            a = 2;
            b = "";
            foreach (var c in "123456".ToCharArray())
            {
                var x = c;
                x = ' ';
                _f = 1;
                b = $"{c}{_f}";
            }
            Console.WriteLine($"{a}{b}");
        }

        [MutableParameter("a")]
        public void F2(int a)
        {
            a = 2;
            var b = $"{a}";
        }
    }
}