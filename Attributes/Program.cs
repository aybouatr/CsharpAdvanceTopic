using System.Diagnostics;

namespace Attributes
{
    internal class Program
    {

        public class MyAttribute
        {
        

            [Conditional("DEBUG")]
            public void DEBUGMethod()
            {
                Console.WriteLine("This is a DEBUG method in MyAttribute.");
            }

            public void NormalMethod()
            {
                Console.WriteLine("This is a normal method in MyAttribute.");
            }

          
        }


        static void Main(string[] args)
        {
            MyAttribute myAttr = new MyAttribute();

            //Console.WriteLine($"Name: {myAttr.Name}, Value: {myAttr.Value}");

            myAttr.DEBUGMethod();
            myAttr.NormalMethod();


        }

    }
}
