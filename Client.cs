using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Client
{
    static void Main()
    {
        TcpClient client = new TcpClient();
        client.Connect("127.0.0.1", 5000);
        Console.WriteLine("Подключился к серверу");

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
                    Console.WriteLine("Сервер: " + msg);
                }
            }
            catch { Console.WriteLine("Отключен от сервера"); }
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
    }
}