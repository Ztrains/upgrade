
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

		public static string emailLoggedIn; // stores the email(username) of the current logged in user
		public static CookieContainer cookieJar; // holds the identification string used in many functions

		// Registers the current device with Firebase for notifications
		// key - the key of the current device to be registered
		public static void registerDevice(string key) {
			// Create request
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/regDevice");
			var request = new RestRequest(Method.POST);
			client.CookieContainer = cookieJar;
			// Create a key holding class
			RegKey rk = new RegKey();
			rk.regKey = key;
			request.AddJsonBody(rk);
			IRestResponse response = client.Execute(request);
		}

		// Registers a user with the database
		// email - the username of the new user
		// password - the user's password
		// Returns the status of the request
		public static int registerRequest(string email, string password) {
			// Creates a new request
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/reg");
			var request = new RestRequest(Method.POST);
			// Holds the username and password
			Account acc = new Account();
			acc.email = email;
			acc.password = password;
			request.AddJsonBody(acc);
			IRestResponse response = client.Execute(request);
			// Check response for errors
			string r = response.Content;
			if (r.Equals("Database error"))
				return -2;
			else if (r.Equals("User exists"))
				return -3;
			else if (r.Equals("Success adding user"))
				return 1;
			else if (r.Equals("Banned"))
				return -4;
			else return -1;
		}

		// Attempts to log a user into the system
		// email - the username of the user
		// password - the user's password
		// Returns the status of the request
		public static int loginRequest(string email, string password) {
			// Initialize the cookie for usage in various functions
			cookieJar = new CookieContainer();
			// Create a new request
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/login");
			var request = new RestRequest(Method.POST);
			// Add the cookie to the request
			client.CookieContainer = cookieJar;
			// Create an account obj to hold username and password
			Account acc = new Account();
			acc.email = email;
			acc.password = password;
			request.AddJsonBody(acc);
			IRestResponse response = client.Execute(request);
			// Check the server response code
			if (response.Content.Equals("you have authenticated properly")) {
				return 1;
			}
			else if (response.Content.Equals("Incorrect email/password combo")) {
				return -2;
			}
			else return -1;
		}



		public static void testLogin() {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/testlogin");
			var request = new RestRequest(Method.POST);
			IRestResponse r = client.Execute(request);
		}

		public static void banUser(string email) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/banUser");
			var request = new RestRequest(Method.POST);

			BanUser bu = new BanUser();
			bu.banEmail = email;
			request.AddJsonBody(bu);

			IRestResponse r = client.Execute(request);
		}

		

		public static void setRecovery(string question, string answer) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/setRecovery"); 
			var request = new RestRequest(Method.POST);
			client.CookieContainer = cookieJar;
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
			client.CookieContainer = cookieJar;
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
			client.CookieContainer = cookieJar;
			RecoveryCheck rc = new RecoveryCheck();
			rc.email = email;
			rc.answer = answer;
			request.AddJsonBody(rc);

			IRestResponse response = client.Execute(request);
			return response.Content;
		}

		
		public static string updatePassword(string password, string newPassword, string recovered, string email) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/changePassword");
			var request = new RestRequest(Method.POST);
			
			PasswordReset pr = new PasswordReset();
			pr.password = password;
			pr.newpassword = newPassword;
			pr.recovered = recovered;
			pr.email = email;
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
			StudentList students;
			try {
				students = JsonConvert.DeserializeObject<StudentList>(response.Content);
			} catch (Exception e) {
				students = new StudentList();
				students.students = null;
			}
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

		// Works for private chats and message boards
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


		// Message board functions
		public static GetChatID startABoard(string className) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/dms/class/start");
			var request = new RestRequest(Method.POST);
			client.CookieContainer = cookieJar;

			MessageBoard mb = new MessageBoard();
			mb.classID = className;
			request.AddJsonBody(mb);

			IRestResponse response = client.Execute(request);
			GetChatID cid = JsonConvert.DeserializeObject<GetChatID>(response.Content);
			return cid;
		}

		public static void sendMessageBoard(string chatID, string className, string message) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/chat/class/message/send");
			var request = new RestRequest(Method.POST);
			client.CookieContainer = cookieJar;

			MessageBoardInfo mbi = new MessageBoardInfo();
			mbi.classID = className;
			mbi.chatID = chatID;
			mbi.message = message;
			request.AddJsonBody(mbi);

			IRestResponse response = client.Execute(request);
		}

		public static string getName(string id) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/retrieveName");
			var request = new RestRequest(Method.POST);
			client.CookieContainer = cookieJar;

			IDtoName i = new IDtoName();
			i.id = id;
			request.AddJsonBody(i);

			IRestResponse response = client.Execute(request);
			Name name = JsonConvert.DeserializeObject<Name>(response.Content);
			return name.name;
		}

		public static string[] getClassAdditionRequests() {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/requestedClasses");
			var request = new RestRequest(Method.POST);

			IRestResponse response = client.Execute(request);
			RequestedClasses rc = JsonConvert.DeserializeObject<RequestedClasses>(response.Content);
			return rc.requested;
		}

		public static void requestClass(string className) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/requestClass");
			var request = new RestRequest(Method.POST);


			WhichClass wc = new WhichClass();
			wc.className = className;
			request.AddJsonBody(wc);

			IRestResponse response = client.Execute(request);
		}

		public static void emptyClassRequestList() {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/clearRequests");
			var request = new RestRequest(Method.POST);
			IRestResponse response = client.Execute(request);
		}

		public static void createClass(string className) {
			var client = new RestClient("https://calm-chamber-49049.herokuapp.com/newClass");
			var request = new RestRequest(Method.POST);
			client.CookieContainer = cookieJar;

			WhichClass wc = new WhichClass();
			wc.className = className;
			request.AddJsonBody(wc);

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
