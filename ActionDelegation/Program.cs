namespace ActionDelegation
{
    internal class Program
    {

            static public Action<string> action = Method1;
            static public Action<string> action2 = Method2;
    
            public static void Method1(string message)
            {
                Console.WriteLine("Method1: " + message);
            }

            public static void Method2(string message)
            {
                Console.WriteLine("Method2: " + message);
            }

        static void Main(string[] args)
        {
            action("Hello, Action Delegation!");
            action2("Hello, Action Delegation 2!");
        }
    }
}
