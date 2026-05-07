using System;
using System.Threading.Tasks;
namespace TaskRun
{
    internal class Program
    {

        public static void DowloadDataDeomWeb(string url)
            {
                Console.WriteLine($"Downloading data from {url}...");
                // Simulate a time-consuming download operation
                Task.Delay(2000).Wait();
                Console.WriteLine($"Finished downloading data from {url}.");
        }

        static async Task Main(string[] args)
        {
            Task t1 = Task.Run(() => DowloadDataDeomWeb("Download From Amazon"));
            Task t2 = Task.Run(() => DowloadDataDeomWeb("Download From CNN"));
            Task t3 = Task.Run(() => DowloadDataDeomWeb("Download From Programming Advice"));

            await Task.WhenAll(t1, t2, t3);
    
            Console.WriteLine("All downloads completed.");
        }
    }
}
