using System;

namespace Arnolyzer.RuleExceptionAttributes
{
    /// <summary>
    /// A static method annotated with the MutatesParameter is allowed to be void as it's explicitly declaring its
    /// intent to return a result through mutating the contents of a parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class MutatesParameterAttribute : Attribute { }
}