using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Server
{
    static void Main()
    {
        TcpListener server = new TcpListener(IPAddress.Any, 5000);
        server.Start();
        Console.WriteLine("Сервер запущен, жду клиента...");

        TcpClient client = server.AcceptTcpClient();
        Console.WriteLine("Клиент подключен");

        NetworkStream stream = client.GetStream();

        Thread receiveThread = new Thread(() =>
        {
            try
            {
                while (true)
                {
                    byte[] buf = new byte[1024];
                    int len = stream.Read(buf, 0, buf.Length);
                    if (len == 0) break;
                    string msg = Encoding.UTF8.GetString(buf, 0, len);
                    Console.WriteLine("Клиент: " + msg);
                }
            }
            catch { Console.WriteLine("Клиент отключился"); }
        });
        receiveThread.Start();

        while (true)
        {
            string text = Console.ReadLine();
            if (text == "exit") break;
            byte[] data = Encoding.UTF8.GetBytes(text);
            stream.Write(data, 0, data.Length);
        }

        client.Close();
        server.Stop();
    }
}