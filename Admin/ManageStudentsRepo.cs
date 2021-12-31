using System;
using Microsoft.Data.SqlClient;

namespace ClassLibraryManageStudents
{
    public class ManageStudentsRepo
    {
        static string constr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CMS_Database;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static void AddStudent()
        {
            //..........assume user will enter correct input........
            Console.Write("Enter name: ");
            string name = Console.ReadLine();
            Console.Write("Enter roll-number: ");
            string roll = Console.ReadLine();
            Console.Write("Enter batch: ");
            string batch = Console.ReadLine();
            Console.Write("Enter semeste dues: ");
            string sdues = Console.ReadLine();
            Console.Write("Enter semester number: ");
            string currsem = Console.ReadLine();

            SqlConnection conn = new SqlConnection(constr);
            conn.Open();

            //..........admin will set username and password of student at once
            //username can be rollnumber or some other but it should unique
            Console.Write("\nSet userName of student: ");
            string un = Console.ReadLine();

            //..........check unique username
            string q1 = $"select * from studentLogin where username='{un}'";
            SqlCommand cmd = new SqlCommand(q1, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            while(dr.HasRows)
            {
                Console.WriteLine("This username already exists!");
                Console.Write("\nEnter different one: ");
                un = Console.ReadLine();
                conn.Close();

                q1 = $"select * from studentLogin where username='{un}'";
                cmd = new SqlCommand(q1, conn);
                conn.Open();
                dr = cmd.ExecuteReader();
            }
            conn.Close();
            //........now set password 
            Console.Write("\nSet password of student: ");
            string pass = Console.ReadLine();

            //...........finally adding student and credentials........
            conn.Open();
            string query = $"insert into studentLogin values ('{un}','{pass}')";
            cmd = new SqlCommand(query, conn);
            int n = cmd.ExecuteNonQuery();
            if (n > 0)
            {
                Console.WriteLine("\nCredentials added successfully\n");
            }

            query = $"insert into student values('{name}','{roll}','{batch}','{sdues}','{currsem}','{un}')";
            cmd = new SqlCommand(query, conn);

            n = cmd.ExecuteNonQuery();
            if (n > 0)
            {
                Console.WriteLine("\nStudent added successfully\n");
            }

            conn.Close();
        }

        public static void UpdateStudent(int id)
        {
            //...........if this id student not exist............
            SqlConnection conn = new SqlConnection(constr);
            conn.Open();
            string query = $"select id from student where id='{id}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            if (!dr.HasRows)
            {
                Console.WriteLine("\nstudent not exist!");
                conn.Close();
                return;
            }
            conn.Close();

            //.........if student exists.............

            Console.WriteLine("\n1.Update student data");
            Console.WriteLine("2.Update student credentials");
            Console.Write("\nEnter choice: ");
            string choice = Console.ReadLine();
            Console.Clear();
            
                if (choice == "1")
                {
                    Console.Write("Enter name: ");
                    string name = Console.ReadLine();
                    Console.Write("Enter roll-number: ");
                    string roll = Console.ReadLine();
                    Console.Write("Enter batch: ");
                    string batch = Console.ReadLine();
                    Console.Write("Enter semeste dues: ");
                    string sdues = Console.ReadLine();
                    Console.Write("Enter semester number: ");
                    string currsem = Console.ReadLine();

                    query = $"update student set name='{name}', rollnumber='{roll}', batch='{batch}', semesterdues='{sdues}',currentsemester='{currsem}' where id='{id}'";
                    cmd = new SqlCommand(query, conn);
                    conn.Open();
                    int n = cmd.ExecuteNonQuery();
                    if (n > 0)
                    {
                        Console.WriteLine("\nStudent updated successfully");
                    }
                    conn.Close();
                }
                else if (choice == "2")
                {
                    query = $"select Susername from student where id='{id}'";
                    cmd = new SqlCommand(query, conn);
                    conn.Open();
                    dr = cmd.ExecuteReader();
                    string uname = string.Empty;
                    while (dr.Read())
                    {
                        uname = (string)dr[0];          //got username to update credentials
                    }
                    conn.Close();
                Console.WriteLine("uname: " + uname);
                    //.........update credentials.............
                    Console.Write("\nUpdate userName of student: ");
                    string un = Console.ReadLine();

                    //..........check unique username
                    query = $"select * from studentLogin where username='{un}'";
                    cmd = new SqlCommand(query, conn);
                    conn.Open();
                    dr = cmd.ExecuteReader();
                    while (dr.HasRows)
                    {
                        Console.WriteLine("This username already exists!");
                        Console.Write("\nEnter different one: ");
                        un = Console.ReadLine();
                        conn.Close();

                        query = $"select * from studentLogin where username='{un}'";
                        cmd = new SqlCommand(query, conn);
                        conn.Open();
                        dr = cmd.ExecuteReader();
                    }
                    conn.Close();
                    //........now set password 
                    Console.Write("\nSet password of student: ");
                    string pass = Console.ReadLine();

                    query = $"update studentLogin set username='{un}',password='{pass}' where username='{uname}'";
                    cmd = new SqlCommand(query, conn);
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
                    Console.WriteLine("Invalid input\n");
                }
           
        }

        public static void DeleteStudent(int id)
        {
            string query = $"delete from student where id='{id}'";

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
                //.......delete courses assigned to student
                query = $"delete from studentEnroll where studentID='{id}'";
                cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();

                //....finallly deleting the student
                query = $"delete from student where id='{id}'";
                cmd = new SqlCommand(query, conn);
                n = cmd.ExecuteNonQuery();

            }
            if (n > 0)
            {
                Console.WriteLine("\nStudent deleted successfully");
            }
            else
            {
                Console.WriteLine("student not exist\n");
            }
            conn.Close();
        }

        public static void ViewAllStudents()
        {
            string query = "select * from student";

            SqlConnection conn = new SqlConnection(constr);
            conn.Open();

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            Console.Write(
                format: "{0,-5} {1,-15} {2,-15}",
                arg0: "Id",
                arg1: "Name",
                arg2: "RollNumber"
                );
            Console.Write(
                format: "{0,-10} {1,-20} {2,-20}",
                arg0: "Batch",
                arg1: "SemesterDues",
                arg2: "CurrentSemester"
                );
            Console.WriteLine(
                format: "{0}",
                arg0: "Stu-username"
                );
            while (dr.Read())
            {
                Console.Write(
                format: "{0,-5} {1,-15} {2,-15}",
                arg0: dr[0],
                arg1: dr[1],
                arg2: dr[2]
                );
                Console.Write(
                    format: "{0,-10} {1,-20} {2,-20}",
                    arg0: dr[3],
                    arg1: dr[4],
                    arg2: dr[5]
                    );
                Console.WriteLine(
                    format: "{0}",
                    arg0: dr[6]
                    );
            }
            conn.Close();
        }

        public static void DisplayOutstandingSemesterDues()
        {
            //..... outstanding=students with remaining dues....

            string query = "select * from student where semesterdues>0";

            SqlConnection conn = new SqlConnection(constr);
            conn.Open();

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            Console.Write(
                format: "{0,-5} {1,-15} {2,-15}",
                arg0: "Id",
                arg1: "Name",
                arg2: "RollNumber"
                );
            Console.Write(
                format: "{0,-10} {1,-20} {2,5}",
                arg0: "Batch",
                arg1: "SemesterDues",
                arg2: "CurrentSemester"
                );
            Console.WriteLine();
            while (dr.Read())
            {
                Console.Write(
                format: "{0,-5} {1,-15} {2,-15}",
                arg0: dr[0],
                arg1: dr[1],
                arg2: dr[2]
                );
                Console.Write(
                    format: "{0,-10} {1,-20} {2,5}",
                    arg0: dr[3],
                    arg1: dr[4],
                    arg2: dr[5]
                    );
                Console.WriteLine();
            }
            conn.Close();
        }

        public static void AssignCourseToStudent()
        {
            Console.Write("Enter student ID to whom assign course: ");
            int sid = int.Parse(Console.ReadLine());
            Console.Write("Enter course ID: ");
            int cid = int.Parse(Console.ReadLine());

            SqlConnection conn = new SqlConnection(constr);
            conn.Open();

            //..........check if course already assigned.......
            string query = $"select * from studentEnroll where studentID='{sid}' and courseID='{cid}'";

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                Console.WriteLine("\nAlready assigned"!);
                conn.Close();
                return;
            }
            conn.Close();

            //.........else assign course
            query = $"insert into studentEnroll values('{sid}','{cid}')";
            cmd = new SqlCommand(query, conn);
            conn.Open();
            //...........foreign key handle
            try
            {
                int n = cmd.ExecuteNonQuery();
                if (n > 0)
                {
                    Console.WriteLine("\ncourse assigned to student successfully");
                }
            }
            catch
            {
                Console.WriteLine("Invalid assignment\n");
            }
            conn.Close();
        }
    }
}
