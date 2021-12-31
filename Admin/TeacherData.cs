using System;

namespace ClassLibraryTeacherClass
{
    public class TeacherData
    {
        //...........................
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        //............................
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        //...........................
        private int salary;

        public int Salary
        {
            get { return salary; }
            set { salary = value; }
        }

        //...........................
        private int experience;         //in years

        public int Experience
        {
            get { return experience; }
            set { experience = value; }
        }

        //...........................
        private int noOfCoursesAssigned;

        public int NoOfCoursesAssigned
        {
            get { return noOfCoursesAssigned; }
            set { noOfCoursesAssigned = value; }
        }

    }
}
