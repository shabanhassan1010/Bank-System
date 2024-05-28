using Main;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_System
{
    public  class Manager:Person
    {
        //Atttibutes

        const string FilePathEmp = @"D:\ITI\\Bank_System\employee.json";
        const string FilePathManager = @"D:\ITI\\Bank_System\Managers.json";
        public static int ManagerID {  get; set; }
        public static List<Person> Managers ;
        public static List<Employee> employees ; //data member

        //Constructor        

        public Manager(int id, string name, string password) : base(id, name, password)
        {
        }


        //Methods

        public static  void Login()
        {
            Managers=LoadDataManger();
            Console.WriteLine("enter your id:");
              ManagerID = int.Parse(Console.ReadLine());
            Console.WriteLine("enter your password:");
            string password = Console.ReadLine();
            if (Managers.Any(x => x.Id == ManagerID) && Managers.Any(e => e.Password == password))
            {
                AnsiConsole.Write(new FigletText("Login successfully").Color(Color.Yellow).Centered());

                Thread.Sleep(1000);
                Console.Clear();

            }
            else
            {
                var userInput = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
           .Title("select option from:")
           .PageSize(4)
           .AddChoices(new[] {
            "1- Invalid pass or Id try again", "2- Previous Menu",
        }));
                switch (userInput)
                {
                    case "1- Invalid pass or Id try again":
                        Login();
                        break;
                    case "2- Previous Menu":
                        Console.Clear();
                        Bank.BankSystem();
                        break;
                }
            }
        }
        public static List<Employee> LoadDataEmp()
        {
            if (File.Exists(FilePathEmp))
            {
                var file = new FileInfo(FilePathEmp);
                if (file.Length > 0)
                { 
                    string json = File.ReadAllText(FilePathEmp);
                    return JsonConvert.DeserializeObject<List<Employee>>(json);
                }
            }
            return new List<Employee>();
        }
        public static List<Person> LoadDataManger()
        {
            if (File.Exists(FilePathManager))
            {
                var file = new FileInfo(FilePathManager);
                if (file.Length > 0)
                {
                    string json = File.ReadAllText(FilePathManager);
                    return JsonConvert.DeserializeObject<List<Person>>(json);
                }
            }
            return new List<Person>();
        }
        private static void SaveData()
        {
            var jsonEmp = JsonConvert.SerializeObject(employees, Formatting.Indented);
            File.WriteAllText(FilePathEmp, jsonEmp);

            var jsonMang = JsonConvert.SerializeObject(Managers, Formatting.Indented);
            File.WriteAllText(FilePathManager, jsonMang);

        }
        public static void AddEmployee()
        {
            Console.WriteLine("enter name of the employee");
            string empName = Console.ReadLine();
            Console.WriteLine("enter password for the employee");
            string empPassword = Console.ReadLine();
            Console.WriteLine("enter salary for the employee");
            double empSalary = double.Parse(Console.ReadLine());
            Console.WriteLine("enter id for the Employee");
            int empId = int.Parse(Console.ReadLine());
            employees =LoadDataEmp();
            if (employees.Any(e => e.Id == empId))
            {
                Console.WriteLine("Employee already .");
                return;
            }
            employees.Add(new Employee(empId, empName, empPassword, empSalary, Manager.ManagerID));
            SaveData();
            Console.WriteLine("Employee added successfully.\n");
            RollBack();
        }
        public static void DeleteEmployee()
        {
            Console.WriteLine("enter employee id");
            int id = int.Parse(Console.ReadLine());
            employees = LoadDataEmp();
            Employee emp = employees.FirstOrDefault(e => e.Id == id);
            if (emp != null)
            {
                employees.Remove(emp);
                SaveData();
                Console.WriteLine($"Employee with ID {id} removed successfully.");

            }
            else
                Console.WriteLine($"Employee not found\n");
            RollBack();
        }
        public static void UpdateSalary()
        {
            employees = LoadDataEmp();
            Console.WriteLine("Enter the employee Id ");
            Employee emp=employees.FirstOrDefault(e=> e.Id== int.Parse(Console.ReadLine()));
            Console.WriteLine("Enter the new salary");
            emp.Salary = double.Parse(Console.ReadLine());
            SaveData();
            Console.WriteLine($"the salary for {emp.Name} become {emp.Salary}\n");
           RollBack();
        }
        public static void RollBack()
        {
            var userInput = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
           .Title("select option from:")
           .PageSize(4)
           .AddChoices(new[] {
            "1- Exit", "2- Previous Menu",
        }));
            switch (userInput)
            {
                case "1- Exit":
                    Console.Clear();
                    Environment.Exit(0);
                    
                    break;
                case "2- Previous Menu":
                    Console.Clear();
                    Bank.ManagerFunc();
                    break;
            }
        }
    }
}
