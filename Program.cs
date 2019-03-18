using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsvHelper;
using System.Threading.Tasks;
using System.IO;
using System;
using System.Data.SqlClient;
using ConsoleApplication9;
using System.Data.Entity.Infrastructure;
using System.Globalization;

namespace ConsoleApplication6
{
    class Program
    {
        public static void displaydata()
        {
            using (var db = new workshopEntities())
            {

                var query = from employee_data in db.employee_details orderby employee_data.name select employee_data;

                foreach (var item in query)
                {
                    Console.WriteLine("Employee ID: " + item.emp_id + "\n" + "Employee Name: " + item.name + "\n" + "Date of Birth: " + item.DOB + "\n" + "Location : " + item.location + "\n" + "Date of Joining: " + item.date_of_joining + "\n");
                    Console.WriteLine("-----------------------------------------------------------------------------");
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void Main(string[] args)
        {

            var reader = new StreamReader("C:\\Users\\suraj\\Desktop\\100_Records.csv");
            var csv = new CsvReader(reader);
            List<data> datalist = new List<data>();
            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                var record = new data
                {
                    emp_id = csv.GetField<int>("Emp_ID"),
                    First_Name = csv.GetField("First_Name"),
                    Last_Name = csv.GetField("Last_Name"),
                    DOB = csv.GetField("Date_of_Birth"),
                    Date_of_joining = csv.GetField("Date_of_Joining"),
                    location = csv.GetField("location")
                };

                datalist.Add(record);

            }

            var employeedata = new employee_details();
           
            try
            {
                foreach (var employee in datalist)
                {
                    DateTime birthday = Convert.ToDateTime(employee.DOB, CultureInfo.GetCultureInfo("en-US").DateTimeFormat);
                    DateTime joining = Convert.ToDateTime(employee.DOB, CultureInfo.GetCultureInfo("en-US").DateTimeFormat);
                    employeedata.emp_id = employee.emp_id;
                    employeedata.name = employee.First_Name + " " + employee.Last_Name;
                    employeedata.DOB = birthday;
                    employeedata.age = 35;
                    employeedata.date_of_joining = joining;
                    employeedata.location = employee.location;

                    using (var dbCtx = new workshopEntities())
                    {

                        dbCtx.employee_details.Add(employeedata);
                        dbCtx.SaveChanges();
                    }

                }
                displaydata();
                Console.ReadKey();
            }
            catch(DbUpdateException e)
            {

                displaydata();
            }

        }
       
    }

    class data
    {
        public int emp_id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string DOB { get; set; }
        public string location { get; set; }
        public string Date_of_joining { get; set; }
    }

}
