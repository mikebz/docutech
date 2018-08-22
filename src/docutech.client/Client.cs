using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;


namespace docutech.client
{
    public class Client
    {
		HttpClient client = new HttpClient();

        public Client(string baseAddress)
		{
			client.BaseAddress = new Uri(baseAddress);
		}

		/// <summary>
		/// an import function that will kick off the import process into DocuTech
		/// </summary>
		public async Task<bool> Import(string importXml)
		{
			const string importUrl = "cx4/cxws/enginews.asmx/Import";
			var content = new StringContent(importXml, Encoding.UTF8, "text/xml");
			var result = await client.PostAsync(importUrl, content);
			return result.IsSuccessStatusCode;
		}

		/// <summary>
		/// a basic ping function that calls "Hello World"
		/// </summary>
		public async Task<bool> Ping()
		{
			const string pingUrl = "cx4/cxws/enginews.asmx/HelloWorld";
			const string pingXml = @"
						<?xml version=""1.0"" encoding=""utf-8""?>
						<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
						<soap:Body>
							<HelloWorld xmlns=""http://conformx.docutechcorp.com/ConformXWS"" />
						</soap:Body>
						</soap:Envelope>";

			var content = new StringContent(pingXml, Encoding.UTF8, "text/xml");
			var result = await client.PostAsync(pingUrl, content);
			return result.IsSuccessStatusCode;
		}
    }
}
