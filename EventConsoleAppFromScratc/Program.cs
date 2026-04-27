using System.Security.Cryptography.X509Certificates;

namespace EventConsoleAppFromScratc
{

    //public class SumArguments : EventArgs
    //{
    //    public int A { get;}
    //    public int B { get; }

    //    public int SumAAndBC { get; }


    //   public SumArguments(int a, int b)
    //    {
    //        A = a;
    //        B = b;
    //        SumAAndBC = a + b;
    //    }
    //}

    //public class MakeEvent
    //{
    //    public int A { get; }

    //    public int B { get; }

    //    public event EventHandler<SumArguments>? OnSumEvent;


    //    public void RasieEvent(int A, int B)
    //    {
    //        RasieEvent(new SumArguments(A, B));
    //    }

    //    protected virtual void RasieEvent(SumArguments args)
    //    {
    //        OnSumEvent?.Invoke(this, args);
    //    }

    //}


    //public class Displa
    //{

    //    public void Subscribe(MakeEvent makeEvent)
    //    {
    //        makeEvent.OnSumEvent += DisplaySum;
    //    }

    //    public void DisplaySum(object? sender, SumArguments args)
    //    {
    //        Console.WriteLine($"The sum of {args.A} and {args.B} is {args.SumAAndBC}");
    //    }
    //}


    public class NewArticts
    {
        public string? Title { get; set; }
        public string? Content { get; set; }

       public NewArticts(string title, string content   )
        {
            Title = title;
            Content = content;
        }
    }


    class NewsPuplisher
    {
        public event EventHandler<NewArticts>? OnPersonCreated;
        public void CreateArticle(string title, string content)
        {
            NewArticts article = new NewArticts(title, content);

            RaiseEvent(article);
        }
        protected virtual void RaiseEvent(NewArticts article)
        {
            OnPersonCreated?.Invoke(this, article);
        }

    }

    class NewsSubscriber    
    {
        string ? Name { get; set; }


        public NewsSubscriber(string name)
        {
            Name = name;
        }
        public void Subscribe(NewsPuplisher eventMake)
        {
            eventMake.OnPersonCreated += Display;
        }

        public void Unsubscribe(NewsPuplisher eventMake)
        {
            eventMake.OnPersonCreated -= Display;
        }
        private void Display(object? sender, NewArticts person)
        {
           Console.WriteLine($"Name Subscriber: {Name}, Title: {person.Title}, Content: {person.Content}");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
         

            NewsPuplisher newsPublisher = new NewsPuplisher();
            NewsSubscriber newsSubscriber = new NewsSubscriber("Subscriber1");
            NewsSubscriber newsSubscriber1 = new NewsSubscriber("Subscriber2");
            NewsSubscriber newsSubscriber2 = new NewsSubscriber("Subscriber3");


            newsSubscriber.Subscribe(newsPublisher);
            newsSubscriber1.Subscribe(newsPublisher);
            newsSubscriber2.Subscribe(newsPublisher);



            newsPublisher.CreateArticle("Breaking News", "This is the content of the breaking news article.");
          
            Console.WriteLine("--------------------------------------------------------------------------------");
            newsSubscriber.Unsubscribe(newsPublisher);
            newsPublisher.CreateArticle("Another News", "This is the content of another news article.");


            Console.ReadLine();
            


        }
    }
}
