using System;
using Microsoft.Data.SqlClient;

namespace ClassLibraryManageCourses
{
    public class ManageCoursesRepo
    {
        static string constr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CMS_Database;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static void AddCourse()
        {
            Console.Write("Enter course name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Credit hours: ");
            int cr = int.Parse(Console.ReadLine());

            SqlConnection conn = new SqlConnection(constr);
            conn.Open();

            string query = $"insert into Course values('{name}','{cr}')";
            SqlCommand cmd = new SqlCommand(query, conn);

            int n = cmd.ExecuteNonQuery();
            if (n > 0)
            {
                Console.WriteLine("\nCourse added successfully");
            }
            conn.Close();
        }

        public static void UpdateCourse(int id)
        {
            Console.Write("Enter name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Credit hours: ");
            int cr = int.Parse(Console.ReadLine());

            SqlConnection conn = new SqlConnection(constr);
            conn.Open();

            string query = $"update course set courseName='{name}',creditHours='{cr}' where id='{id}'";
            SqlCommand cmd = new SqlCommand(query, conn);

            int n = cmd.ExecuteNonQuery();
            if (n > 0)
            {
                Console.WriteLine("\nCourse updated successfully");
            }
            else
            {
                Console.WriteLine("\ncourse not exist");
            }
            conn.Close();
        }

        public static void DeleteCourse(int id)
        {
            SqlConnection conn = new SqlConnection(constr);
            conn.Open();

            string query = $"delete from course where id='{id}'";
            SqlCommand cmd = new SqlCommand(query, conn);

            int n = 0;
            //..................//foreign key issue.......
            try
            {
                n = cmd.ExecuteNonQuery();          
            }
            catch
            {
                //.......delete course assigned to student
                query = $"delete from studentEnroll where courseID='{id}'";
                cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();

                //.......delete course assigned to teacher
                query = $"delete from teacherEnroll where courseID='{id}'";
                cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();

                //....finallly deleting the course
                query = $"delete from course where id='{id}'";
                cmd = new SqlCommand(query, conn);
                n = cmd.ExecuteNonQuery();
            }
            if (n > 0)
            {
                Console.WriteLine("\nCourse deleted successfully");
            }
            else
            {
                Console.WriteLine("\ncourse not exist");
            }
            conn.Close();
        }

        public static void ViewAllCourses()
        {
            string query = "select * from course";

            SqlConnection conn = new SqlConnection(constr);
            conn.Open();

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            Console.WriteLine(
                format: "{0,-10} {1,-20} {2,-20}",
                arg0: "Id",
                arg1: "CourseName",
                arg2: "CreditHours"
                );
            while(dr.Read())
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
