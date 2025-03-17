namespace Tiberhealth.XsvSerializer
{
    internal class XsvReaderEnumerator<TResult>: IEnumerator<TResult>
        where TResult: class, new()
    {
        private XsvReader<TResult> Reader { get; }
        public XsvReaderEnumerator(XsvReader<TResult> reader)
        {
            this.Reader = reader;
            this.Reader.Reset();
        }

        public bool MoveNext()
        {
            this.Current = this.Reader.GetNext();
            return this.Current != null;
        }

        public void Reset() => this.Reader.Reset();

        public TResult Current { get; private set; }
        object IEnumerator.Current => Current;

        public void Dispose()
        {
            
        }
    }
}