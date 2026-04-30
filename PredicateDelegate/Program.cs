namespace PredicateDelegate
{
    internal class Program
    {
        static Predicate<int> PrDelagate = IsEven;

        public static bool IsEven(int number)
        {
            return number % 2 == 0;
        }

        static void Main(string[] args)
        {
            Console.WriteLine(PrDelagate(0));
            Console.WriteLine(PrDelagate(91));
        }
    }
}
