using System.Reflection;

namespace Reflection
{

    public class MyClass
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public void MyMethod()
        {
            Console.WriteLine("This is a method in MyClass.");
        }

        public MyClass(string name, int id)

        {
            Name = name;
            Id = id;
        }
    }


    public class BankAccount
    {

        public event EventHandler? OnBalanceChanged;
        public string? AccountHolder { get; set; }

        private decimal Balance { get; set; } = 0;

        public decimal Id { get; set; }

        public void Oposite(decimal amount)
        {
            this.Balance = amount;
        }

        public decimal Withdraw(decimal amount)
        {
            if (amount > Balance)
            {
                OnBalanceChanged?.Invoke(this, null);
            }
            Balance -= amount;
            return Balance;
        }

    }

    internal class Program
    {
        static void Main(string[] args)
        {

            //Type KnowType = typeof(MyClass);

            //Console.WriteLine(KnowType);
            //Console.WriteLine("this is Full Name : {0}", KnowType.FullName);
            //Console.WriteLine("this is Name : {0}", KnowType.Name);
            //Console.WriteLine("this is Namespace : {0}", KnowType.Namespace);

            //Type type = DateTime.Now.GetType(); // this at runtime
            //Type type2  = type.GetType(); // this is at compile time

            // this example of using reflection to create an instance of BankAccount and invoke its method
            
            {

                Console.WriteLine("====================================== This Memberinfor =============================================\n\n");

                BankAccount account = new BankAccount();

                Type BankAccountType = account.GetType();

                

                MemberInfo[] members = BankAccountType.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);      

                foreach (MemberInfo member in members)
                {
                    Console.WriteLine("Member Name: {0}, Member Type: {1}", member.Name, member.MemberType);
                }

                Console.WriteLine("====================================== This FieldInfo =============================================\n\n");

                FieldInfo[] fields = BankAccountType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

                foreach (FieldInfo field in fields)
                {
                    Console.WriteLine("Field Name: {0}, Field Type: {1}", field.Name, field.FieldType);
                 }

                Console.WriteLine("====================================== This EventInfo =============================================\n\n");

                EventInfo[] events = BankAccountType.GetEvents(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

                foreach (EventInfo eventInfo in events)
                {
                    Console.WriteLine(eventInfo);
                }


                // this example of using reflection to create an instance of BankAccount and invoke its method

                Console.WriteLine("====================================== This MethodInfo =============================================\n\n");


                //Type[] paramaters  = new Type[] { typeof(decimal) };

                //MethodInfo method = BankAccountType.GetMethod("Withdraw");
                //method.Invoke(account, new object[] { 100m });

                //Console.WriteLine(account.Withdraw(50m));
                //Console.WriteLine("Method Name: {0}, Return Type: {1}", method.Name, method.ReturnType);

                Object myBankAccount = Activator.CreateInstance(BankAccountType);

                BankAccountType.GetMethod("Oposite").Invoke(myBankAccount, new object[] { 1000m });

                    Console.WriteLine("Balance after Oposite: {0}", BankAccountType.GetMethod("Withdraw").Invoke(myBankAccount, new object[] { 100m }));



            }
            //Assembly 
        }
    }
}
