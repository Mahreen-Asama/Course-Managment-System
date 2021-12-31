using System;
using ClassLibraryManageCourses;
using ClassLibraryManageStudents;
using ClassLibraryManageTeachers;
using Microsoft.Data.SqlClient;

namespace ClassLibraryAdmin
{

    public class Admin
    {
        static string constr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CMS_Database;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static bool AdminLogin()
        {
            Console.Write("Enter username: ");
            string uname = Console.ReadLine();
            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            string query = "select * from AdminLogin where username=@un and password=@pass";

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
            conn.Close();                           //if admin not exist
            return false;
        }

        public static void DisplayAdminMenu()
        {
            string input = string.Empty;
            do
            {
                Console.WriteLine("\nEnter 1 to Manage Students");
                Console.WriteLine("Enter 2 to Manage Teachers");
                Console.WriteLine("Enter 3 to Manage Courses");
                Console.WriteLine("Enter 4 to Exit");

                Console.Write("\nEnter Choice: ");
                input = Console.ReadLine();         //assume user will enter a number
                Console.Clear();

                if (input == "1")
                {
                    ManageStudent();
                }
                else if (input == "2")
                {
                    ManageTeacher();
                }
                else if (input == "3")
                {
                    ManageCourses();
                }
                else if (input != "4")
                {
                    Console.WriteLine("\nInvalid input");
                }

            } while (input != "4");
        }

        public static void ManageStudent()
        {
            string input = string.Empty;
            do
            {
                Console.WriteLine("\nEnter 1 to Add Student");
                Console.WriteLine("Enter 2 to Update Student");
                Console.WriteLine("Enter 3 to Delete Student");
                Console.WriteLine("Enter 4 to View All Students");
                Console.WriteLine("Enter 5 to Display Outstanding Semester Dues");
                Console.WriteLine("Enter 6 to Assign Course to Student");
                Console.WriteLine("Enter 7 to Exit");

                Console.Write("\nEnter Choice: ");
                input = Console.ReadLine();
                Console.Clear();

                if (input == "1")
                {
                    ManageStudentsRepo.AddStudent();
                }
                else if (input == "2")
                {
                    Console.Write("Enter id: ");
                    int id = int.Parse(Console.ReadLine());
                    ManageStudentsRepo.UpdateStudent(id);
                }
                else if (input == "3")
                {
                    Console.Write("Enter id: ");
                    int id = int.Parse(Console.ReadLine());
                    ManageStudentsRepo.DeleteStudent(id);
                }
                else if (input == "4")
                {
                    ManageStudentsRepo.ViewAllStudents();
                }
                else if (input == "5")
                {
                    ManageStudentsRepo.DisplayOutstandingSemesterDues();
                }
                else if (input == "6")
                {
                    ManageStudentsRepo.AssignCourseToStudent();
                }
                else if (input != "7")
                {
                    Console.WriteLine("\nInvalid input");
                }

            } while (input != "7");

        }

        public static void ManageTeacher()
        {
            string input = string.Empty;
            do
            {
                Console.WriteLine("\nEnter 1 to Add Teacher");
                Console.WriteLine("Enter 2 to Update Teacher");
                Console.WriteLine("Enter 3 to Delete Teacher");
                Console.WriteLine("Enter 4 to View All Teachers");
                Console.WriteLine("Enter 5 to Assign Course to Teacher");
                Console.WriteLine("Enter 6 to Exit");

                Console.Write("\nEnter Choice: ");
                input = Console.ReadLine();
                Console.Clear();

                if (input == "1")
                {
                    ManageTeachersRepo.AddTeacher();
                }
                else if (input == "2")
                {
                    Console.Write("Enter id: ");
                    int id = int.Parse(Console.ReadLine());

                    ManageTeachersRepo.UpdateTeacher(id);
                }
                else if (input == "3")
                {
                    Console.Write("Enter id: ");
                    int id = int.Parse(Console.ReadLine());
                    ManageTeachersRepo.DeleteTeacher(id);
                }
                else if (input == "4")
                {
                    ManageTeachersRepo.ViewAllTeachers();
                }
                else if (input == "5")
                {
                    ManageTeachersRepo.AssignCourseToTeacher();
                }
                else if (input != "6")
                {
                    Console.WriteLine("\nInvalid input");
                }

            } while (input != "6");
        }

        public static void ManageCourses()
        {
            string input = string.Empty;
            do
            {
                Console.WriteLine("\nEnter 1 to Add Course");
                Console.WriteLine("Enter 2 to Update Course");
                Console.WriteLine("Enter 3 to Delete Course");
                Console.WriteLine("Enter 4 to View All Courses");
                Console.WriteLine("Enter 5 to Exit");

                Console.Write("\nEnter Choice: ");
                input = Console.ReadLine();
                Console.Clear();

                if (input == "1")
                {
                    ManageCoursesRepo.AddCourse();
                }
                else if (input == "2")
                {
                    Console.Write("Enter id: ");
                    int id = int.Parse(Console.ReadLine());
                    ManageCoursesRepo.UpdateCourse(id);
                }
                else if (input == "3")
                {
                    Console.Write("Enter id: ");
                    int id = int.Parse(Console.ReadLine());
                    ManageCoursesRepo.DeleteCourse(id);
                }
                else if (input == "4")
                {
                    ManageCoursesRepo.ViewAllCourses();
                }
                else if (input != "5")
                {
                    Console.WriteLine("\nInvalid input");
                }

            } while (input != "5");
        }
    }
}
