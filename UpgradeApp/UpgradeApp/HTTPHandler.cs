
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace System.Net.Http {

	public class HTTPHandler {
		public HTTPHandler() {
			//
		}

		public static async void loginRequest() {
			string urlServer = "https://calm-chamber-49049.herokuapp.com/register";
			Dictionary<string, string> info = new Dictionary<string, string>();
			info.Add("givenName", "Geo");
			info.Add("surname", "Myers");

			info.Add("email", "geo@memes.dank.com");
			info.Add("password", "lecurbronry");
			string js = JsonConvert.SerializeObject(info);

			//JsonValue json = await FetchJsonAsync(urlServer, js);

		}

		public static int Testfn() {
			//loginRequest();
			RestTestBest();
			Console.WriteLine("12345\n");
			return 1;
		}

		/*
		// This method can throw exceptions !!!!!!!!! (those aren't handled currently)
		private static async Task<JsonValue> FetchJsonAsync(string url, string json) {
			// Create an HTTP web request
			HttpWebRequest request =
				(HttpWebRequest)HttpWebRequest.Create(new Uri(url+json));
			request.ContentType = "application/json";
			request.Method = "POST";
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
		*/

		// current register function
		public static void RestTestBest() {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/register");
			var request = new RestRequest(Method.POST);
			request.AddHeader("postman-token", "0fccbb68-76d2-f0d9-51f9-c657ce173d67");
			request.AddHeader("cache-control", "no-cache");
			request.AddHeader("accept", "application/json");
			request.AddHeader("content-type", "application/json");
			request.AddParameter("application/json", "{ \"givenName\": \"Ryan\", \"surname\": \"Memelord\", \"email\": \"abort@blackbabies.com\", \"password\": \"FantasyBaghd4d\" }", ParameterType.RequestBody);
			IRestResponse response = client.Execute(request);

		}
	}
}
