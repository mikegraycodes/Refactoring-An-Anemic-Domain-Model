using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CurrentAccount.AnemicDomainModel;

namespace CurrentAccount
{
    public class Application
    {
        private List<(string KeyboadShortCut, string Prompt, Func<bool> Function)> Commands;

        private readonly BankAccount BankAccount;
        
        public Application()
        {
            BankAccount = new BankAccount
            {
                Id = Guid.NewGuid(),
                IsOpen = true
            };
        }
        
        public async Task Run()
        {
            try
            {
                await this.SetupCommands();
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
        
        private async Task SetupCommands()
        {
            // General commands.
            this.Commands = new();

            this.AddBannerCommands(this.Commands);
            int keyCode = 1;
        }
        
        private void AddBannerCommands(List<(string KeyboadShortCut, string Prompt, Func<bool> Function)> commands)
        {
            commands.Add(("Q", "Quit Application", () => { Environment.Exit(0); return true; }));

            commands.Add(("D", "Deposit 50", () =>
            {
                this.BankAccount.Balance += 50;
                
                WriteLine($"*********", ConsoleColor.Green);
                WriteLine("Deposited 50");
                WriteLine($"New balance {BankAccount.Balance}");
                WriteLine($"*********", ConsoleColor.Green);

                return true;
            }));

            commands.Add(("W", "Withdraw 50", () =>
            {
                if (this.BankAccount.Balance - 50 < BankAccount.OverdraftLimit)
                {
                    WriteLine($"*********", ConsoleColor.Red);
                    WriteLine("Failed to withdraw 50 as balance would be below overdraft limit");
                    WriteLine($"*********", ConsoleColor.Red);
                }
                else
                {
                    this.BankAccount.Balance -= 50;
                    
                    WriteLine($"*********", ConsoleColor.Green);
                    WriteLine("Withdraw 50");
                    WriteLine($"New balance {BankAccount.Balance}");
                    WriteLine($"*********", ConsoleColor.Green);
                }

                return true;
            }));
            
            commands.Add(("I", "Increase overdraft by 50", () =>
            {
                BankAccount.OverdraftLimit -= 50;
                
                WriteLine($"*********", ConsoleColor.Green);
                WriteLine($"Overdraft Limit Updated To {BankAccount.OverdraftLimit}");
                WriteLine($"*********", ConsoleColor.Green);                
                
                return true;
            }));
            
            commands.Add(("D", "Decrease overdraft by 50", () =>
            {
                if (BankAccount.OverdraftLimit + 50 >= 0)
                {
                    WriteLine($"*********", ConsoleColor.Red);
                    WriteLine("Overdraft Limit cannot be above 0");
                    WriteLine($"*********", ConsoleColor.Red);     
                }
                else
                {
                    BankAccount.OverdraftLimit -= 50;
                    
                    WriteLine($"*********", ConsoleColor.Green);
                    WriteLine("Overdraft Limit Updated");
                    WriteLine($"*********", ConsoleColor.Green);   
                }
                
                return true;
            }));
            
            commands.Add(("C", "Close account", () =>
            {
                if (BankAccount.IsOpen && BankAccount.Balance > 0)
                {
                    BankAccount.IsOpen = false;
                    
                    WriteLine($"*********", ConsoleColor.Green);
                    WriteLine("Closeed Bank Account");
                    WriteLine($"*********", ConsoleColor.Green);         
                }
                else
                {
                    WriteLine($"*********", ConsoleColor.Red);
                    WriteLine("Failed To Close Bank Account");
                    WriteLine($"*********", ConsoleColor.Red);     
                }
                

                
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
                        var cmdResult = Commands
                            .First(c => c.KeyboadShortCut.Equals(keyboardInput, StringComparison.OrdinalIgnoreCase))
                            .Function();
                    }
                }
            }
        }
    }
}