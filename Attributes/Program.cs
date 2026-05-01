using System.Diagnostics;

namespace Attributes
{
    internal class Program
    {

        public class MyAttribute
        {
        

            [Conditional("Condition")]
            public void DEBUGMethod()
            {
                Console.WriteLine("This is a DEBUG method in MyAttribute.");
            }

            public void NormalMethod()
            {
                Console.WriteLine("This is a normal method in MyAttribute.");
            }

            [Obsolete("This method is obsolete. Use another method instead.")]
            public void MethodAbsolute()
            {
                Console.WriteLine("This method will always be called.");
            }


        }


        static void Main(string[] args)
        {
            MyAttribute myAttr = new MyAttribute();

            //Console.WriteLine($"Name: {myAttr.Name}, Value: {myAttr.Value}");

            myAttr.DEBUGMethod();
            myAttr.NormalMethod();
            myAttr.MethodAbsolute();


        }

    }
}
