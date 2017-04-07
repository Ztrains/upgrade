using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace UpgradeApp {
	public class Password {
		public string pass;
	}

	public class RecoveryInfo {
		public string email;
		public string question;
		public string answer;
	}

	public class Question {
		public string question;
	}

	public class RecoveryCheck {
		public string email;
		public string answer;
	}

	public class Account {
		public string email;
		public string password;
	};

	public class Profile {
		public string _id;
		public string hash;
		public string name;
		public string email;
		public string newemail; // used if updating the email associated with the account
		public string contact;
		public int rating;
		public string about;
		public string[] classesTutor;
		public string[] classesStudent;
		public string time;
		public string price;
	};

	public class ClassList {
		public string[] classes;
	}

	public class StudentList {
		public Student[] students;
	}

	public class Student {
		public string name;
		public string type;
	}

	public class SendUpvote {
		public string name;
		public string email;
		public string vote; // has to be "up" or "down"
	}

	public class WhichProfile {
		public string email;
	}

	public class WhichStudent {
		public string name;
	}

	public class WhichClass {
		public string className;
	}

	public class ChatID {
		public string chatID;
	}

	public class MessageInfo {
		public string chatID;
		public string message;
	}

	public class UserID {
		public string dm_user;
	}

}