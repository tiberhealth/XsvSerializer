using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiberhealth.XsvSerializer;

namespace XsvSerializer.Test
{
    public class CsvTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [XsvObject('\t')]
        public class DeserializeCsvTestObject
        {
            [JsonProperty("Name")]
            public string StudentName { get; set; }

            [System.Text.Json.Serialization.JsonPropertyName("Age")]
            public int StudentAge { get; set; }

            [Xsv("Rate")]
            public decimal HourlyRate { get; set; }
            public DateTime BirthDate { get; set; }
        }

        private string CsvString = "Name,Age,Rate,BirthDate\n\"Lenihan, Bryan\",49,12.25,06/04/1971\n\"Lenihan, Susan\",48,17.45,01/12/1972";
        private string TsvString = "Name\tAge\tRate\tBirthDate\n\"Lenihan, Bryan\"\t49\t12.25\t06/04/1971\n\"Lenihan\tSusan\"\t48\t17.45\t01/12/1972";

        [Test]
        public async Task DeserializeCsvBasic()
        {
            await using var memStream = new MemoryStream(Encoding.UTF8.GetBytes(CsvString));
            var returnObj = new Tiberhealth.XsvSerializer.XsvReader<DeserializeCsvTestObject>(memStream, ','); 
            
            Assert.IsNotNull(returnObj, "returnObj ia null");
            Assert.IsNotEmpty(returnObj, "returnObj is empty");
            Assert.AreEqual(2, returnObj.Count(), "Record counts do not match");

            Assert.AreEqual("Lenihan, Bryan", returnObj.First().StudentName);
            Assert.AreEqual("Lenihan, Susan", returnObj.Last().StudentName);

            Assert.AreEqual(49, returnObj.First().StudentAge);
            Assert.AreEqual(12.25, returnObj.First().HourlyRate);

            Assert.AreEqual(DateTime.Parse("01/12/1972"), returnObj.Last().BirthDate);
        }

        [Test]
        public async Task DeserializeTsvBasic()
        {
            await using var memStream = new MemoryStream(Encoding.UTF8.GetBytes(TsvString));
            var returnObj = new Tiberhealth.XsvSerializer.XsvReader<DeserializeCsvTestObject>(memStream);

            Assert.IsNotNull(returnObj, "returnObj ia null");
            Assert.IsNotEmpty(returnObj, "returnObj is empty");
            Assert.AreEqual(2, returnObj.Count(), "Record counts do not match");

            Assert.AreEqual("Lenihan, Bryan", returnObj.First().StudentName);
            Assert.AreEqual("Lenihan\tSusan", returnObj.Last().StudentName);

            Assert.AreEqual(49, returnObj.First().StudentAge);
            Assert.AreEqual(12.25, returnObj.First().HourlyRate);

            Assert.AreEqual(DateTime.Parse("01/12/1972"), returnObj.Last().BirthDate);
        }


    }
}
