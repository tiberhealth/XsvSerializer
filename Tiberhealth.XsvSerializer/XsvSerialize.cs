using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Tiberhealth.XsvSerializer
{
    public class XsvSerializer
    {
        public XsvSerializer(char delimiter = ',')
        {
        }

        public Stream Serialize<TType>(IEnumerable<TType> collection)
        {
            return null;
        }
    }
}
