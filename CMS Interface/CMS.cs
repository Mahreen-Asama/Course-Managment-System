using System;
using ClassLibraryAdmin;
using ClassLibraryStudentRepository;
using ClassLibraryTeacherRepository;

namespace ClassLibraryCMS
{
    public class CMS
    {
        public static void InterfaceCMS()
        {
            string input = string.Empty;
            do
            {
                Console.WriteLine("Press 1 to Login as Student");
                Console.WriteLine("Press 2 to Login as Teacher");
                Console.WriteLine("Press 3 to Login as Admin");
                Console.WriteLine("Press 4 to exit");

                Console.Write("\nEnter Choice: ");
                input = Console.ReadLine();
                Console.Clear();

                if (input == "1")
                {
                    if (StudentMethods.StudentLogin())
                    {
                        Console.WriteLine("---------Student Login----------");
                        StudentMethods.DisplayStudentMenu();
                    }
                    else
                    {
                        Console.WriteLine("\nInvalid credentials");
                    }
                }
                else if (input == "2")
                {
                    if (TeacherMethods.TeacherLogin())
                    {
                        Console.WriteLine("---------Instructor Login----------");
                        TeacherMethods.DisplayTeacherMenu();
                    }
                    else
                    {
                        Console.WriteLine("\nInvalid credentials");
                    }
                }
                else if (input == "3")
                {
                    if (Admin.AdminLogin())
                    {
                        Console.WriteLine("---------Admin Login----------");
                        Admin.DisplayAdminMenu();
                    }
                    else
                    {
                        Console.WriteLine("\nInvalid credentials");
                    }
                }
                else if (input != "4")
                {
                    Console.WriteLine("\nInvalid input!");
                }

            } while (input != "4");

        }
    }
}
