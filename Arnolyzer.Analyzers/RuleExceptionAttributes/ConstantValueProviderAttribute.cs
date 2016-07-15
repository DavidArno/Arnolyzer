using System;
using static System.Environment;

namespace Arnolyzer.RuleExceptionAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ConstantValueProviderAttribute : Attribute, IAttributeDescriber
    {
        public string AttributeDescription =>
            $"A static method annotated with {nameof(ConstantValueProviderAttribute)} is allowed have no " +
            "parameters as it is explicitly guaranteeing to return the same result every time it's called." +
            $"{NewLine}{NewLine}For example, a static factory method that generates the same shape object " +
            "each time it's called can use this annotation.";
    }
}