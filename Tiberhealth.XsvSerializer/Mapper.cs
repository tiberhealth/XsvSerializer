using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tiberhealth.XsvSerializer
{
    internal class Mapper
    {
        public long BuildMap(Stream inputStream)
        {
            using var reader =  new StreamReader(inputStream, leaveOpen: true);;
            var headerLine = reader.ReadLine();

            return headerLine.Length;
        }
    }
}
