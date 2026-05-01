namespace Reflection
{

    //public class MyClass
    //{
    //    public string Name { get; set; }

    //    public int Id { get; set; }

    //    public void MyMethod()
    //    {
    //        Console.WriteLine("This is a method in MyClass.");
    //    }

    //    public MyClass(string name, int id) 
        
    //    {
    //         Name = name;
    //        Id = id;
    //    }
    //}

    internal class Program
    {
        static void Main(string[] args)
        {

            Type KnowType = typeof(string);


            Console.WriteLine("this is Full Name : {0}",KnowType.FullName);
            Console.WriteLine("this is Name : {0}",KnowType.Name);
            Console.WriteLine("this is Namespace : {0}",KnowType.Namespace);

        }
    }
}
