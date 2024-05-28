using Main;
using Newtonsoft.Json;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_System
{
    static class Bank
    {

        private static List<AtmLocattion> AtmLocations {  get; set; }
        public const string Atmfile = @"D:\ITI\\Bank_System\ATM.json";

        private  class AtmLocattion
        {
            public string AtmLocation { get; set; }
            public int AtmID { get; set; }
            public AtmLocattion(string location,int id)
            {
                this.AtmID = id;
                this.AtmLocation = location;
            }
        }


        private static void AtmSaveData()
        {
            var json=JsonConvert.SerializeObject(AtmLocations, Formatting.Indented);
            File.WriteAllText(Atmfile, json);
        }
        private static List<AtmLocattion> AtmLoadData()
        {
            if (File.Exists(Atmfile))
            {
                var file=new FileInfo(Atmfile);
                if(file.Length > 0)
                {
                    var json = File.ReadAllText(Atmfile);
                    return JsonConvert.DeserializeObject<List<AtmLocattion>>(json);
                }
            }
            return new List<AtmLocattion>();
        }
        private static void AddAtm()
        {
            AtmLocations=AtmLoadData();
            Console.WriteLine("enter the ATM location");
            string atmLocation=Console.ReadLine();
            Console.WriteLine("enter the ATM id");
            int atmID=int.Parse(Console.ReadLine());
            if(AtmLocations.Any(a=> a.AtmID == atmID))
            {
                Console.WriteLine("the ATM with this ID exist");
                return;
            }
            AtmLocations.Add(new AtmLocattion(atmLocation, atmID));
            AtmSaveData();
            Console.WriteLine($"the ATM with {atmID} added successfully\n");
            Manager.RollBack();
        }
        private static void DeleteAtm()
        {
            AtmLocations = AtmLoadData();
            Console.WriteLine("enter the Atm ID to remove");
            int id=int.Parse(Console.ReadLine());
            AtmLocattion atm = AtmLocations.FirstOrDefault(a => a.AtmID == id);

            if (atm != null)
            {
                AtmLocations.Remove(atm);
                AtmSaveData();
                Console.WriteLine($"the ATM with {atm.AtmID} deleted successfully");
            }
            else
            {
                Console.WriteLine("invalid ATM ID try again");
                DeleteAtm();
            }
            Manager.RollBack();
        }
        public static void ShowAtmLocation()
        {
            AtmLocations = AtmLoadData();
            foreach (var atm in AtmLocations)
                Console.WriteLine(atm.AtmLocation);
        }
        public static void Atm()

        {
            AnsiConsole.Write(new FigletText($"ohayo {Employee.Clients.FirstOrDefault(c => c.Id == Client.CurrentClientId).Name}").Color(Color.Yellow).Centered());

            var userInput = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
           .Title("select option from:")
           .PageSize(5)
           .AddChoices(new[] {
            "1- Wihdraw", "2- Deposit","3- Transfer","4- Check Balance",
        }));
            switch (userInput)
            {
                case "1- Wihdraw":
                    Client.Withdraw();
                    break;
                case "2- Deposit":
                    Client.Deposit();
                    break;
                case "3- Transfer":
                    Client.TransferToAtm();
                    break;
                case "4- Check Balance":
                    Client.GetBalanceAtm();
                    break;

            }
        }
        public static void BankSystem()
        {
            var userInput = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
           .Title("select option from:")
           .PageSize(3)
           .AddChoices(new[] {
            "1- Client", "2- Employee","3- Manager",
        }));
            switch (userInput)
            {
                case "1- Client":
                    Client.Login();
                    ClientFunc();
                    break;
                case "2- Employee":
                    Employee.Login();
                    EmployeeFunc();
                    break;
                case "3- Manager":
                    Manager.Login();
                    ManagerFunc();
                    break;
            }
        }
        public static void ManagerFunc()
        {
            AnsiConsole.Write(new FigletText($"ohayo {Manager.Managers.FirstOrDefault(m => m.Id == Manager.ManagerID).Name}").Color(Color.Yellow).Centered());

            var userInput = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
          .Title("select option from:")
          .PageSize(6)
          .AddChoices(new[] {
            "1- Add employee", "2- Remove employee","3- Add Atm","4- Remove Atm","5- update salary for employee","6- Previous Menu",
       }));

            switch (userInput)
            {
                case "1- Add employee":
                    Manager.AddEmployee();
                    break;
                case "2- Remove employee":
                    Manager.DeleteEmployee();
                    break;
                case "3- Add Atm":
                    Bank.AddAtm();
                    break; 
                case "4- Remove Atm":
                    Bank.DeleteAtm();
                    break;
                case "5- update salary for employee":
                    Manager.UpdateSalary();
                    break;
                case "6- Previous Menu":
                    BankSystem();
                    break;
            }
        }
        public static void EmployeeFunc()
        {
            AnsiConsole.Write(new FigletText($"ohayo {Manager.employees.FirstOrDefault(e => e.Id == Employee.empID).Name}").Color(Color.Yellow).Centered());
            var userInput = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
          .Title("select option from:")
          .PageSize(7)
          .AddChoices(new[] {
            "1- Add Account", "2- Remove Account","3- Withdraw","4- Deposit","5- Transfer","6- Show Client info","7- Previous Menu",
       }));
            switch (userInput)
            {
                case "1- Add Account":
                    Employee.AddClient();
                    break;

                case "2- Remove Account":
                    Employee.DeleteClient();
                    break;

                case "3- Withdraw":
                    Employee.Withdraw();

                    break;

                case "4- Deposit":
                    Employee.Deposit();
                    break;

                case "5- Transfer":

                    Employee.TransferTo();

                    break;

                case "6- Show Client info":

                    Employee.PrintClientInfo();
                    break;
                case "7- Previous Menu":
                    BankSystem();
                    break;

            }
        }
        public static void ClientFunc()
        {
            AnsiConsole.Write(new FigletText($"ohayo {Employee.Clients.FirstOrDefault(e => e.Id == Client.CurrentClientId).Name}").Color(Color.Yellow).Centered());

            var userInput = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
          .Title("select option from:")
          .PageSize(5)
          .AddChoices(new[] {
            "1- Transfer", "2- Show Balance","3- ATM Locations","4- change ID or password","5- Previous Menu",
       }));


            switch (userInput)
            {
                case "1- Transfer":
                    Client.TransferToSystem();
                    break;
                case "2- Show Balance":
                    Client.GetBalanceSystem();
                    break;
                case "3- ATM Locations":
                    ShowAtmLocation();
                    break;
                case "4- change ID or password":
                    Client.UpdatePassOrID();
                    break;
                case "5- Previous Menu":
                    BankSystem();
                    break;
            }
        }

    }
}
