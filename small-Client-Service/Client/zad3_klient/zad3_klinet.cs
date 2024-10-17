using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
class Client2
{
    static void Main()
    {
    
            TcpClient client = new TcpClient("Localhost", 8888);
            Console.WriteLine("Połączono z serwerem.");

            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream);


            while (true)
            {

                Console.Write("Wpisz wiadomość: ");
                string message = Console.ReadLine();
                writer.WriteLine(message);
                writer.Flush();

                if (message == "!end")
                {

                    Console.WriteLine("Zakończono połączenie");
                    break;
                }

                string response = reader.ReadLine();
                Console.WriteLine("Odpowiedź serwera: " + response);
            }

            client.Close();
        }
    }



