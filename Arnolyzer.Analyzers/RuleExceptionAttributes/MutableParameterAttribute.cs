using System;
using static System.AttributeTargets;
using static System.Environment;

namespace Arnolyzer.RuleExceptionAttributes
{
    // ReSharper disable UnusedParameter.Local
    // ReSharper disable UnusedMember.Local
    // ReSharper disable UnusedMember.Global
    [AttributeUsage(Method|Property)]
    public class MutableParameterAttribute : Attribute, IAttributeDescriber
    {
        public MutableParameterAttribute(string firstMutableParameter, params string[] subsequentMutableParameters) { }
        private MutableParameterAttribute() { }

        public string AttributeDescription =>
            $"A method annotated with {nameof(MutableParameterAttribute)} is allowed to have parameters that change value (ie can" +
            $"be reassigned. The parameters affected must be named in the {nameof(MutableParameterAttribute)} attribute, eg" +
            $"{NewLine}````cs{NewLine}" +
            $"[{nameof(MutableParameterAttribute)}(\"p1\")]{NewLine}" +
            $"public void F(int p1, int p2){NewLine}{{{NewLine}" +
            $"    p1 = 1;{NewLine}p2 = 2;{NewLine}}}{NewLine}````{NewLine}" +
            $"would result in no errors over the reassignment of `p1`.{NewLine}" +
            $"Multiple variables can be supplied to the attribute, eg [{nameof(MutableParameterAttribute)}(\"p1\", \"p2\")]";
    }
}