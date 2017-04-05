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
		public string Pass;
	}

	public class EmailRecovery {
		public string Email;
	}

	public class ResetCode {
		public string Code;
	}

	public class Account {
		public string Email;
		public string Password;
	};

	public class Profile {
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

	public class ClassList {
		public string[] Classes;
	}

	public class StudentList {
		public string[] Students;
	}

	public class WhichProfile {
		public string Email;
	}
}