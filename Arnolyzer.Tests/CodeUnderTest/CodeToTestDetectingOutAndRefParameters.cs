namespace Arnolyzer.Tests.CodeUnderTest
{
    public static class CodeToTestDetectingOutAndRefParameters
    {
        private static int DoesntUseEither(int p)
        {
            p++;
            return p;
        }

        private static int UsesRefParameter(ref int p)
        {
            p += 5;
            return 1;
        }

        private static int UsesOutParameter(out int p)
        {
            p = 5;
            return 1;
        }
    }
}
