using System;

namespace Arnolyzer.Tests.CodeUnderTest
{
    public class CodeToTestLSPViolatingExceptions
    {
        private NotImplementedException ex1 = new NotImplementedException();
        private NotSupportedException ex2 = new NotSupportedException();

        private NotImplementedException SomeException { get; }
        private NotSupportedException ExProp2 { get; }

        public CodeToTestLSPViolatingExceptions()
        {
            SomeException = new NotImplementedException();
            ExProp2 = new NotSupportedException();
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

        public void Method6()
        {
            throw new NotSupportedException();
        }

        public void Method7()
        {
            throw ex2;
        }

        public void Method8()
        {
            var ex = new NotSupportedException();
            throw ex;
        }

        public void Method9()
        {
            throw ExProp2;
        }
    }
}
