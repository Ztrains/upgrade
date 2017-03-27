
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
using Stormpath.SDK.Serialization;

namespace System.Net.Http {
	public class HTTPHandler {

		// TODO Profile Visibility 





		private static IApplication myApp;
		/*static void Main(string[] args) {
			MainAsync().GetAwaiter().GetResult();
		}

		public static async Task MainAsync() {
			var client = Clients.Builder()
				.SetApiKeyFilePath(".\\apiKey.properties")
				.Build();
			var myApp = await client.GetApplications()
				.Where(x => x.Name == "calm-chamber-49049")
				.SingleAsync();
		}
		*/
		/*
		// Initialize Stormpath so the other functions work
		public static async void launchStormPath() {
			string path = "C:\\Users\\Jonathan\\.stormpath\\apiKey.properties";
			var client = Clients.Builder().SetApiKeyFilePath("apiKey.properties")
				.Build();

			//.SetApiKeyId("5ID2J1CY76G8FYBWIS45HAZ1B").SetApiKeySecret("1JzbsC7Eck/28VDdmmWYSLKIlDv3lY/NFMrLHdDSVGQ")
			//	.SetHttpClient(Stormpath.SDK.Http.HttpClients.Create().RestSharpClient())
			//	.SetSerializer(Stormpath.SDK.Serialization.Serializers.Create().JsonNetSerializer())


			myApp = await client.GetApplications()
				.Where(x => x.Name == "calm-chamber-49049")
				.SingleAsync();
			// myApp is an IApplication obj
		}

		// Register account function
		public static async void registerRequest(string first, string last, string email, string password) {
			var user = await myApp.CreateAccountAsync(first, last, email, password);
			// Returns an IAccount obj


			Console.WriteLine("User " + user.FullName + " created");
		}

		// Login function
		public static async void loginRequest(string email, string password) {
			try {
				var loginResult = await myApp.AuthenticateAccountAsync(email, password);
				// Returns an IAuthenticationResult obj
				var loggedInAccount = await loginResult.GetAccountAsync();
				//

				Console.WriteLine("User {0} logged in.", loggedInAccount.FullName);
			}
			catch (ResourceException rex) {
				// rex has Message and DeveloperMessage fields
				Console.WriteLine("Could not log in. Error: " + rex.Message);
			}
		}
		*/






		class Password {
			public string Pass;
		}

		class ResetCode {
			public string Code;
		}

		class Account {
			public string Email;
			public string Password;
		};

		class Profile {
			public string Name;
			public string Email;
			public string Contact;
			public int Rating;
			public string About;
			public string[] ClassesTutor;
			public string[] ClassesStudent;
			public string Time;
			public string Prices;
		};

		class ClassList {
			public string[] Classes;
		}

		class StudentList {
			public string[] Students;
		}



		// current register function
		public static int registerRequest(string email, string password) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/register");
			var request = new RestRequest(Method.POST);

			Account acc = new Account();
			acc.Email = email;
			acc.Password = password;
			string json = JsonConvert.SerializeObject(acc);

			//request.AddHeader("postman-token", "0fccbb68-76d2-f0d9-51f9-c657ce173d67");
			//request.AddHeader("cache-control", "no-cache");
			//request.AddHeader("accept", "application/json");
			//request.AddHeader("content-type", "application/json");
			//string json = "{ \"email\": \"" + email + "\", \"password\": \"" + password + "\" }";

			//request.AddParameter("application/json", json, ParameterType.RequestBody);
			IRestResponse response = client.Execute(request);

			return 1;
		}

		// current login function
		public static int loginRequest(string email, string password) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/login");
			var request = new RestRequest(Method.GET);

			Account acc = new Account();
			acc.Email = email;
			acc.Password = password;
			string json = JsonConvert.SerializeObject(acc);

			//request.AddHeader("postman-token", "0fccbb68-76d2-f0d9-51f9-c657ce173d67");
			//request.AddHeader("cache-control", "no-cache");
			//request.AddHeader("accept", "application/json");
			//request.AddHeader("content-type", "application/json");
			//string json = "{ \"email\": \"" + email + "\", \"password\": \"" + password + "\" }";

			//request.AddParameter("application/json", json, ParameterType.RequestBody);
			IRestResponse response = client.Execute(request);
			Console.WriteLine(response.Content + "\n");

			return 1; // success
		}

		public static void checkPasswordResetCode(string code) {
			var client = new RestClient(); // needs url
			var request = new RestRequest(Method.GET);

			ResetCode resetCode = new ResetCode();
			resetCode.Code = code;

			IRestResponse response = client.Execute(request);
		}

		public static void updatePassword(string password) {
			var client = new RestClient(); // needs url
			var request = new RestRequest(Method.GET);

			Password pass = new Password();
			pass.Pass = password;

			IRestResponse response = client.Execute(request);
		}

		public static void classListRequest() {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/classList");
			var request = new RestRequest(Method.GET);
			IRestResponse response = client.Execute(request);

			//var classes = JsonConvert.DeserializeObject<dynamic>(response.Content);
		}

		public static void studentListRequest(string classToList) {
			var client = new RestClient(); // needs url
			var request = new RestRequest(Method.GET);
			IRestResponse response = client.Execute(request);
		}

		public static void updateProfile(string name, string email, string contact, int rating,
			string about, string[] classesTutor, string[] classesStudent, string time, string prices) {
			var client = new RestClient(); // needs url
			var request = new RestRequest(Method.GET);

			Profile profile = new Profile();
			profile.Name = name;
			profile.Email = email;
			profile.Contact = contact;
			profile.Rating = rating;
			profile.About = about;
			profile.ClassesTutor = classesTutor;
			profile.ClassesStudent = classesStudent;
			profile.Time = time;
			profile.Prices = prices;

			IRestResponse response = client.Execute(request);
		}

		public static void getProfile() {
			var client = new RestClient(); // Needs url
			var request = new RestRequest(Method.GET);

			IRestResponse response = client.Execute(request);
		}

		public static void upvoteProfile() {
			var client = new RestClient(); // Needs url
			var request = new RestRequest(Method.GET);

			IRestResponse response = client.Execute(request);
		}

		public static void reportProfile() {
			var client = new RestClient(); // Needs url
			var request = new RestRequest(Method.GET);

			IRestResponse response = client.Execute(request);
		}

		public static void blockProfile() {
			var client = new RestClient(); // Needs url
			var request = new RestRequest(Method.GET);

			IRestResponse response = client.Execute(request);
		}

		public static void deleteProfile() {
			var client = new RestClient(); // Needs url
			var request = new RestRequest(Method.GET);

			IRestResponse response = client.Execute(request);
		}





		// Are we still going to use these?  I'm not sure
		// I could make a bunch of individual calls to every field, but I'd like to avoid that if possible :S

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
