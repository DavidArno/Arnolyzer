using System;

namespace Arnolyzer.Tests.CodeUnderTest
{
    public class CodeToTestLSPViolatingExceptions
    {
        private NotImplementedException ex1 = new NotImplementedException();

        private NotImplementedException SomeException { get; }

        public CodeToTestLSPViolatingExceptions()
        {
            SomeException = new NotImplementedException();
        }

        public void Method1()
        {
            throw new NotImplementedException();
        }

        public void Method2()
        {
            throw ex1;
        }

        public void Method3()
        {
            throw new Exception();
        }

        public void Method4()
        {
            var ex = new NotImplementedException();
            throw ex;
        }

        public void Method5()
        {
            throw SomeException;
        }
    }
}
