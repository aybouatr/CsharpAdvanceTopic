using System;
using System.Threading;
using System.Threading.Tasks;


namespace TaskFactorY
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;


            TaskFactory factory = new TaskFactory( token
                , TaskCreationOptions.AttachedToParent
                , TaskContinuationOptions.ExecuteSynchronously
                , TaskScheduler.Default);

            Task task1 = factory.StartNew(() =>
            {
                Console.WriteLine("Task 1 started.");
                Thread.Sleep(2000); // Simulate work
                Console.WriteLine("Task 1 completed.");
            });


                Task task2 = factory.StartNew(() =>
                {
                    Console.WriteLine("Task 2 started.");
                    Thread.Sleep(3000); // Simulate work
                    Console.WriteLine("Task 2 completed.");
                });
    
                Task task3 = factory.StartNew(() =>
                {
                    Console.WriteLine("Task 3 started.");
                    Thread.Sleep(1000); // Simulate work
                    Console.WriteLine("Task 3 completed.");
                });

            Task.WhenAll(task1, task2, task3).ContinueWith(t =>
            {
                Console.WriteLine("All tasks completed.");
            });

            Console.ReadLine();


        }
    }
}
