
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using Stormpath.SDK.Client;
using Stormpath.SDK.Error;
using Stormpath.SDK;
using Stormpath.SDK.Application;

namespace System.Net.Http {
	public class HTTPHandler {

		private static IApplication myApp;

		// Initialize Stormpath so the other functions work
		public static async void launchStormPath() {
			//string path = Directory.GetCurrentDirectory() + "keys.txt";
			//Console.WriteLine(path);
			var client = Clients.Builder()
				//.SetApiKeyFilePath()
				//.SetApiKeyId("5ID2J1CY76G8FYBWIS45HAZ1B") //Redacted for push
				//.SetApiKeySecret("1JzbsC7Eck/28VDdmmWYSLKIlDv3lY/NFMrLHdDSVGQ") //Redacted for push
				.Build();
			myApp = await client.GetApplications()
				.Where(x => x.Name == "My Application")
				.SingleAsync();
		}

		// Register account function
		public static async void registerRequest(string first, string last, string email, string password) {
			var user = await myApp.CreateAccountAsync(first, last, email, password);
			Console.WriteLine("User " + user.FullName + " created");
		}

		// Login function
		public static async void loginRequest(string email, string password) {
			try {
				var loginResult = await myApp.AuthenticateAccountAsync(email, password);
				var loggedInAccount = await loginResult.GetAccountAsync();

				Console.WriteLine("User {0} logged in.", loggedInAccount.FullName);
			}
			catch (ResourceException rex) {
				Console.WriteLine("Could not log in. Error: " + rex.Message);
			}
		}

		public static void classListRequest() {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/classList");
			var request = new RestRequest(Method.GET);
			IRestResponse response = client.Execute(request);

			//var classes = JsonConvert.DeserializeObject<dynamic>(response.Content);
			
		}

		public static void updateProfile(string name, string email, string contact, string time, string price) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/classList");
			var request = new RestRequest(Method.GET);
			IRestResponse response = client.Execute(request);
			//
		}

		public static void changeEmail(string email) {
			string url = "https://calm-chamber-49049.herokuapp.com/email/";
			url += email;
			var client = new RestClient(url);
			var request = new RestRequest(Method.GET);
			IRestResponse response = client.Execute(request);
			//
		}

		public static void changeFirstName(string name) {
			string url = "https://calm-chamber-49049.herokuapp.com/name/";
			url += name;
			var client = new RestClient(url);
			var request = new RestRequest(Method.GET);
			IRestResponse response = client.Execute(request);
			//
		}




		/*
		// current register function
		public static int registerRequest(string email, string password) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/register");
			var request = new RestRequest(Method.POST);
			//request.AddHeader("postman-token", "0fccbb68-76d2-f0d9-51f9-c657ce173d67");
			//request.AddHeader("cache-control", "no-cache");
			request.AddHeader("accept", "application/json");
			request.AddHeader("content-type", "application/json");
			string json = "{ \"email\": \"" + email + "\", \"password\": \"" + password + "\" }";

			request.AddParameter("application/json", json, ParameterType.RequestBody);
			IRestResponse response = client.Execute(request);

			return 1;

		}

		// current login function
		public static int loginRequest(string email, string password) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/login");
			var request = new RestRequest(Method.GET);
			//request.AddHeader("postman-token", "0fccbb68-76d2-f0d9-51f9-c657ce173d67");
			//request.AddHeader("cache-control", "no-cache");
			request.AddHeader("accept", "application/json");
			request.AddHeader("content-type", "application/json");
			string json = "{ \"email\": \"" + email + "\", \"password\": \"" + password + "\" }";

			request.AddParameter("application/json", json, ParameterType.RequestBody);
			IRestResponse response = client.Execute(request);
			Console.WriteLine(response.Content+"\n");

			return 1; // success

		}
		*/





		/*
		public static async void loginRequest(string email, string password) {
			string urlServer = "https://calm-chamber-49049.herokuapp.com/register";
			Dictionary<string, string> info = new Dictionary<string, string>();
			info.Add("email", email);
			info.Add("password", password);
			string js = JsonConvert.SerializeObject(info);

			//JsonValue json = await FetchJsonAsync(urlServer, js);
		}

		public static async void registerRequest(string email, string password) {
			string urlServer = "https://calm-chamber-49049.herokuapp.com/register";
			Dictionary<string, string> info = new Dictionary<string, string>();
			info.Add("email", email);
			info.Add("password", password);
			string js = JsonConvert.SerializeObject(info);
		}

		public static int Testfn() {
			//loginRequest();
			RestTestBest();
			Console.WriteLine("12345\n");
			return 1;
		}
		*/

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


	}
	}
