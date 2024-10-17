using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
class Client
{
    static void Main()
    {
    
            TcpClient client = new TcpClient("Localhost", 8888);
            Console.WriteLine("Połączono z serwerem.");


            Console.Write("Wpisz wiadomość: ");
            string message = Console.ReadLine();
            message = message.Substring(0, Math.Min(message.Length, 1024));
            byte[] data = Encoding.ASCII.GetBytes(message);
            NetworkStream stream = client.GetStream();
            stream.Write(data, 0, data.Length);
            Console.WriteLine("Wiadomość wysłana do serwera.");

            data = new byte[1024];
            int bytesRead = stream.Read(data, 0, data.Length);
            string responseData = Encoding.ASCII.GetString(data, 0, bytesRead);
            Console.WriteLine($"Odebrano od serwera: {responseData}");
            client.Close();
    }

}
