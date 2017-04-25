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

	public class RegKey {
		public string regKey;
	}

	public class Password {
		public string pass;
	}

	public class PasswordReset {
		public string password;
		public string newpassword;
		public string recovered;
		public string email;
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
		//public string[] classesTutor;
		//public string[] classesStudent;
		public classInfo[] classesIn;
		public string time;
		public string price;
		public string visible;
		public string avatar;
		public string admin;
		public upvotedID[] usersUpvoted;
		public blockedID[] blockedUsers;
	};

	public class upvotedID {
		public string _id;
	}

	public class blockedID {
		public string id;
	}

	public class Reports {
		public Report[] reportedUsers;
	}

	public class Report {
		public string id;
		public string name;
		public string reason;
	}

	public class Block {
		public string id;
	}

	public class classInfo {
		public string className;
		public string type;
	}

	public class joinClass {
		public string className;
		public string type; // tutor or student
	}

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

	public class GetChatID {
		public string _id;
	}

	public class SendChatID {
		public string chatID;
	}

	public class MessageInfo {
		public string chatID;
		public string message;
	}

	public class UserID {
		public string dm_user;
	}

	public class Messages {
		public Message[] messages;
	}

	public class Message {
		public string message;
		public string date;
		public string sender;
	}

	public class MessageBoard {
		public string classID;
	}

	public class MessageBoardInfo {
		public string message;
		public string chatID;
		public string classID;
	}
}