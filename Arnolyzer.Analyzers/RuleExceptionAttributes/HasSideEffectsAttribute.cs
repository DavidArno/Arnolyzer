using System;

namespace Arnolyzer.RuleExceptionAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HasSideEffectsAttribute : Attribute, IAttributeDescriber
    {
        public string AttributeDescription =>
            $"A static method annotated with {nameof(HasSideEffectsAttribute)} is allowed to be void and have no " +
            "parameters as it is explicitly declaring its intent to cause side effects.";
    }
}