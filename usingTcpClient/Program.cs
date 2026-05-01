using System.Net;
using System.Net.Sockets;
using System.Text;

namespace usingTcpClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 8080);

            server.Start();

            Console.WriteLine("Server started on port 8080...");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();

                NetworkStream stream = client.GetStream();

                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);

                string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Request:\n" + request);

                // Send response (next step)
                string response = "HTTP/1.1 200 OK\r\nContent-Type: text/html\r\n\r\n<h1>Hello from C# socket!</h1>";
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);

                stream.Write(responseBytes, 0, responseBytes.Length);

                client.Close();
            }
        }
    }
}
