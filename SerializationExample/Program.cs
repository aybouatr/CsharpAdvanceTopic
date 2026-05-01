using System.Runtime.InteropServices.JavaScript;

namespace SerializationExample
{


    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public DateOnly Birth { get; set; }

        public string Ema { get; set; }


        public Person(string name, int age, DateOnly birth, string ema)
        {
            Name = name;
            Age = age;
            Birth = birth;
            Ema = ema;
        }


    }
    internal class Program
    {
        static void Main(string[] args)
        {

            Person person = new Person("John Doe", 30, new DateOnly(1994, 5, 15), "example@gmail.com");
        
            string json = System.Text.Json.JsonSerializer.Serialize(person);

            Console.WriteLine(json);
        }
                
    }
}
