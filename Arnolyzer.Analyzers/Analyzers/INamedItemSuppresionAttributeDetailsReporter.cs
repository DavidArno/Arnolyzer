using System.Collections.Generic;

namespace Arnolyzer.Analyzers
{
    public interface INamedItemSuppresionAttributeDetailsReporter
    {
        IList<NamedItemSuppresionAttributeDetails> GetNamedItemSuppresionAttributeDetails();
    }
}
