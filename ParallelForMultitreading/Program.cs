using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading;


namespace ParallelForMultitreading
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int numberOfIterations = 10;

            // Using Parallel.For to execute iterations in parallel
            //Parallel.For(0, numberOfIterations, i =>
            //{
            //    Console.WriteLine($"Iteration {i} is running on thread {Task.CurrentId}");
            //    // Simulate work by sleeping for a random time
            //    Random rand = new Random();
            //    int sleepTime = rand.Next(500, 2000);
            //    Task.Delay(sleepTime).Wait();
            //    Console.WriteLine($"Iteration {i} completed after sleeping for {sleepTime} ms");
            //});

            // Alternatively, you can define a separate method and call it from Parallel.For
            //Parallel.For(0,numberOfIterations,Method1);


            //List<string> urls = new List<string>
            //{
            //    "https://www.cnn.com",
            //    "https://www.amazon.com",
            //    "https://www.programmingadvices.com"
            //};

            //    Parallel.ForEach(urls, url =>
            //    {
            //       DowloadDromWeb(url);
            //    });
            
            Parallel.Invoke(
                () => DowloadDromWeb("https://www.cnn.com"),
                () => DowloadDromWeb("https://www.amazon.com"),
                () => DowloadDromWeb("https://www.programmingadvices.com")
            );


        }
    
        public static void DowloadDromWeb(string url)
        {
            string Content = string.Empty;
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("User-Agent", "Mozilla/5.0");
                Task.Delay(2000).Wait(); // Simulate a time-consuming download operation
                Content = client.DownloadString(url);
                Console.WriteLine($"Downloaded content from {url} Content length: {Content.Length}");
            }

        }

        public static void Method1(int i)
        {
            Console.WriteLine($"Method 1 is running for iteration {i}.");
            Task.Delay(1000).Wait(); // Simulate work
            Console.WriteLine($"Method 1 completed for iteration {i}.");
        }
    }
}
