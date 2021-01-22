using System;
namespace Tiberhealth.XsvSerializer.Exceptions
{
    public class FieldNotDefinedException: Exception
    {
        public FieldNotDefinedException(string fieldName, Exception innerException = null) :
            base($"Field {fieldName} not defined in object", innerException)
        {

        }
    }
}
