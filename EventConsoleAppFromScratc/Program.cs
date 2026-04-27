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


    // Employee Attendance and Departure
    // (using Events)
    // When an employee records their arrival or departure,
    // the registration device triggers an Event
    // that notifies three departments:
    // Payroll (records the time),
    // Security (alerts if the employee leaves before 2:00 PM),
    // and Human Resources (calculates lateness after 8:00 AM).

    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime ArrivalTime { get; set; }

        public DateTime DepartureTime { get; set; }


        public Employee(int id, string name, DateTime arrivalTime, DateTime departureTime)
        {
            Id = id;
            Name = name;
            ArrivalTime = arrivalTime;
            DepartureTime = departureTime;
        }
    }


    public class  MarkDay
    {
        public DateTime DateCome { get; set; }

        public DateTime DateLeave { get; set; }
        public MarkDay( DateTime dateCome, DateTime dateLeave)
            {
                DateCome = dateCome;
                DateLeave = dateLeave;
            }

    }

    public class EventMarkDay
    {
        
        public event EventHandler<Employee>? OnDayMarked;


        public void MarkDay(string Name,DateTime arrivalTime, DateTime leavetime)
        {
            Employee em = new Employee(0, Name, arrivalTime, leavetime);

            RaiseEvent(em);
        }

        protected virtual void RaiseEvent(Employee Date)
        {
                OnDayMarked?.Invoke(this, Date);
        }

    }


    public class PayrollDepartment
    {
        private static readonly TimeSpan StartTime = new(8, 0, 0);
        private static readonly TimeSpan EndTime = new(17, 0, 0); // 5:00 PM

        public void Subscribe(EventMarkDay eventMarkDay)
        {
            eventMarkDay.OnDayMarked += RecordTime;
        }

        private void RecordTime(object? sender, Employee employee)
        {
            if (employee.ArrivalTime.TimeOfDay < StartTime)
            {
                Console.WriteLine($"Employee arrived before 8:00 AM: {employee.ArrivalTime}");
            }

            if (employee.DepartureTime.TimeOfDay > EndTime)
            {
                Console.WriteLine($"Employee left after 5:00 PM: {employee.DepartureTime}");
            }
        }
    }



    internal class Program
    {
        static void Main(string[] args)
        {

            //{
            //    NewsPuplisher newsPublisher = new NewsPuplisher();
            //    NewsSubscriber newsSubscriber = new NewsSubscriber("Subscriber1");
            //    NewsSubscriber newsSubscriber1 = new NewsSubscriber("Subscriber2");
            //    NewsSubscriber newsSubscriber2 = new NewsSubscriber("Subscriber3");


            //    newsSubscriber.Subscribe(newsPublisher);
            //    newsSubscriber1.Subscribe(newsPublisher);
            //    newsSubscriber2.Subscribe(newsPublisher);



            //    newsPublisher.CreateArticle("Breaking News", "This is the content of the breaking news article.");

            //    Console.WriteLine("--------------------------------------------------------------------------------");
            //    newsSubscriber.Unsubscribe(newsPublisher);
            //    newsPublisher.CreateArticle("Another News", "This is the content of another news article.");

            //}



            EventMarkDay eventMarkDay = new EventMarkDay();

            PayrollDepartment payrollDepartment = new PayrollDepartment();

            payrollDepartment.Subscribe(eventMarkDay);

            eventMarkDay.MarkDay("John Doe", DateTime.Now, DateTime.Now.AddHours(8));
            eventMarkDay.MarkDay("Jane Smith", DateTime.Now.AddHours(-1), DateTime.Now.AddHours(7));
            eventMarkDay.MarkDay("Bob Johnson", DateTime.Now.AddHours(1), DateTime.Now.AddHours(9));



            Console.ReadLine();
        }
    }
}
