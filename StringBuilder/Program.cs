using System;
using System.Text;
using System.Diagnostics;



    internal class Program
    {
        public static void DemonstrateStringBuilder(int iterations)
        {
             StringBuilder sb = new StringBuilder();
            while (iterations > 0)
            {
                sb.Append("Hello, World! ");
                iterations--;
            }
        }

        public static void DemonstrateStringNormal(int iterations)
        {
            string result = "";
            while (iterations > 0)
            {
                result += "Hello, World! ";
                iterations--;
            }
        }

        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            DemonstrateStringBuilder(20000);
            stopwatch.Stop();

            Console.WriteLine("StringBuilder time: " + stopwatch.ElapsedMilliseconds + " ms");

            Stopwatch stopwatch2 = new Stopwatch();

            stopwatch2.Start();
            DemonstrateStringNormal(20000);
            stopwatch2.Stop();

            Console.WriteLine("String concatenation time: " + stopwatch2.ElapsedMilliseconds + " ms");
        }
    }

