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

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class  ValadationRangeAttribute : Attribute
    {
        public int Min { get; }

        public int Max { get; }

        public string ErrorMessage { get; set; }

        public ValadationRangeAttribute(int min, int max, string errorMessage)
        {
            Min = min;
            Max = max;
            ErrorMessage = errorMessage;
        }
    }

    /// <summary>
    /// this class is used to demonstrate how to use the ValadationRangeAttribute to validate the properties of a class using reflection. The Person class has three properties: Age, Name, and Expirence. The Age and Expirence properties are decorated with the ValadationRangeAttribute to specify the valid range for these properties. 
    /// The ValidatePerson method uses reflection to read the custom attributes from the Person class and validate the properties based on the specified ranges.
    /// </summary>

    public class Person
    {
        /// <summary>
        /// Gets or sets the age of the individual.
        /// </summary>
        [ValadationRange(18, 120,  "Age must be between 18 and 120")]
        public int Age { get; set; }

        public string Name { get; set; }

        [ValadationRange(10, 30, "Experience must be between 10 and 30")]
        public int Expirence { get; set; }

        public Person(string name, int age, int expirence)
        {
            Name = name;
            Age = age;
            Expirence = expirence;
        }

    }

    internal class Program
    {

        // 1 ===>  // this first example is to show how to use reflection to read custom attributes from a class and its method.
        //static void Main(string[] args)
        //{
        //    Type myClassType = typeof(MyClass);


        //    Object[] classAttributes = myClassType.GetCustomAttributes(typeof(MyCustomAttribute), false);

        //    foreach (MyCustomAttribute attr in classAttributes)
        //    {
        //        Console.WriteLine($"Class Attribute Description: {attr.Description}");
        //    }
        //    // Get method information

        //    Console.WriteLine("\n--- Method Attributes ---");

        //    var methods = myClassType.GetMethods();

        //    foreach (var method in methods)
        //    {
        //        var methodAttributes = method.GetCustomAttributes(typeof(MyCustomAttribute), false);
        //        foreach (MyCustomAttribute attr in methodAttributes)
        //        {
        //            Console.WriteLine($"\nMethod Attribute Description: {attr.Description}");
        //        }
        //    }
        //}

        public static bool ValidatePerson(Person person)
        {
            Type TypePerson = typeof(Person);

            var properties = TypePerson.GetProperties();

            foreach (var property in properties)
            {
                var validationAttributes = property.GetCustomAttributes(typeof(ValadationRangeAttribute), false);
                foreach (ValadationRangeAttribute attr in validationAttributes)
                {
                    int value = (int)property.GetValue(person);
                    if (value < attr.Min || value > attr.Max)
                    {
                        Console.WriteLine(attr.ErrorMessage);
                        return false;
                    }
                }
            }
            return true;

        }

        // 2 ===> this second example is to show how to use reflection to read custom attributes from a class and its method, but this time we will use the new C# 14.0 feature called "Attributes on Local Functions" to apply the custom attribute directly to a local function inside the Main method.
        static void Main(string[] args)
        {
           
            Person P = new Person("John", 22, 5);

            P.Age = 25; // Valid age
            P.Expirence = 15; // Valid experienc
            if (ValidatePerson(P))
            {
                Console.WriteLine("Person is valid.");
            }
            else
            {
                    Console.WriteLine("Person is not valid.");
            }

        }

      

    }
}
