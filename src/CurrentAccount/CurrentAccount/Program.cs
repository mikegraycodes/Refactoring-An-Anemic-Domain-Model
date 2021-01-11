using System.Threading.Tasks;

namespace CurrentAccount
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            await new Application().Run();
        }
    }
}