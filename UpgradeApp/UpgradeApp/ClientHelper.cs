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

		// Returns a list of classes based on the given list and filter
		public static ClassList filterClasses(ClassList classes, string filter) {
			ClassList filteredClasses = new ClassList();
            int location = 0;
			// Ignore case
            StringComparison comp = StringComparison.OrdinalIgnoreCase;
            filteredClasses.classes = new string[classes.classes.Length];
            foreach (string c in classes.classes) {
                if (c.IndexOf(filter, 0, comp) != -1)
                {
                    filteredClasses.classes[location] = c;
                    location++;
                }
				// Need a "subject" field or something to filter classes based on if we want that functionality
				//else if (() && ())
				//	filteredClasses.classes.Append(c);
			}
			// Create return list
            ClassList returner = new ClassList();
            returner.classes = new string[location];
            for (int j = 0; j < location; j++)
            {
                returner.classes[j] = filteredClasses.classes[j];
            }
            return returner;
		}

		// Returns a list of students filtered based on the given filter string
		public static StudentList filterStudents(ref StudentList studentList, string filter) {
			if (studentList == null || studentList.students == null)
				return null;

			StudentList filteredStudentList = new StudentList();
            filteredStudentList.students = new Student[studentList.students.Length];
            int location = 0;
            StringComparison comp = StringComparison.OrdinalIgnoreCase;
			foreach (Student s in studentList.students) {
				// Filter based on text entered
				if (s.name.IndexOf(filter, 0, comp) !=-1)
                {
                    filteredStudentList.students[location] = s;
                    location++;
                }
				// Filter to only students
                else if ((filter.Equals("students") || filter.Equals("student") || filter.Equals("stud")) && (s.type.Equals("student")))
                {
                    filteredStudentList.students[location] = s;
                    location++;
                }
				// Filter to only tutors
                else if ((filter.Equals("tutors") || filter.Equals("tutor") || filter.Equals("tut")) && (s.type.Equals("tutor")))
                {
                    filteredStudentList.students[location] = s;
                    location++;
                }
            }
			// Create return list
            StudentList returner = new StudentList();
            returner.students = new Student[location];
            for(int j = 0; j < location; j++)
            {
                returner.students[j] = filteredStudentList.students[j]; 
            }
			return returner;
		}

	}
}