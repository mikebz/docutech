using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Specialized;
using Microsoft.Extensions.Logging;


namespace docutech.client
{
    public class Client
    {
		HttpClient client = new HttpClient();
		private readonly ILogger<Client> _logger;

        public Client(ILogger<Client> logger)
		{
			_logger = logger;
		}

		// <summary>
		// set the base address of the Client.
		// </summary>
		public string BaseAddress 
		{
			set  { client.BaseAddress = new Uri(value); }
			get { return client.BaseAddress.AbsoluteUri; }
		}

		/// <summary>
		/// the main function that will kick off the process into DocuTech
		/// </summary>
		public async Task<bool> Process(string xml, string baseAddress = null)
		{
			BaseAddress = baseAddress ?? BaseAddress;
			
			const string processUrl = "cx4/cxws/enginews.asmx";
			
			var encoded = WebUtility.UrlEncode(xml);
			var requestBody = $@"<?xml version=""1.0"" encoding=""utf-8""?>
				<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
				<soap:Body>
					<Process xmlns=""http://conformx.docutechcorp.com/ConformXWS"">
					<xmlRequest>{encoded}</xmlRequest>
					</Process>
				</soap:Body>
				</soap:Envelope>";

			var content = new StringContent(requestBody);
			content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
			content.Headers.ContentEncoding.Add("UTF-8");
			content.Headers.Add("SOAPAction", "http://conformx.docutechcorp.com/ConformXWS/Process");

			var result = await client.PostAsync(processUrl, content);
			
			// TODO: this should not be logged in the production version.
			var rbody = await result.Content.ReadAsStringAsync();
			_logger.LogInformation(rbody);

			return result.IsSuccessStatusCode;
		}

		/// <summary>
		/// a basic ping function that calls "Hello World"
		/// </summary>
		public async Task<bool> Ping(string baseAddress = null)
		{
			BaseAddress = baseAddress ?? BaseAddress;

			const string pingUrl = "cx4/cxws/enginews.asmx/HelloWorld";
			const string pingXml = @"
						<?xml version=""1.0"" encoding=""utf-8""?>
						<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
						<soap:Body>
							<HelloWorld xmlns=""http://conformx.docutechcorp.com/ConformXWS"" />
						</soap:Body>
						</soap:Envelope>";

			var content = new StringContent(pingXml, Encoding.UTF8, "text/xml");
			content.Headers.Add("SOAPAction", "http://conformx.docutechcorp.com/ConformXWS/HelloWorld");
			var result = await client.PostAsync(pingUrl, content);
			return result.IsSuccessStatusCode;
		}
    }
}
