using System;

namespace Arnolyzer.RuleExceptionAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MutablePropertyAttribute : Attribute, IAttributeDescriber
    {
        public string AttributeDescription => 
            "A public setter may sometimes be required. It is therefore allowed if decorated with " +
            $"the {nameof(MutablePropertyAttribute)} as it explicitly asserts the need for the " +
            "property to be mutable.";
    }
}