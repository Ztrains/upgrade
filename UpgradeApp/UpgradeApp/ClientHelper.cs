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
	class ClientHelper {

		public static ClassList filterClasses(ClassList classes, string filter) {

			ClassList filteredClasses = new ClassList();
			foreach (string c in classes.classes) {
				if (c.Contains(filter))
					filteredClasses.classes.Append(c);
				// Need a "subject" field or something to filter classes based on if we want that functionality
				//else if (() && ())
				//	filteredClasses.classes.Append(c);
			}

			return filteredClasses;
		}

		public static StudentList filterStudents(StudentList studentList, string filter) {

			StudentList filteredStudentList = new StudentList();
			foreach (Student s in studentList.students) {
				if (s.name.Contains(filter))
					filteredStudentList.students.Append(s);
				else if ((filter.Equals("students") || filter.Equals("student") || filter.Equals("stud")) && (s.type.Equals("student")))
					filteredStudentList.students.Append(s);
				else if ((filter.Equals("tutors") || filter.Equals("tutor") || filter.Equals("tut")) && (s.type.Equals("tutor")))
					filteredStudentList.students.Append(s);
			}

			return filteredStudentList;
		}

	}
}