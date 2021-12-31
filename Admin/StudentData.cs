using System;

namespace ClassLibraryStudentClass
{
    public class StudentData
    {
        //............................
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        //...........................
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        //...........................
        private string rollNumber;

        public string RollNumber
        {
            get { return rollNumber; }
            set { rollNumber = value; }
        }

        //...........................
        private string batch;

        public string Batch
        {
            get { return batch; }
            set { batch = value; }
        }

        //...........................
        private int semesterDues;

        public int SemesterDues
        {
            get { return semesterDues; }
            set { semesterDues = value; }
        }

        //...........................
        private int currentSemester;

        public int CurrentSemester
        {
            get { return currentSemester; }
            set { currentSemester = value; }
        }



    }
}
