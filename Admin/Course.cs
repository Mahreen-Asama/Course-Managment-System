using System;

namespace ClassLibraryCourse
{
    public class Course
    {
        //...........................
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        //...........................
        private string courseName;

        public string CourseName
        {
            get { return courseName; }
            set { courseName = value; }
        }

        //...........................
        private int creditHours;

        public int CreditHours
        {
            get { return creditHours; }
            set { creditHours = value; }
        }

    }
}
