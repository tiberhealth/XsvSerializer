using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XsvSerializer.Test
{
    public class CsvTests
    {
        [SetUp]
        public void Setup()
        {
        }

        public class DeserializeCsvTestObject
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public decimal Rate { get; set; }
            public DateTime BirthDate { get; set; }
        }

        private string CsvString = "Name,Age,Rate,BirthDate\n\"Lenihan, Bryan\",49,12.25,06/04/1971\n'Lenihan, Susan',48,17.45,01/12/1972";
            
        [Test]
        public async Task DeserializeCsvBasic()
        {
            await using var memStream = new MemoryStream(Encoding.UTF8.GetBytes(CsvString));
            var returnObj = new Tiberhealth.XsvSerializer.XsvReader<DeserializeCsvTestObject>(memStream); 
            
            Assert.IsNotNull(returnObj, "returnObj ia null");
            Assert.IsNotEmpty(returnObj, "returnObj is empty");
            Assert.AreEqual(2, returnObj.Count(), "Record counts do not match");
        }
    }
}
