using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.IO;

using docutech.client;


namespace docutech.test
{
    [TestClass]
    public class BasicTest
    {
        private const string STAGE_URL = "https://stage.conformx.com";
        public TestContext TestContext {get;set;}

        [TestMethod]
        public async Task TestPing()
        {
            var client = new Client(STAGE_URL);
            var result = await client.Ping();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task TestImport()
        {
            var client = new Client(STAGE_URL);
            using (StreamReader stream = new StreamReader("test_data/init_disc_lights_on.xml"))
            {
                string importXml = stream.ReadToEnd();
                TestContext.WriteLine($"XML file content: {importXml.Substring(0, 60)}...");

                var result = await client.Import(importXml);
                Assert.IsTrue(result);
            }
        }
    }
}
