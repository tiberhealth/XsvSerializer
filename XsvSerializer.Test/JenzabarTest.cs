using System;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Tiberhealth.XsvSerializer;
using XsvSerializer.Test.Models.JenzTest;

namespace XsvSerializer.Test
{
    public class JenzabarTest
    {
        [Test]
        public void TestJenzabarFile()
        {
            using var fileStream = File.OpenRead($"/Users/bryanlenihan/TiberProjects/WorkTemp/JenzImports/jenz_tiber_md_msms_20201210-155624.txt");

            var reader = new XsvReader<JenzObject>(fileStream, '|');
            foreach (var lineObject in reader)
            {
                Assert.IsNotNull(lineObject);
                Assert.IsInstanceOf<JenzObject>(lineObject);
            }


        }
    }
}
