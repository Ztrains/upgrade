
using System.IO;
using System.Json;
using System.Threading.Tasks;

namespace System.Net.Http {

	public class HTTPHandler {
		public static async void testFN() {
			string urlServer = "https://calm-chamber-49049.herokuapp.com";
			JsonValue json = await FetchJsonAsync(urlServer);
		}

		// This method can throw exceptions !!!!!!!!! (those aren't handled currently)
		private static async Task<JsonValue> FetchJsonAsync(string url) {
			// Create an HTTP web request
			HttpWebRequest request =
				(HttpWebRequest)HttpWebRequest.Create(new Uri(url));
			request.ContentType = "application/json";
			request.Method = "GET";
			// Send the request to the server and wait for the response
			using (WebResponse response = await request.GetResponseAsync()) {
				// Get a stream representation of the HTTP web response
				using (Stream stream = response.GetResponseStream()) {
					// Use this stream to build a JSON doc obj
					JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
					Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());
					// Return the JSON doc:
					return jsonDoc;
				}
			}
		}
	}
}
