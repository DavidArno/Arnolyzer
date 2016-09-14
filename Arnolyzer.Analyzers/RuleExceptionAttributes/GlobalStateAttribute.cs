using System;
using static System.AttributeTargets;

namespace Arnolyzer.RuleExceptionAttributes
{
    [AttributeUsage(Field|Property)]
    public class GlobalStateAttribute : Attribute, IAttributeDescriber
    {
        public string AttributeDescription =>
            $"A field or property annotated with {nameof(GlobalStateAttribute)} is allowed to be static as it is " +
            "explicitly declaring its need to be \"globally\" accessible.";
    }
}