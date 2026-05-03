namespace MutableAndImmutableTypes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // define mutable type
            List<string> mutableList = new List<string> { "A", "B", "C" };

            for (int i = 0;i < mutableList.Count; i++)
            {
                mutableList[i] += " modified";
            }

            Console.WriteLine("Mutable List before modification:");
            foreach (var item in mutableList)
            {
                Console.WriteLine(item);
            }

            // define immutable type

            var immutableList = new[] { "X", "Y", "Z" };

            Console.WriteLine("\nImmutable List before modification:");
            foreach (var item in immutableList)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("nModifying Mutable List...");

            for (int i = 0; i < mutableList.Count; i++)
            {
                mutableList[i] += " modified";
                Console.WriteLine(mutableList[i]);
            }
        }
    }
}
