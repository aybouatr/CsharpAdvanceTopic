namespace Nullable
{
    internal class Program
    {
        static void Main(string[] args)
        {
            

             Nullable<int> res = 0;

            Console.WriteLine(res);

            string? name = null;

            Console.WriteLine(name);

            string? Value = null;

            Console.WriteLine("this try write value it's has null = {0} no null = {1}\n",res.ToString(), Value?.ToString());

            string? value = "hh";

            Console.WriteLine("this value the var Value = {0}",value ?? "is null");


        }
    }
}
