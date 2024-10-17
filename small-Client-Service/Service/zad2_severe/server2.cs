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
            byte[] data_len = new byte[4];
            stream.Read(data_len, 0, data_len.Length);
            int len = BitConverter.ToInt32(data_len,0);

            byte[] data = new byte[len];
            Console.WriteLine("Długość w byte wiadomości: " + len);
            int bytesRead = stream.Read(data, 0, data.Length);
            string message = Encoding.ASCII.GetString(data, 0, bytesRead);
            Console.WriteLine($"Odebrano: {message}");

            byte[] responseData = Encoding.ASCII.GetBytes($"odczytalem twoja wiadomość");
            stream.Write(responseData, 0, responseData.Length);
            Console.WriteLine("Wiadomość wysłana do klienta.");

            client.Close();

            server.Stop();
       
    }
}