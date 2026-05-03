namespace Generics
{

    public class GenericClass<T>
    {
        public T Value { get; set; }
        public GenericClass(T value)
        {
            Value = value;
        }
        public void Display()
        {
            Console.WriteLine($"Value: {Value}");
        }
    }

    internal class Program
    {

        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        static void Main(string[] args)
        {
           string x = "Hello";
            string y = "World";
            Console.WriteLine($"Before swap: x = {x}, y = {y}");
            Swap(ref x, ref y);
            Console.WriteLine($"After swap: x = {x}, y = {y}");
            int a = 5;
            int b = 10;
            Console.WriteLine($"\nBefore swap: a = {a}, b = {b}");
            Swap(ref a, ref b);
            Console.WriteLine($"After swap: a = {a}, b = {b}");

                GenericClass<int> intInstance = new GenericClass<int>(42);

            Console.WriteLine("\n\nGenericClass with int:");
            intInstance.Display();
        
        GenericClass<string> stringInstance = new GenericClass<string>("Hello Generics");
            Console.WriteLine("\nGenericClass with string:");
            stringInstance.Display();
        }
    }
}
