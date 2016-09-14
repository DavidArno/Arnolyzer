using Arnolyzer.RuleExceptionAttributes;

namespace Arnolyzer.Tests.CodeUnderTest
{
    public class CodeToTestAvoidUsingStaticFieldsAndProperties
    {
        public int A1;
        public static int A2;
        public static readonly int A3;
        [GlobalState] public static int A4;
        private int B1;
        private static int B2;
        private static readonly int B3 = 1;
        [GlobalState] private static int B4;
        internal int C1;
        internal static int C2;
        internal static readonly int C3 = 1;
        [GlobalState] internal static int C4;
        protected int D1;
        protected static int D2;
        protected static readonly int D3;
        [GlobalState] protected static int D4;

        public int BP1 { get { return B1; } set { B1 = value; } }
        public int BP2 { get { return B2; } set { B2 = value; } }
        public int BP3 => B3;
        public int BP4 { get { return B4; } set { B4 = value; } }
        public int CP1 { get { return C1; } set { C1 = value; } }
        public int CP2 { get { return C2; } set { C2 = value; } }
        public int CP3 => C3;
        public int CP4 { get { return C4; } set { C4 = value; } }

        public static int PropA1 { get { return A2; } set { A2 = value; } }
        internal static int PropB1 { get { return B2; } set { B2 = value; } }
        private static int PropC1 { get { return C2; } set { C2 = value; } }
        protected static int PropD1 { get { return D2; } set { D2 = value; } }

        public static int PropA2 => A2;
        internal static int PropB2 => B2;
        private static int PropC2 => C2;
        protected static int PropD2 => D2;

        [GlobalState] public static int PropA3 { get { return A2; } set { A2 = value; } }
        [GlobalState] internal static int PropB3 { get { return B2; } set { B2 = value; } }
        [GlobalState] private static int PropC3 { get { return C2; } set { C2 = value; } }
        [GlobalState] protected static int PropD3 { get { return D2; } set { D2 = value; } }
    }
}
