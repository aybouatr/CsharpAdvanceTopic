namespace FuncDelegation
{
    internal class Program
    {

        static public Func<string> func = Method1;

        static public Action<string> func2 = Method2;

        public static string Method1()
        {
            return "Method1: Hello, World!";
        }

        public static void Method2(string message)
        {

            Console.WriteLine("Method2: " + message);

        }

        static void Main(string[] args)
        {
            Console.WriteLine(func());
            func2("Hello, Func Delegation!");
        }
    }
}
