namespace DelagationsLogger
{

    public class Logger
    {
        public delegate void LogAction(string message);

        LogAction _logAction;

        public Logger(LogAction logAction)
        {
            this._logAction = logAction;
        }

        public void Log(string name)
        {
            _logAction(name);
        }
    }

    public class Program
    {
        public static void LogConsole(string message)
        {
            Console.WriteLine(message);
        }

        public static void LogDebug(string message)
        {
            Console.WriteLine(message);
        }


        static void Main(string[] args)
        {
            Logger logger = new Logger(LogConsole);
            Logger ol = new Logger(LogDebug);


            logger.Log("Hello World");
            ol.Log("Hello Debug");
        }
    }
    
}
