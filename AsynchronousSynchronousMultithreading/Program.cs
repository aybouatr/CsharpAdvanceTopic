using System;
using System.Net;
using System.Threading;

namespace AsynchronousSynchronousMultithreading
{
    internal class Program
    {

        static public void MyThreadMethod()

        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} is running iteration {i}");
                Thread.Sleep(1000); // Simulate work by sleeping for 1 second
            }
        }

        static public void MyThreadMethodParamaters(string arg)

        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} is running iteration {i} Name : {arg}");
                Thread.Sleep(1000); // Simulate work by sleeping for 1 second
            }
        }

        static public void FinishDoeloadAmazon()
        {
            Console.WriteLine("Finished downloading from Amazon");
        }

        static public void FinishDoeloadCNN()
        {
            Console.WriteLine("Finished downloading from CNN");
        }

        static public void FinishDoeloadProgrammingAdvice()
        {
            Console.WriteLine("Finished downloading from Programming Advice");
        }

        static void Main(string[] args)
        {
            if (false)
            {
                Thread t = new Thread(MyThreadMethod);

                t.Start();
                //MyThreadMethod();
                int i = 0;
                while (i < 40)
                {
                    Console.WriteLine($"Main thread is running iteration {i}");
                    Thread.Sleep(1000); // Simulate work by sleeping for 1 second

                    i++;
                }
            }

            // thread paramaters

            if (false)
            {
                Console.WriteLine("========================== Threads paramaters =================================================\n");

                Thread t2 = new Thread(() => MyThreadMethodParamaters("Thread2"));

                t2.Start();
                //MyThreadMethod();
                int j = 0;
                while (j < 40)
                {
                    Console.WriteLine($"Main thread is running iteration {j}");
                    Thread.Sleep(1000); // Simulate work by sleeping for 1 second

                    j++;
                }
            }

            if (true)
            {
                Console.WriteLine("==========================Web Pages using Multi Threading =================================================\n");

                Thread t = new Thread(() => DownloadDataFromWeb("http://www.cnn.com", FinishDoeloadCNN));
                t.Start();

                Thread t1 = new Thread(() => DownloadDataFromWeb("http://www.Amazon.com", FinishDoeloadAmazon));
                t1.Start();

                Thread t2 = new Thread(() => DownloadDataFromWeb("http://www.programmingAdvice.com", FinishDoeloadProgrammingAdvice));
                t2.Start();

                t.Join();
                t1.Join();
                t2.Join();

            }
        }

        static public void DownloadDataFromWeb(string url,Action callback)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("User-Agent", "Mozilla/5.0");

                    Thread.Sleep(2000); // simulate delay
                    string content = client.DownloadString(url);

                    callback?.Invoke();
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine($"Error for {url}: {ex.Message}");
            }

        }
    }
}
