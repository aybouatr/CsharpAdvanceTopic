using System.Security.Cryptography.X509Certificates;

namespace EventConsoleAppFromScratc
{

    public class SumArguments : EventArgs
    {
        public int A { get;}
        public int B { get; }

        public int SumAAndBC { get; }


       public SumArguments(int a, int b)
        {
            A = a;
            B = b;
            SumAAndBC = a + b;
        }
    }

    public class MakeEvent
    {
        public int A { get; }

        public int B { get; }

        public event EventHandler<SumArguments>? OnSumEvent;


        public void RasieEvent(int A, int B)
        {
            RasieEvent(new SumArguments(A, B));
        }

        protected virtual void RasieEvent(SumArguments args)
        {
            OnSumEvent?.Invoke(this, args);
        }

    }


    public class Displa
    {

        public void Subscribe(MakeEvent makeEvent)
        {
            makeEvent.OnSumEvent += DisplaySum;
        }

        public void DisplaySum(object? sender, SumArguments args)
        {
            Console.WriteLine($"The sum of {args.A} and {args.B} is {args.SumAAndBC}");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            MakeEvent makeEvent = new MakeEvent();
            Displa displa = new Displa();

            displa.Subscribe(makeEvent);

            makeEvent.RasieEvent(5, -10);
        }
    }
}
