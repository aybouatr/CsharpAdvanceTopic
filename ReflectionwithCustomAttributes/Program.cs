namespace ReflectionwithCustomAttributes
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class MyCustomAttribute : Attribute
    {
        public string Description { get; }

        public MyCustomAttribute(string description)
        {
            Description = description;
        }

    }

    [MyCustomAttribute("This is a custom attribute for the MyClass class")]
   public class MyClass
   {
        [MyCustomAttribute("This is a custom method attribute")]
        public void MyMethod()
        {
            Console.WriteLine("Executing MyMethod");
        }
   }

    internal class Program
    {
        static void Main(string[] args)
        {
            Type myClassType = typeof(MyClass);

            
            Object[] classAttributes = myClassType.GetCustomAttributes(typeof(MyCustomAttribute), false);

            foreach (MyCustomAttribute attr in classAttributes)
            {
                Console.WriteLine($"Class Attribute Description: {attr.Description}");
            }
            // Get method information

            Console.WriteLine("\n--- Method Attributes ---");

            var methods = myClassType.GetMethods();

            foreach (var method in methods)
            {
                var methodAttributes = method.GetCustomAttributes(typeof(MyCustomAttribute), false);
                foreach (MyCustomAttribute attr in methodAttributes)
                {
                    Console.WriteLine($"\nMethod Attribute Description: {attr.Description}");
                }
            }
        }
    }
}
