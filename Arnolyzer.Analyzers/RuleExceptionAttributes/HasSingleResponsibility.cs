using System;
using static System.AttributeTargets;

namespace Arnolyzer.RuleExceptionAttributes
{
    [AttributeUsage(Method)]
    public class HasSingleResponsibility : Attribute, IAttributeDescriber
    {
        public string AttributeDescription =>
            $"A method annotated with {nameof(HasSingleResponsibility)} is allowed to contain \"And\" as " +
            "it is explicitly guaranteeing that it only has one responsibility.";
    }
}