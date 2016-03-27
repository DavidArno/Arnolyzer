using System;

namespace Arnolyzer.RuleExceptionAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MutatesParameterAttribute : Attribute, IAttributeDescriber
    {
        public string AttributeDescription =>
            $"A static method annotated with {nameof(MutatesParameterAttribute)} is allowed to be void as it " +
            "is explicitly declaring its intent to return a result through mutating the contents of a parameter.";
    }
}