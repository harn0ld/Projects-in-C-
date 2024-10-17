using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    static void Main()
    {


            
            TcpListener server = new TcpListener(IPAddress.Any, 8888);

            server.Start();

            Console.WriteLine("Oczekiwanie na połączenie...");

            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("Połączono!");

            NetworkStream stream = client.GetStream();
            byte[] data = new byte[1024];
            int bytesRead = stream.Read(data, 0, data.Length);
            string message = Encoding.ASCII.GetString(data, 0, bytesRead);
            Console.WriteLine($"Odebrano: {message}");

            message = message.Substring(0, Math.Min(message.Length, 1024));

            byte[] responseData = Encoding.ASCII.GetBytes($"odczytalem: {message}");
            stream.Write(responseData, 0, responseData.Length);
            Console.WriteLine("Wiadomość wysłana do klienta.");

            client.Close();

            server.Stop();
       
    }
}

