namespace Arnolyzer.Tests.CodeUnderTest
{
    public class ContainsSetters
    {
        private bool _property7;
        public void SomeMethod() { }
        public int Property1 { get; }
        public bool Property2 { get; set; }
        public void AnotherMethod() { }
        public int Property3 { get; private set; }
        public int Property4 { get; internal set; }
        internal bool Property5 { set; get; }
        private bool Property6 { set; get; }

        public bool Property7
        {
            get { return _property7; }
            set { _property7 = value; }
        }
    }
}
