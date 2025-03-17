namespace Tiberhealth.XsvSerializer
{
    [AttributeUsage(AttributeTargets.Class)]
    public class XsvObjectAttribute: Attribute
    {
        public char Delimiter { get; private set; }

        public XsvObjectAttribute(char delimiter) =>
            this.Delimiter = delimiter;

    }
}
