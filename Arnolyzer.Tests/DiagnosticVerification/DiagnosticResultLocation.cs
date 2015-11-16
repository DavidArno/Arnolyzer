namespace Arnolyzer.Tests.DiagnosticVerification
{
    public struct DiagnosticLocation
    {
        public DiagnosticLocation(int line, int startColumn, int endColumn)
        {
            Line = line;
            StartColumn = startColumn;
            EndColumn = endColumn;
        }

        public int Line { get; }

        public int StartColumn { get; }

        public int EndColumn { get; }
    }
}