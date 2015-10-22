namespace Arnolyzer.Test.DiagnosticVerification
{
    public struct DiagnosticResultLocation
    {
        public DiagnosticResultLocation(string fileName, int line, int startColumn, int endColumn)
        {
            FileName = fileName;
            Line = line;
            StartColumn = startColumn;
            EndColumn = endColumn;
        }

        public string FileName { get; }
        public int Line { get; }
        public int StartColumn { get; }
        public int EndColumn { get; }
    }
}