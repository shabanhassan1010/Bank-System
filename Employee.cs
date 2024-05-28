
using Bank_System;
using Newtonsoft.Json;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    public class Employee : Person
    {
        //Atttibutes
        public static int empID {  get; set; } 
        public double Salary { get; set; }
        public int ManagerId { get; set; }
        const string filePath = @"D:\ITI\\Bank_System\clients.json";
        public static List<Client> Clients;
        //Static Attribute
        static int EmpCount { get; set; }

        //Constructor
        public Employee(int id, string name, string password, double salary,int managerId) : base(id, name, password)
        {
            this.Salary = salary;
            this.ManagerId = managerId;
        }

        
        //Methods
        public static void Login()
        { 
            Manager.employees = Manager.LoadDataEmp();
            Console.WriteLine("Enter your Id");
            empID=int.Parse(Console.ReadLine());
            Console.WriteLine("Enter your password");
            string pass=Console.ReadLine();
            if(Manager.employees.Any(e => e.Id == empID) && Manager.employees.Any(e => e.Password == pass))
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
        public static List<Client> LoadData()
        {
            if (File.Exists(filePath))
            {
                var file = new FileInfo(filePath);
                if (file.Length > 0)
                {
                    string json = File.ReadAllText(filePath);
                    return JsonConvert.DeserializeObject<List<Client>>(json);
                }
            }

            return new List<Client>();
        }

        public static void SaveData()
        {
            var Json = JsonConvert.SerializeObject(Clients, Formatting.Indented);
            File.WriteAllText(filePath, Json);

        }

        public static void AddClient()
        {
            Console.Write("Enter client Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter client Password: ");
            string pass = Console.ReadLine();
            Console.Write("Enter client ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("enter the initial balance: ");
            double balance = double.Parse(Console.ReadLine());
            Console.Write("Enter true if you want to add credit account or false if ypu don't: ");
            bool isCredit = bool.Parse(Console.ReadLine());

            Clients = LoadData();
            if (Clients.Any(c => c.Id == id))
            {
                Console.WriteLine("Client already exists.");
                return;
            }
            

            Clients.Add(new Client(id, name, pass, balance, isCredit, Employee.empID));
            SaveData();
            Console.WriteLine("Client added successfully.");
            RollBack();
        }

        public static void DeleteClient()
        {
            Console.Write("Enter client ID to delete: ");
            int id = int.Parse(Console.ReadLine());

            Client client = Clients.FirstOrDefault(c => c.Id == id);
            if (client != null)
            {
                Clients.Remove(client);
                SaveData();
                Console.WriteLine($"Client {id} is removed successfully.");

            }
            else
                Console.WriteLine($"Employee not found");
            RollBack();
        }
        public static void Withdraw()
        {
            Clients=LoadData();
            Console.Write("Enter client ID for withdrawal: ");
            int clientIdForWithdaw = int.Parse(Console.ReadLine());
            Client withdrawClient = Clients.FirstOrDefault(c => c.Id == clientIdForWithdaw);
            if (withdrawClient != null)
            {
                Console.Write("Enter withdrawal amount: ");
                if (double.TryParse(Console.ReadLine(), out double withdrawalAmount) && withdrawClient.Balance > withdrawalAmount)
                {
                    withdrawClient.Balance -= withdrawalAmount;
                    withdrawClient.EmpId = Employee.empID;
                    SaveData();
                    Console.WriteLine($"Withdrawal of {withdrawalAmount:C} for {clientIdForWithdaw} successful.");
                }
                else
                {
                    Console.WriteLine("Invalid input for withdrawal amount. Try again.");
                }
            }
            else
            {
                Console.WriteLine($"Client {clientIdForWithdaw} not found.");
            }
            RollBack() ;
        }
        public static void Deposit()
        {
            Clients = LoadData();
            Console.Write("Enter client ID for deposit: ");
            int id =int.Parse(Console.ReadLine());
            Client depositClient = Employee.Clients.FirstOrDefault(c => c.Id == id);
            if (depositClient != null)
            {
                Console.Write("Enter deposit amount: ");
                double depositAmount;
                if (double.TryParse(Console.ReadLine(), out depositAmount))
                {
                    depositClient.Balance += depositAmount;
                    depositClient.EmpId = Employee.empID;
                    SaveData();
                    Console.WriteLine($"Deposit of {depositAmount:C} for {id} successful.");
                }
                else
                {
                    Console.WriteLine("Invalid input for deposit amount. Try again.");
                }
            }
            else
            {
                Console.WriteLine($"Client {id} not found.");
            }
            RollBack();
        }
        public static void TransferTo()
        {
            Clients = LoadData();
            Console.Write("Enter the Account ID which Transfer From: ");
            int sourceClientId = int.Parse(Console.ReadLine());
            Client sourceClient = Employee.Clients.FirstOrDefault(c => c.Id == sourceClientId);
            if (sourceClient != null)
            {
                Console.Write("Enter the Account ID which Transfer TO: ");
                int targetClientId = int.Parse(Console.ReadLine());
                Client targetClient = Employee.Clients.FirstOrDefault(c => c.Id == targetClientId);
                if (targetClient != null)
                {
                    Console.Write("Enter transfer amount: ");
                    if (double.TryParse(Console.ReadLine(), out double transferAmount) && sourceClient.Balance > transferAmount)
                    {
                        sourceClient.Balance -= transferAmount;
                        targetClient.Balance += transferAmount;
                        sourceClient.EmpId =Employee.empID;
                        SaveData();
                        Console.WriteLine($"Transfer of {transferAmount:C} from {sourceClientId} to {targetClientId} successful.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid input for transfer amount. Try again.");
                    }
                }
                else
                {
                    Console.WriteLine($"Target client {targetClientId} not found.");
                }
            }
            else
            {
                Console.WriteLine($"Source client {sourceClientId} not found.");
            }
            RollBack();
        }
        public static void PrintClientInfo()
        {
            Clients=LoadData();
            Console.WriteLine("Enter the client Id ");
            int clientId = int.Parse(Console.ReadLine());
            Client cli=Clients.FirstOrDefault(c => c.Id==clientId);
            if(cli != null)
            {

                Console.WriteLine($"Name:{cli.Name} ");
                Console.WriteLine($"Id: {cli.Id}");
                Console.WriteLine($"Balance:{cli.Balance} ");
                Console.WriteLine($"Is client has Credit acount: {cli.IsDebit}");
            }else 
            {
                Console.WriteLine("client doesn't exist, enter valid id ");
                Employee.PrintClientInfo();
            }
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
                    Bank.EmployeeFunc();

                    break;
            }
           
        }

    }
}
