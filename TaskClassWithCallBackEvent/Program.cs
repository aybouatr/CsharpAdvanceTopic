using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading;


namespace TaskClassWithCallBackEvent
{

    class AllArg : EventArgs
    {

        public enum UrlType
        {
            CNN,
            Amazon,
            ProgrammingAdvice
        }
        public UrlType Type { get; set; }
        public string? Content  { get; set; }

        public string? Url  { get; set; }

        public AllArg(string? content, string? url,UrlType type) 
        {
            Content = content;
            Url = url ?? string.Empty;
            Type = type;
        }
    }

    internal class Program
    {
        public delegate void CallBackEventHandler(object sender, AllArg e);

        public static event CallBackEventHandler? CallBack;

        static async Task Main(string[] args)
        {

            CallBack += CallNotifecation;


            Task t =  DownloadDataFromWeb("http://www.cnn.com", CallBack);

            Task t1 =  DownloadDataFromWeb("http://www.Amazon.com", CallBack);

            Task t2 = DownloadDataFromWeb("http://www.programmingAdvice.com", CallBack);
            
            await Task.WhenAll(t, t1, t2);

             Console.WriteLine("All downloads completed.");

        }

        static public async Task DownloadDataFromWeb(string url, CallBackEventHandler? callback)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("User-Agent", "Mozilla/5.0");

                    await Task.Delay(2000); // simulate delay
                    
                    string content = await client.DownloadStringTaskAsync(url);

                    AllArg.UrlType type = url switch
                    {
                        "http://www.cnn.com" => AllArg.UrlType.CNN,
                        "http://www.Amazon.com" => AllArg.UrlType.Amazon,
                        "http://www.programmingAdvice.com" => AllArg.UrlType.ProgrammingAdvice,
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    callback?.Invoke(null, new AllArg(content, url, type));
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine($"Error for {url}: {ex.Message}");
            }

        }


        public static void CallNotifecation(object sender, AllArg e)
        {
            //if (e.Type == AllArg.UrlType.Amazon)
            Console.WriteLine($"Call back event called with parameters this URl: {e.Url} and length {e.Content.Length}");
        }
    }
}
