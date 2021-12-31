using System;
using Microsoft.Data.SqlClient;

namespace ClassLibraryStudentRepository
{
    public class StudentMethods
    {
        static string constr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CMS_Database;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        static string uname = string.Empty;
        static string password = string.Empty;
        static int idgot = -1;

        public static bool StudentLogin()
        {
            Console.Write("Enter username: ");
            uname = Console.ReadLine();
            Console.Write("Enter password: ");
            password = Console.ReadLine();

            string query = "select * from studentLogin where username=@un and password=@pass";
           
            SqlParameter p1 = new SqlParameter("un", uname);
            SqlParameter p2 = new SqlParameter("pass", password);

            SqlConnection conn = new SqlConnection(constr);
            conn.Open();

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);

            SqlDataReader dr = cmd.ExecuteReader();
            if(dr.HasRows)
            {
                conn.Close();
                return true;
            }
            conn.Close();                           //if student not exist
            return false;
        }

        public static void DisplayStudentMenu()
        {
            string input = string.Empty;
            do
            {
                Console.WriteLine("\nPress 1 to Pay Semester Dues");
                Console.WriteLine("Press 2 to view enrolled courses");
                Console.WriteLine("Press 3 to view Assignments");
                Console.WriteLine("Press 4 to view Attendance");
                Console.WriteLine("Press 5 to Exit");

                Console.Write("\nEnter Choice: ");
                input = Console.ReadLine();
                Console.Clear();

                if (input == "1")
                {
                    //viewthis();
                    PaySemesterDues();
                }
                else if (input == "2")
                {
                    ViewCoursesEnrolled();
                }
                else if (input == "3")
                {
                    ViewCoursesEnrolled();
                    Console.Write("Enter course ID to view its assignemnts: ");
                    int id = int.Parse(Console.ReadLine());
                    ViewAssignments(id);
                }
                else if (input == "4")
                {
                    ViewCoursesEnrolled();
                    Console.Write("Enter course ID: ");
                    int id = int.Parse(Console.ReadLine());
                    ViewAttendance(id);
                }
                else if (input != "5")
                {
                    Console.WriteLine("\nInvalid input");
                }

            } while (input != "5");
        }
        
        public static void PaySemesterDues()
        {
            // username should unique
            String query = $"select semesterDues from student where Susername='{uname}'";
            
            SqlConnection conn = new SqlConnection(constr);
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            int prevDues = -1;
            while(dr.Read())
            {
                prevDues = (int)dr[0];        //first display previous dues
            }
            Console.WriteLine("Your remaining dues are: " + prevDues);
            conn.Close();

            //....asume user enter correct input accrding to datatype...

            if (prevDues > 0)
            {
                //.......pay dues.....
                Console.Write("\nEnter amount you want to pay: ");
                int amount = int.Parse(Console.ReadLine());

                while(amount>prevDues || amount<0)
                {
                    Console.Write("\nEnter valid amount: ");
                    amount = int.Parse(Console.ReadLine());
                }

                int rdues = prevDues - amount;
                string query1 = $"update student set semesterDues='{rdues}' where Susername='{uname}'";
                conn.Open();

                cmd = new SqlCommand(query1, conn);
                int n = cmd.ExecuteNonQuery();
                if (n > 0)
                {
                    Console.WriteLine("Your remaining dues are: " + rdues+"\n");
                }
            }
            else if(prevDues==0)
            {
                Console.WriteLine("\nYour dues are clear\n");
            }

        }

        public static void ViewCoursesEnrolled()
        {
            //getting id against logedin username...
            string query = $"select id from student where Susername='{uname}'";

            SqlConnection conn = new SqlConnection(constr);
            conn.Open();

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            idgot = -1;             //just initialize
            while(dr.Read())
            {
                idgot = (int)dr[0];
            }
            conn.Close();

            //....................loged in student courses
            conn.Open();
            query = $"select c.id,c.courseName,c.creditHours from studentEnroll se inner join course c on se.courseID=c.id  where se.studentID='{idgot}'";
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

        public static void ViewAssignments(int courseId)
        {
            SqlConnection conn = new SqlConnection(constr);
            conn.Open();

            string query = $"select * from studentEnroll where studentID='{idgot}' and courseID='{courseId}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            if(!dr.HasRows)
            {
                Console.WriteLine("You are not enrolled to this course\n");
                conn.Close();
                return;
            }
            conn.Close();

            //........comparing with today's date
            DateTime today = DateTime.Today;
            Console.WriteLine("date today: " + today);
            int dayToday = today.Day;
            Console.WriteLine("day today: " + dayToday);

            // pending assignments -> which have deadline of today or after current day

            query = $"select a.topic,a.description,a.deadline,a.cid from AssignmentInfo a inner join studentEnroll se on a.cid=se.courseID where se.studentID='{idgot}' and se.courseID='{courseId}' and a.deadline>='{today}'";
            cmd = new SqlCommand(query, conn);
            conn.Open();
            dr = cmd.ExecuteReader();

            Console.WriteLine("-----Pending assignments-------");
            Console.Write(
               format: "{0,-20} {1,-50} {2,-30}",
               arg0: "Topic",
               arg1: "Description",
               arg2: "Deadline(mm/dd/yyyy)"
               );
            Console.WriteLine(
              format: "{0}",
              arg0: "CourseID"
              );
            while (dr.Read())
            {
                Console.Write(
               format: "{0,-20} {1,-50} {2,-30}",
               arg0: dr[0],
               arg1: dr[1],
               arg2: dr[2]
               );
                Console.WriteLine(
            format: "{0}",
            arg0: dr[3]
            );
            }
            conn.Close();
        }

        public static void ViewAttendance(int courseId)
        {
            SqlConnection conn = new SqlConnection(constr);
            conn.Open();

            string query = $"select * from studentEnroll where studentID='{idgot}' and courseID='{courseId}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            if (!dr.HasRows)
            {
                Console.WriteLine("You are not enrolled to this course\n");
                conn.Close();
                return;
            }
            conn.Close();

            query = $"select coursename from course where id='{courseId}'";
            cmd = new SqlCommand(query, conn);
            conn.Open();
            dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                Console.WriteLine("\nCourse Name: " + dr[0]+"\n");
            }
            conn.Close();

            query = $"select date,A_status from AttendanceInfo where crsid='{courseId}' and stu_ID='{idgot}'";
            cmd = new SqlCommand(query, conn);
            conn.Open();
            dr = cmd.ExecuteReader();
            Console.WriteLine(
              format: "{0,-20} {1}",
              arg0: "Date",
              arg1: "Att_status"
              );
            while(dr.Read())
            {
                Console.WriteLine(
              format: "{0,-20} {1}",
              arg0: dr[0],
              arg1: dr[1]
              );
            }
            conn.Close();
        }

    }
}
