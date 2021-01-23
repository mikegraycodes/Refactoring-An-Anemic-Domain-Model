using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BankAccounts.Rich
{
    public class Application
    {
        private List<(string KeyboadShortCut, string Prompt, Func<bool> Function)> Commands;

        private readonly BankAccount BankAccount;
        
        public Application()
        {
            BankAccount = BankAccount.Open();
        }
        
        public async Task Run()
        {
            try
            {
                SetupCommands();
                DisplayMenu();
                await WaitForCommand();
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
                WriteLine("Command caused an exception: " + ex.Message, ConsoleColor.Red);
            }
        }

        private void DisplayMenu()
        {
            Console.Clear();
            PrintAppBanner();
            PrintOptions();
        }

        private void PrintAppBanner()
        {
            WriteLine("**********************************");
            WriteLine("***** Bank Account Simulator *****");
            WriteLine("**********************************");
            WriteLine();
        }
        
        private void SetupCommands()
        {
            // General commands.
            this.Commands = new();

            this.AddBannerCommands(this.Commands);
        }
        
        private void AddBannerCommands(List<(string KeyboadShortCut, string Prompt, Func<bool> Function)> commands)
        {
            commands.Add(("Q", "Quit Application", () => { Environment.Exit(0); return true; }));

            commands.Add(("D", "Deposit 50", () =>
            {
                this.BankAccount.DepositFunds(50);
                
                WriteLine($"*********", ConsoleColor.Green);
                WriteLine("Deposited 50");
                WriteLine($"New balance {BankAccount.Balance}");
                WriteLine($"*********", ConsoleColor.Green);

                return true;
            }));

            commands.Add(("W", "Withdraw 50", () =>
            {
                this.BankAccount.WithdrawFunds(50);
                
                WriteLine($"*********", ConsoleColor.Green);
                WriteLine("Withdraw 50");
                WriteLine($"New balance {BankAccount.Balance}");
                WriteLine($"*********", ConsoleColor.Green);
                

                return true;
            }));
            
            commands.Add(("I", "Increase overdraft by 50", () =>
            {
                BankAccount.UpdateOverdraftLimit(BankAccount.OverdraftLimit + 50);
                
                WriteLine($"*********", ConsoleColor.Green);
                WriteLine($"Overdraft Limit Updated To {BankAccount.OverdraftLimit}");
                WriteLine($"*********", ConsoleColor.Green);                
                
                return true;
            }));
            
            commands.Add(("L", "Decrease overdraft by 50", () =>
            {
                BankAccount.UpdateOverdraftLimit(BankAccount.OverdraftLimit - 50);
                    
                WriteLine($"*********", ConsoleColor.Green);
                WriteLine("Overdraft Limit Updated");
                WriteLine($"*********", ConsoleColor.Green);   
                
                return true;
            }));
            
            commands.Add(("C", "Close account", () =>
            {
                BankAccount.Close();
                
                WriteLine($"*********", ConsoleColor.Green);
                WriteLine("Closeed Bank Account");
                WriteLine($"*********", ConsoleColor.Green);
                
                return true;
            }));
            
            commands.Add(("R", "ReOpen account", () =>
            {
                BankAccount.ReOpen();
                
                WriteLine($"*********", ConsoleColor.Green);
                WriteLine("Re-Opened Bank Account");
                WriteLine($"*********", ConsoleColor.Green);
                
                return true;
            }));

            // Null command to print a gap in the menu options.
            commands.Add((string.Empty, string.Empty, () => true));
        }

        private void PrintOptions()
        {
            WriteLine();
            WriteLine("Select an option");
            foreach (var cmd in Commands)
                if (cmd.KeyboadShortCut != string.Empty)
                    WriteLine($"{cmd.KeyboadShortCut}: {cmd.Prompt}");
                else
                    WriteLine();
            WriteLine();
        }

        private static void WriteLine(string output = null, ConsoleColor colour = ConsoleColor.White)
        {
            if (output != null)
            {
                Console.ForegroundColor = colour;
                Console.WriteLine(output);
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.WriteLine();
            }
        }
        
        private async Task WaitForCommand()
        {
            while (true)
            {
                var isValidCommand = false;
                while (isValidCommand == false)
                {
                    var keyboardInput = Console.ReadLine();

                    isValidCommand = Commands.Any(c =>
                        c.KeyboadShortCut.Equals(keyboardInput, StringComparison.OrdinalIgnoreCase));

                    if (isValidCommand)
                    {
                        try
                        {
                            var cmdResult = Commands
                                .First(c => c.KeyboadShortCut.Equals(keyboardInput, StringComparison.OrdinalIgnoreCase))
                                .Function();
                        }
                        catch (InvalidOperationException e)
                        {
                            WriteLine($"*********", ConsoleColor.Red);
                            WriteLine(e.Message);
                            WriteLine($"*********", ConsoleColor.Red);     
                        }

                    }
                }
            }
        }
    }
}