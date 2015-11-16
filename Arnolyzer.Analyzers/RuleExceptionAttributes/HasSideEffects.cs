using System;

namespace Arnolyzer.RuleExceptionAttributes
{
    /// <summary>
    /// A static method annotated with the HasSideEffects is allowed to be void and/or have no parameters as it's
    /// explicitly declaring its intent to cause side effects.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class HasSideEffectsAttribute : Attribute { }
}