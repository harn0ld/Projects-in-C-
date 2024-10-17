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
           string myDir = Directory.GetCurrentDirectory();
            
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream);
            string myDir = Directory.GetCurrentDirectory();

            while (true)
            {
                string message = reader.ReadLine();

                if (string.IsNullOrEmpty(message))
                    break;

                if (message == "!end")
                {
                    Console.WriteLine("Zakończono połączenie");
                    break;
                }
                else if (message == "list")
                {
                    string[] files = Directory.GetFiles(myDir);
                    string[] directories = Directory.GetDirectories(myDir);
                    StringBuilder fileList = new StringBuilder();
                    foreach (string file in files)
                    {
                        fileList.AppendLine(Path.GetFileName(file));
                    }
                    foreach (string directory in directories)
                    {
                        fileList.AppendLine(Path.GetFileName(directory) + "/");
                    }
                    writer.WriteLine(fileList);
                    writer.Flush();
                }
                else if (message.StartsWith("in "))
                {
                    string subdir = message.Substring(3);
                    string newDir = Path.Combine(myDir, subdir);

                    if (Directory.Exists(newDir))
                    {
                        myDir = newDir;
                        string[] files = Directory.GetFiles(myDir);
                        string[] directories = Directory.GetDirectories(myDir);
                        StringBuilder fileList = new StringBuilder();
                        foreach (string file in files)
                        {
                            fileList.AppendLine(Path.GetFileName(file));
                        }
                        foreach (string directory in directories)
                        {
                            fileList.AppendLine(Path.GetFileName(directory) + "/");
                        }
                        writer.WriteLine(fileList);
                        writer.Flush();
                    }
                    else
                    {
                        writer.WriteLine("katalog nie istnieje");
                        writer.Flush();
                    }
                }
                else if (message == "cd .."){
                    if(string.IsNullOrEmpty(Directory.GetParent(myDir)).FullName){

                        writer.WriteLine("Katalog nie ma nadkatalogu");
                        writer.Flush();
                    }
                    else{
                        string x = Directory.GetParent(myDir).FullName;
                        myDir = x;
                    }
                }
                else
                {
                    writer.WriteLine("nieznane polecenie");
                    writer.Flush();
                }
            }

            server.Stop();
       
    }
}