
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
using System;
using System.Net.Http;
using System.Diagnostics;
using System.Net;

namespace UpgradeApp {
	public class HTTPHandler {

		public static string emailLoggedIn;
		public static CookieContainer cookieJar;


		// current register function
		public static int registerRequest(string email, string password) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/reg");
			var request = new RestRequest(Method.POST);

			Account acc = new Account();
			acc.email = email;
			acc.password = password;
			//string json = JsonConvert.SerializeObject(acc);

			request.AddJsonBody(acc);
			//request.AddParameter("application/json", json, ParameterType.RequestBody);
			IRestResponse response = client.Execute(request);
			//Console.WriteLine(response.Content);
			string r = response.Content;
			if (r.Equals("Database error"))
				return -2;
			else if (r.Equals("User exists"))
				return -3;
			else if (r.Equals("Success adding user"))
				return 1;
			else return -1;
		}

		public static void testLogin() {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/testlogin");
			var request = new RestRequest(Method.POST);
			IRestResponse r = client.Execute(request);
		}

		// current login function
		public static int loginRequest(string email, string password) {
			cookieJar = new CookieContainer();

			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/login");
			var request = new RestRequest(Method.POST);
			
			client.CookieContainer = cookieJar;

			Account acc = new Account();
			acc.email = email;
			acc.password = password;
			//string json = JsonConvert.SerializeObject(acc);

			//request.AddHeader("postman-token", "0fccbb68-76d2-f0d9-51f9-c657ce173d67");
			//request.AddHeader("cache-control", "no-cache");
			//request.AddHeader("accept", "application/json");
			//request.AddHeader("content-type", "application/json");
			//string json = "{ \"email\": \"" + email + "\", \"password\": \"" + password + "\" }";
			request.AddJsonBody(acc);
			//request.AddParameter("application/json", json, ParameterType.RequestBody);
			IRestResponse response = client.Execute(request);
			//Console.WriteLine(response.Content + "\n");
			if (response.Content.Equals("you have authenticated properly")) {
				return 1;
			}
			else if (response.Content.Equals("Incorrect email/password combo")) {
				return -2;
			}
			else return -1;	
		}

		public static void setRecovery(string question, string answer) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/setRecovery"); 
			var request = new RestRequest(Method.POST);

			RecoveryInfo ri = new RecoveryInfo();
			ri.email = emailLoggedIn;
			ri.question = question;
			ri.answer = answer;
			request.AddJsonBody(ri);

			IRestResponse response = client.Execute(request);
		}

		public static Question getRecoveryQuestion(string email) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/getQuestion");
			var request = new RestRequest(Method.POST);

			WhichProfile wp = new WhichProfile();
			wp.email = email;
			request.AddJsonBody(wp);
			IRestResponse response = client.Execute(request);
			Question question = JsonConvert.DeserializeObject<Question>(response.Content); 
			return question;
		}

		public static string checkRecoveryAnswer(string email, string answer) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/doRecovery");
			var request = new RestRequest(Method.POST);

			RecoveryCheck rc = new RecoveryCheck();
			rc.email = emailLoggedIn;
			rc.answer = answer;
			request.AddJsonBody(rc);

			IRestResponse response = client.Execute(request);
			return response.Content;
		}

		
		public static string updatePassword(string password, string newPassword, string recovered) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/changePassword");
			var request = new RestRequest(Method.POST);
			client.CookieContainer = cookieJar;
			PasswordReset pr = new PasswordReset();
			pr.password = password;
			pr.newpassword = newPassword;
			pr.recovered = recovered;
			request.AddJsonBody(pr);
			IRestResponse response = client.Execute(request);
			return response.Content;
		}

		public static ClassList classListRequest() {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/classList");
			var request = new RestRequest(Method.GET);
			IRestResponse response = client.Execute(request);
			
			ClassList classes = JsonConvert.DeserializeObject<ClassList>(response.Content);
			//ClassList classes = new ClassList();
			//classes.classes = response.Content;
			return classes;
		}

		public static StudentList studentListRequest(string classToList) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/studentsInClass"); 
			var request = new RestRequest(Method.POST);
			WhichClass wc = new WhichClass();
			wc.className = classToList;
			request.AddJsonBody(wc);

			IRestResponse response = client.Execute(request);
			//Debug.WriteLine("\n\n\n\n\n\n\n\n\n\n\n a a a a a a a a a a a a a a a a a a \n\n\n\n\n\n\n\n\n\n");
			//Debug.WriteLine(response.Content);

			StudentList students = JsonConvert.DeserializeObject<StudentList>(response.Content);
			return students;
		}

		public static void joinClass(string className, string type) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/joinClass");
			var request = new RestRequest(Method.POST);
			client.CookieContainer = cookieJar;
			joinClass jc = new joinClass();
			jc.className = className;
			jc.type = type;
			request.AddJsonBody(jc);

			IRestResponse response = client.Execute(request);
		}

		public static void leaveClass(string className, string type) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/leaveClass");
			var request = new RestRequest(Method.POST);
			client.CookieContainer = cookieJar;
			joinClass jc = new joinClass();
			jc.className = className;
			jc.type = type;
			request.AddJsonBody(jc);

			IRestResponse response = client.Execute(request);
		}

		public static void updateProfile(string name, string email, string contact, string about,
			string[] classesTutor, string[] classesStudent, string time, string prices, string visible, string avatar) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/updateProfile"); 
			var request = new RestRequest(Method.POST);

			Profile profile = new Profile();
			profile.name = name;
			if (!email.Equals(emailLoggedIn) && email != "") {
				profile.email = emailLoggedIn;
				profile.newemail = email;
			}
			else {
				profile.email = email;
			}
			profile.contact = contact;
			profile.about = about;
			//profile.classesTutor = classesTutor;
			//profile.classesStudent = classesStudent;
			profile.time = time;
			profile.price = prices;
			profile.visible = visible;
			profile.avatar = avatar;

			request.AddJsonBody(profile);

			IRestResponse response = client.Execute(request);
		}

		public static Profile getProfile(string email) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/retrieveLogin");
			var request = new RestRequest(Method.POST);

			WhichProfile wp = new WhichProfile();
			wp.email = email;
			request.AddJsonBody(wp);

			IRestResponse response = client.Execute(request);
			Profile profile = JsonConvert.DeserializeObject<Profile>(response.Content);
			return profile;
		}

		public static Profile getProfileByName(string name) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/retrieveProfile");
			var request = new RestRequest(Method.POST);

			WhichStudent ws = new WhichStudent();
			ws.name = name;
			request.AddJsonBody(ws);

			IRestResponse response = client.Execute(request);
			
			Profile profile = JsonConvert.DeserializeObject<Profile>(response.Content);
			return profile;
		}

		public static void upvoteProfile(string name) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/upvote");
			client.CookieContainer = cookieJar;
			var request = new RestRequest(Method.POST);
			SendUpvote su = new SendUpvote();
			su.name = name;
			su.email = emailLoggedIn;
			su.vote = "up";
			request.AddJsonBody(su);
			//WhichStudent ws = new WhichStudent();
			//ws.name = name;
			//request.AddJsonBody(ws);

			IRestResponse response = client.Execute(request);
		}

		public static void unblockProfile(string id) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/unblockUser");
			var request = new RestRequest(Method.POST);
			client.CookieContainer = cookieJar;
			Block b = new Block();
			b.id = id;
			request.AddJsonBody(b);

			IRestResponse response = client.Execute(request);
		}

		public static void blockProfile(string id) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/blockUser");
			var request = new RestRequest(Method.POST);
			client.CookieContainer = cookieJar;
			Block b = new Block();
			b.id = id;
			request.AddJsonBody(b);

			IRestResponse response = client.Execute(request);
		}

		public static void deleteProfile() {
			var client = new RestClient(); // Needs url
			var request = new RestRequest(Method.GET);

			IRestResponse response = client.Execute(request);
		}

		public static void reportProfile(string id, string name, string reason) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/reportUser");
			var request = new RestRequest(Method.POST);

			Report r = new Report();
			r.id = id;
			r.name = name;
			r.reason = reason;
			request.AddJsonBody(r);

			IRestResponse response = client.Execute(request);
		}

		public static Reports getReports() {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/getReports");
			var request = new RestRequest(Method.POST);

			IRestResponse response = client.Execute(request);
			Reports reports = JsonConvert.DeserializeObject<Reports>(response.Content);
			return reports;
		}



		// Messaging functions
		public static GetChatID startAChat(string id) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/dms/start");
			var request = new RestRequest(Method.POST);
			client.CookieContainer = cookieJar;

			UserID uid = new UserID();
			uid.dm_user = id;
			request.AddJsonBody(uid);	

			IRestResponse response = client.Execute(request);
			GetChatID cid = JsonConvert.DeserializeObject<GetChatID>(response.Content);
			return cid;
		}

		public static Messages getMessages(string chatID) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/chat/messages/get");
			var request = new RestRequest(Method.POST);
			client.CookieContainer = cookieJar;

			SendChatID cid = new SendChatID();
			cid.chatID = chatID;
			request.AddJsonBody(cid);

			IRestResponse response = client.Execute(request);
			Messages ms = null;
			try {
				ms = JsonConvert.DeserializeObject<Messages>(response.Content); // crashed here when navigating to message page
			} catch (Exception e) {
				// Ignore and return no messages
			}
			return ms;

		}

		public static void sendMessage(string id, string message) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/chat/message/send");
			var request = new RestRequest(Method.POST);
			client.CookieContainer = cookieJar;

			MessageInfo mi = new MessageInfo();
			mi.chatID = id;
			mi.message = message;
			request.AddJsonBody(mi);

			IRestResponse response = client.Execute(request);
		}








		// Deprecated
		/*
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
