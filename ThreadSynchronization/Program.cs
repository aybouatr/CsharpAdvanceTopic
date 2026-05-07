namespace ThreadSynchronization
{


    internal class Program
    {

        public static int Counter = 0;

        static object lockObject = new object();


        static void Main(string[] args)
        {

            Thread t1 = new Thread(() => IncrementCounter("1"));
            Thread t2 = new Thread(() => IncrementCounter("2"));
            
            t1.Start();
            t2.Start();
            
            t1.Join();
            t2.Join();
            
            Console.WriteLine("Final Counter Value: " + Counter);


        }

        public static int AddTwo(int value)
        {
            return value;
        }

        public static void IncrementCounter(string name)
        {
            for (int i = 0; i < 1000; i++)
            {
                //lock (lockObject)
                //{
                    Counter = AddTwo(Counter + 1);
                    Console.WriteLine(@"Counter: {0 } thread is : {1}", Counter, name);
                //}
            }
        }

    }
}
