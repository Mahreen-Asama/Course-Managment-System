using System;
using Microsoft.Data.SqlClient;

namespace ClassLibraryManageTeachers
{
    public class ManageTeachersRepo
    {
        static string constr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CMS_Database;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public static void AddTeacher()
        {
            //.......assuming user will enter corrrect input...........

            Console.Write("Enter name: ");
            string name = Console.ReadLine();
            Console.Write("Enter salary: ");
            string sal = Console.ReadLine();
            Console.Write("Enter experience(years): ");
            string exp = Console.ReadLine();
            Console.Write("Enter Number of courses assigned: ");
            string num = Console.ReadLine();
            //..........admin will set username and password of teacher at once
            Console.Write("\nSet userName of teacher: ");
            string un = Console.ReadLine();


            SqlConnection conn = new SqlConnection(constr);
            conn.Open();

            //..........check unique username
            string q1 = $"select * from teacherLogin where username='{un}'";
            SqlCommand cmd = new SqlCommand(q1, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.HasRows)
            {
                Console.WriteLine("This username already exists!");
                Console.Write("\nEnter different one: ");
                un = Console.ReadLine();
                conn.Close();

                q1 = $"select * from teacherLogin where username='{un}'";
                cmd = new SqlCommand(q1, conn);
                conn.Open();
                dr = cmd.ExecuteReader();
            }
            conn.Close();
            //...............now get password
            Console.Write("\nSet password of teacher: ");
            string pass = Console.ReadLine();

            conn.Open();
            //.......finaly adding teacher and its credentials
            string query = $"insert into teacherLogin values ('{un}','{pass}')";
            cmd = new SqlCommand(query, conn);
            int n = cmd.ExecuteNonQuery();
            if (n > 0)
            {
                Console.WriteLine("\nCredentials added successfully\n");
            }

            query = $"insert into teacher values('{name}','{sal}','{exp}','{num}','{un}')";
            cmd = new SqlCommand(query, conn);

            n = cmd.ExecuteNonQuery();
            if (n > 0)
            {
                Console.WriteLine("\nTeacher added successfully\n");
            }
            
            conn.Close();
        }

        public static void UpdateTeacher(int id)
        {
            //.........if teacher of this id not exist
            SqlConnection conn = new SqlConnection(constr);
            conn.Open();
            string query = $"select id from teacher where id='{id}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            if (!dr.HasRows)
            {
                Console.WriteLine("\nTeacher not exist!");
                conn.Close();
                return;
            }
            conn.Close();

            //...........if that id teacher exist
                Console.WriteLine("\n1.Update teacher data");
                Console.WriteLine("2.Update teacher credentials");
                Console.Write("\nEnter choice: ");
                string choice = Console.ReadLine();
                Console.Clear();

            if (choice == "1")
            {
                Console.Write("Enter name: ");
                string name = Console.ReadLine();
                Console.Write("Enter salary: ");
                string sal = Console.ReadLine();
                Console.Write("Enter experience(years): ");
                string exp = Console.ReadLine();
                Console.Write("Enter Number of courses assigned: ");
                string num = Console.ReadLine();

                query = $"update teacher set name='{name}', salary='{sal}', experience='{exp}', NoOfcoursesAssigned='{num}' where id='{id}'";
                cmd = new SqlCommand(query, conn);
                conn.Open();
                int n = cmd.ExecuteNonQuery();
                if (n > 0)
                {
                    Console.WriteLine("\nTeacher updated successfully");
                }
                conn.Close();
            }
            else if (choice == "2")
            {
                query = $"select Tusername from teacher where id='{id}'";
                cmd = new SqlCommand(query, conn);
                conn.Open();
                dr = cmd.ExecuteReader();
                string uname = string.Empty;
                while (dr.Read())
                {
                    uname = (string)dr[0];            //got username to update credentials
                }
                conn.Close();
                //..........admin will update username and password of teacher 
                Console.Write("\nUpdate userName of teacher: ");
                string un = Console.ReadLine();

                //..........check unique username
                string q1 = $"select * from teacherLogin where username='{un}'";
                cmd = new SqlCommand(q1, conn);
                conn.Open();
                dr = cmd.ExecuteReader();
                while (dr.HasRows)
                {
                    Console.WriteLine("This username already exists!");
                    Console.Write("\nEnter different one: ");
                    un = Console.ReadLine();
                    conn.Close();

                    q1 = $"select * from teacherLogin where username='{un}'";
                    cmd = new SqlCommand(q1, conn);
                    conn.Open();
                    dr = cmd.ExecuteReader();
                }
                conn.Close();
                //...............now get password
                Console.Write("\nUpdate password of teacher: ");
                string pass = Console.ReadLine();

                q1 = $"update teacherLogin set username='{un}',password='{pass}' where username='{uname}'";
                cmd = new SqlCommand(q1, conn);
                conn.Open();
                int n = cmd.ExecuteNonQuery();
                if (n > 0)
                {
                    Console.WriteLine("\nCredentials updated successfully");
                }
                conn.Close();
            }
            else
            {
                Console.WriteLine("Invalid input");
            }
        }

        public static void DeleteTeacher(int id)
        {
            string query = $"delete from teacher where id='{id}'";

            SqlConnection conn = new SqlConnection(constr);
            conn.Open();

            SqlCommand cmd = new SqlCommand(query, conn);

            int n = 0;
            //..................//foreign key issue.......
            try
            {
                n = cmd.ExecuteNonQuery();
            }
            catch
            {
                //.......delete courses assigned to teacher
                query = $"delete from teacherEnroll where teacherID='{id}'";
                cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();

                //....finallly deleting the teacher
                query = $"delete from teacher where id='{id}'";
                cmd = new SqlCommand(query, conn);
                n = cmd.ExecuteNonQuery();

            }
            if (n > 0)
            {
                Console.WriteLine("\nTeacher deleted successfully");
            }
            else
            {
                Console.WriteLine("\nTeacher not exist");
            }
            conn.Close();
        }

        public static void ViewAllTeachers()
        {
            string query = "select * from teacher";

            SqlConnection conn = new SqlConnection(constr);
            conn.Open();

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            Console.Write(
                format: "{0,-5} {1,-20} {2,-10}",
                arg0: "Id",
                arg1: "Name",
                arg2: "Salary"
                );
            Console.Write(
                format: "{0,-15} {1,-25} {2}",
                arg0: "Experience",
                arg1: "Courses Assigned",
                arg2: "T-username"
                );
            Console.WriteLine();
            while (dr.Read())
            {
                Console.Write(
                format: "{0,-5} {1,-20} {2,-10}",
                arg0: dr[0],
                arg1: dr[1],
                arg2: dr[2]
                );
                Console.Write(
                    format: "{0,-15} {1,-25} {2}",
                    arg0: dr[3],
                    arg1: dr[4],
                    arg2: dr[5]
                    );
                Console.WriteLine();
            }
            conn.Close();
        }

        public static void AssignCourseToTeacher()
        {
            Console.Write("Enter Teacher ID to whom assign course: ");
            int tid = int.Parse(Console.ReadLine());
            Console.Write("Enter course ID: ");
            int cid = int.Parse(Console.ReadLine());

            SqlConnection conn = new SqlConnection(constr);
            conn.Open();

            //..........check if course already assigned.......
            string query = $"select * from teacherEnroll where teacherID='{tid}' and courseID='{cid}'";

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            if(dr.HasRows)
            {
                Console.WriteLine("\nAlready assigned"!);
                conn.Close();
                return;
            }
            conn.Close();

            //.........else assign course
            query = $"insert into teacherEnroll values('{tid}','{cid}')";
            cmd = new SqlCommand(query, conn);
            conn.Open();
            try
            {
                int n = cmd.ExecuteNonQuery();
                if (n > 0)
                {
                    Console.WriteLine("\ncourse assigned to teacher successfully");
                }
            }
            catch
            {
                Console.WriteLine("Invalid assignment\n");  //if teacher/course not exist
            }
            conn.Close();
        }
    }
}
