using System.Diagnostics;
using Bank_System;
using Newtonsoft.Json;
using Spectre.Console;
namespace Main
{
    internal class Program
    {
        static void Main()
        {
            Console.Clear();
            Console.Title = "Bank System";

            // Welcome
            AnsiConsole.Write(new FigletText("Welcome To Our Bank").Color(Color.Yellow).Centered());
            //Bank System or ATM
            var userInput = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
           .Title("select option from:")
           .PageSize(3)
           .AddChoices(new[] {
            "1- Bank System", "2- ATM",
        }));
            switch (userInput)
            {
                case "1- Bank System":
                    Bank.BankSystem();
                    break;
                case "2- ATM":
                    Client.Login();
                    Bank.Atm();
                    break;
            }
        }  
    }
}