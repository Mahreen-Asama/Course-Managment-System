using System;
using Microsoft.Data.SqlClient;

namespace ClassLibraryTeacherRepository
{
    public class TeacherMethods
    {
        static string constr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CMS_Database;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        static string uname = string.Empty;
        static string password = string.Empty;
        static int idgot = -1;                  //id against logedin username

        public static bool TeacherLogin()
        {
            Console.Write("Enter username: ");
            uname = Console.ReadLine();
            Console.Write("Enter password: ");
            password = Console.ReadLine();

            string query = "select * from teacherLogin where username=@un and password=@pass";

            SqlParameter p1 = new SqlParameter("un", uname);
            SqlParameter p2 = new SqlParameter("pass", password);

            SqlConnection conn = new SqlConnection(constr);
            conn.Open();

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                conn.Close();
                return true;
            }
            conn.Close();                           //if student not exist
            return false;
        }

        public static void DisplayTeacherMenu()
        {
            string input = string.Empty;
            do
            {
                Console.WriteLine("\nEnter 1 to Mark attendance");
                Console.WriteLine("Enter 2 to post assignment");
                Console.WriteLine("Enter 3 to View Assigned Courses");
                Console.WriteLine("Enter 4 to Exit");

                Console.Write("\nEnter Choice: ");
                input = Console.ReadLine();
                Console.Clear();

                if (input == "1")
                {
                    MarkAttendance();
                }
                else if (input == "2")
                {
                    PostAssignment();
                }
                else if (input == "3")
                {
                    ViewAssignedCourses();
                }
                else if (input != "4")
                {
                    Console.WriteLine("\nInvalid input");
                }

            } while (input != "4");
        }

        public static void MarkAttendance()
        {
            //..........first display all courses to teacher..........
            ViewAssignedCourses();      //"idgot" 

            Console.Write("Enter course ID of which to mark attendance: ");
            int cid = int.Parse(Console.ReadLine());
            Console.Write("Enter attendance date: ");
            string atdate = Console.ReadLine();     //asuming teacher input correct date

            SqlConnection conn = new SqlConnection(constr);
            conn.Open();

            //........verify course for teacher
            string query = $"select * from teacherEnroll where teacherID='{idgot}' and courseID='{cid}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            if (!dr.HasRows)
            {
                Console.WriteLine("You are not enrolled to this course\n");
                conn.Close();
                return;
            }
            conn.Close();

            //.......get name of course.....
            conn.Open();
            query = $"select courseName from course where id='{cid}'";

            cmd = new SqlCommand(query, conn);
            dr = cmd.ExecuteReader();

            string cname = string.Empty;
            while(dr.Read())
            {
                cname = (string)dr[0];
            }
            conn.Close();

            //..........display.........
            Console.WriteLine("\n\nCourse name: " + cname);
            Console.WriteLine("Attendance date: " + atdate+"\n\n");

            Console.WriteLine(
                format: "{0,-25} {1,-25} {2}",
                arg0: "RollNumber",
                arg1: "Name",
                arg2: "Attendance Status"
                );
            //........get those students enrolled in this course.....
            query = $"select s.rollNumber,s.name,s.id from studentEnroll se inner join student s on se.studentID=s.id where se.courseID='{cid}'";
            cmd = new SqlCommand(query, conn);
            conn.Open();
            dr = cmd.ExecuteReader();

            //..........making a new sql connection...
            SqlConnection conn1 = new SqlConnection(constr);
            conn1.Open();
            while(dr.Read())
            {
                Console.Write(dr[0] + "\t\t  " + dr[1] + "\t\t\t");
                string att = Console.ReadLine();

                string q1 = $"insert into AttendanceInfo values('{atdate}','{idgot}','{cid}','{(int)dr[2]}','{att}')";
                SqlCommand cmd1 = new SqlCommand(q1, conn1);
                int n = cmd1.ExecuteNonQuery();
                if(n>0)
                {
                    //Console.WriteLine("status saved");
                }
                //......
            }
            conn1.Close();
            conn.Close();
        }

        public static void PostAssignment()
        {
            //..........first display all courses to teacher..........
            ViewAssignedCourses();      //will get "idgot' from this func

            Console.Write("\nEnter course ID of which assigemnt to post: ");
            string cid = Console.ReadLine();

            //........verify course for teacher
            SqlConnection conn = new SqlConnection(constr);
            conn.Open();
            string query = $"select * from teacherEnroll where teacherID='{idgot}' and courseID='{cid}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            if (!dr.HasRows)
            {
                Console.WriteLine("You are not enrolled to this course\n");
                conn.Close();
                return;
            }
            conn.Close();

            //........................................
            Console.Write("Enter Topic: ");
            string topic = Console.ReadLine();
            Console.Write("Enter description: ");
            string desc = Console.ReadLine();
            Console.Write("Enter Submission-Date(mm/dd/yyyy): ");   
            string deadline = Console.ReadLine();

            query = $"insert into AssignmentInfo values ('{topic}','{desc}','{deadline}','{cid}','{idgot}')";

            conn.Open();

            cmd = new SqlCommand(query, conn);
            try
            {
                int n = cmd.ExecuteNonQuery();
                if (n > 0)
                {
                    Console.WriteLine("\nAssignment posted successfully");
                }
            }
            catch
            {
                Console.WriteLine("Invalid input");
            }
            conn.Close();
        }

        public static void ViewAssignedCourses()
        {
            //getting id against logedin username...
            string query = $"select id from teacher where Tusername='{uname}'";

            SqlConnection conn = new SqlConnection(constr);
            conn.Open();

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            idgot = -1;             //just initialize
            while (dr.Read())
            {
                idgot = (int)dr[0];
            }
            conn.Close();

            //....................loged in student courses
            conn.Open();
            query = $"select c.id,c.courseName,c.creditHours from teacherEnroll te inner join course c on te.courseID=c.id  where te.teacherID='{idgot}'";
            cmd = new SqlCommand(query, conn);
            dr = cmd.ExecuteReader();

            Console.WriteLine(
                format: "{0,-10} {1,-20} {2,-20}",
                arg0: "CourseID",
                arg1: "CourseName",
                arg2: "CreditHours"
                );
            while (dr.Read())
            {
                Console.WriteLine(
               format: "{0,-10} {1,-20} {2,-20}",
               arg0: dr[0],
               arg1: dr[1],
               arg2: dr[2]
               );
            }
            conn.Close();
        }
    }
}
