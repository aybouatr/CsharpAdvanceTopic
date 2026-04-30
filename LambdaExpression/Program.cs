namespace LambdaExpression
{
    internal class Program
    {
        // expresion lambda

            static public Func<int, int> square = x => x * x;
    
            static public Func<int, int, int> add = (x, y) => x + y;
    
            static public Action<string> printMessage = message => Console.WriteLine(message);

        static void Main(string[] args)
        {

            printMessage("Hello, Lambda Expressions!");
            Console.WriteLine("Square of 5: " + square(5));
            Console.WriteLine("Sum of 3 and 4: " + add(3, 4));

        }
    }
}
