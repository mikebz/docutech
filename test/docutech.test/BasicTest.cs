using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using docutech.client;


namespace docutech.test
{
    [TestClass]
    public class BasicTest
    {
        private const string STAGE_URL = "https://stage.conformx.com";
        public TestContext TestContext {get;set;}
        private ILoggerFactory _loggerFactory;

        [TestInitialize]
        public void Init()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();

            _loggerFactory = serviceProvider.GetService<ILoggerFactory>().AddConsole().AddDebug();
        }

        [TestMethod]
        public async Task TestPing()
        {
            var logger = _loggerFactory.CreateLogger<Client>();
            var client = new Client(logger);
            var result = await client.Ping(STAGE_URL);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task TestProcess()
        {
            var logger = _loggerFactory.CreateLogger<Client>();
            var client = new Client(logger);

            using (StreamReader stream = new StreamReader("test_data/init_disc_lights_out.xml"))
            {
                string importXml = stream.ReadToEnd();
                TestContext.WriteLine($"XML file content: {importXml.Substring(0, 60)}...");

                var result = await client.Process(importXml, STAGE_URL);
                Assert.IsTrue(result);
            }
        }
    }
}
