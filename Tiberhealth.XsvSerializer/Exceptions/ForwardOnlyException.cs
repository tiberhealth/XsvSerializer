namespace Tiberhealth.Exceptions
{
    internal class ForwardOnlyException: Exception
    {
        public ForwardOnlyException() : base("Serialization is forward only")
        {
        }

        public ForwardOnlyException(string message, Exception inner) : base("Serialization is forward only", new Exception(message, inner))
        {
        }
    }
}
