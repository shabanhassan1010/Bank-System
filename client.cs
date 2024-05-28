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
    public class Client : Person
    {
        //Atttibutes
        public double Balance { get; set; }
        public bool IsDebit { get; set; }
        public static int CurrentClientId { get; set; }
        public int EmpId { get; set; }

        //Static Attribute



        //Constructor        
        public Client(int id, string name, string password, double balance, bool isDebit, int empId) : base(id, name, password)
        {

            this.Balance = balance;
            this.IsDebit = isDebit;
            this.EmpId = empId;
        }

        //Methods

        public static void Login()
        {
            Employee.Clients = Employee.LoadData();
            AnsiConsole.Write("Enter your Id: ");
            CurrentClientId = int.Parse(Console.ReadLine());
            Console.Write("Enter your password: ");
            string pass = Console.ReadLine();
            if (Employee.Clients.Any(c => c.Id == CurrentClientId) && Employee.Clients.Any(c => c.Password == pass))
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



        public static void Withdraw()
        {
            Employee.Clients = Employee.LoadData();


            Console.Write("Enter the amount for withdrawa : ");
            if (double.TryParse(Console.ReadLine(), out double withdrawalAmount) && Employee.Clients.FirstOrDefault(c => c.Id == CurrentClientId).Balance > withdrawalAmount)
            {
                Employee.Clients.FirstOrDefault(c => c.Id == CurrentClientId).Balance -= withdrawalAmount;
                Employee.SaveData();
                Console.WriteLine($"Withdrawa of {withdrawalAmount:C} successful.");
            }
            else
            {
                Console.WriteLine("Invalid input for withdrawal amount. Try again.");
            }

            RollBackAtm();
        }

        public static void Deposit()
        {
            Employee.Clients = Employee.LoadData();
            Console.Write("Enter deposit amount: ");
            double depositAmount;
            if (double.TryParse(Console.ReadLine(), out depositAmount))
            {
                Employee.Clients.FirstOrDefault(c => c.Id == CurrentClientId).Balance += depositAmount;
                Employee.SaveData();
                Console.WriteLine($"Deposit of {depositAmount:C} successful.");
            }
            else
            {
                Console.WriteLine("Invalid input for deposit amount. Try again.");
            }
            RollBackAtm();
        }
        public static void TransferTo()
        {
            Employee.Clients = Employee.LoadData();
            Console.Write("Enter the Account ID: ");
            int targetClientId = int.Parse(Console.ReadLine());
            Client targetClient = Employee.Clients.FirstOrDefault(c => c.Id == targetClientId);
            if (targetClient != null)
            {
                Console.Write("Enter transfer amount: ");
                if (double.TryParse(Console.ReadLine(), out double transferAmount) && Employee.Clients.FirstOrDefault(c => c.Id == CurrentClientId).Balance > transferAmount)
                {
                    Employee.Clients.FirstOrDefault(c => c.Id == CurrentClientId).Balance -= transferAmount;
                    targetClient.Balance += transferAmount;
                    Employee.SaveData();
                    Console.WriteLine($"Transfer of {transferAmount:C} to {targetClientId} successful.");
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
        public static void TransferToSystem()
        {
            TransferTo();
            RollBackSystem();
        }
        public static void TransferToAtm()
        {
            TransferTo();
            RollBackAtm();
        }

        public static void GetBalanceSystem()
        {
            Console.WriteLine($"your current Balance : {Employee.Clients.FirstOrDefault(c => c.Id == CurrentClientId).Balance}");
            RollBackSystem();
        }
        public static void GetBalanceAtm()
        {
            Console.WriteLine($"your current Balance : {Employee.Clients.FirstOrDefault(c => c.Id == CurrentClientId).Balance}");
            RollBackAtm();
        }
        public static void UpdatePassOrID() 
        {
            Employee.Clients = Employee.LoadData();
            var userInput = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
           .Title("select option from:")
           .PageSize(3)
           .AddChoices(new[] {
            "1- Update password", "2- Update ID","3- Previouse Menu",
        }));
            switch (userInput)
            {
                case "1- Update password":
                    Console.WriteLine("Enter the new Password: ");
                    Employee.Clients.FirstOrDefault(c => c.Id == CurrentClientId).Password = Console.ReadLine();
                    break;
                case "2- Update ID":
                    Console.WriteLine("Enter the new ID: ");
                    Employee.Clients.FirstOrDefault(c => c.Id == CurrentClientId).Id=int.Parse(Console.ReadLine());
                    break;
                case "3- Previouse Menu":
                    Console.Clear();
                    Bank.ClientFunc();
                    break;
            }
            Employee.SaveData();
            var clientInput = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
           .Title("select option from:")
           .PageSize(3)
           .AddChoices(new[] {
            "1- Exit", "2- Previous Menu",
        }));
            switch (clientInput)
            {
                case "1- Exit":
                    Console.Clear();
                    AnsiConsole.Write(new FigletText("THX For using our System").Color(Color.Yellow).Centered());
                    Environment.Exit(0);

                    break;
                case "2- Previous Menu":
                    Client.UpdatePassOrID();

                    break;
            }
        }
        public static void RollBackSystem()
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
                    AnsiConsole.Write(new FigletText("THX For using our System").Color(Color.Yellow).Centered());
                    Environment.Exit(0);

                    break;
                case "2- Previous Menu":
                    Console.Clear();
                    Bank.ClientFunc();

                    break;
            }
            

        }
        public static void RollBackAtm()
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
                    AnsiConsole.Write(new FigletText("THX For using our System").Color(Color.Yellow).Centered());
                    Environment.Exit(0);

                    break;
                case "2- Previous Menu":
                    Console.Clear();
                    Bank.Atm();

                    break;
            }


        }
    }
}
