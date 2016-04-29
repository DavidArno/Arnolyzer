namespace ArnolyzerDocumentationGenerator
{
    internal class ExtraWordsContents
    {
        public ExtraWordsContents(string cause, string preCodeFix, string postCodeFix, string preSuppression, string postSuppression)
        {
            Cause = cause;
            PreCodeFix = preCodeFix;
            PostCodeFix = postCodeFix;
            PreSuppression = preSuppression;
            PostSuppression = postSuppression;
        }

        internal string Cause { get; }
        internal string PreCodeFix { get; }
        internal string PostCodeFix { get; }
        internal string PreSuppression { get; }
        internal string PostSuppression { get; }
    }
}
