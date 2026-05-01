using System.IO;
using System.Runtime.Serialization.Json;
using System.Text.Json;
using System.Xml.Serialization;

namespace SerializationExample
{

    [Serializable]
    public class Person
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTime Birth { get; set; }
        public string Ema { get; set; } = string.Empty;

        public Person()
        {
        }

        public Person(string name, int age, DateTime birth, string ema)
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
            Person person = new("John Doe", 30, new DateTime(1994, 5, 15), "example@gmail.com");

            string json = JsonSerializer.Serialize(person);



            //XmlSerializer ser = new XmlSerializer(typeof(Person));

            //using (TextWriter sw = new StreamWriter("example.xml"))
            //{
            //    ser.Serialize(sw, person);

            //}


            //using (TextReader sr = new StreamReader("example.xml"))
            //{
            //    Person deserializedPerson = (Person)ser.Deserialize(sr);
            //    Console.WriteLine($"Name: {deserializedPerson.Name}, Age: {deserializedPerson.Age}, Birth: {deserializedPerson.Birth}, Email: {deserializedPerson.Ema}");

            //}



            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Person));

            using (MemoryStream ms = new MemoryStream())
            {
                jsonSerializer.WriteObject(ms, person);
                ms.Position = 0;
                StreamReader sr = new StreamReader(ms);
                string jsonData = sr.ReadToEnd();
                File.WriteAllText("example.json", jsonData);

            }

        }
    }
}
