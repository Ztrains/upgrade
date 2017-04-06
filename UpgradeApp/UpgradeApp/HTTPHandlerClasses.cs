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

	public class EmailRecovery {
		public string email;
	}

	public class ResetCode {
		public string code;
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
		public string prices;
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

	public class WhichProfile {
		public string email;
	}

	public class WhichClass {
		public string className;
	}
}