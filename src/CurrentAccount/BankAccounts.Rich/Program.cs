using System.Threading.Tasks;

namespace BankAccounts.Rich
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            await new Application().Run();
        }
    }
}