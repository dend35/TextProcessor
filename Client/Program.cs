using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    class Program
    {
        private const int Port = 8005;
        private const string Address = "127.0.0.1";

        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    var ipPoint = new IPEndPoint(IPAddress.Parse(Address), Port);

                    var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(ipPoint);
                    Console.Write("Введите префикс:");
                    var message = Console.ReadLine();
                    var data = Encoding.Unicode.GetBytes(message ?? string.Empty);
                    socket.Send(data);

                    data = new byte[256];
                    var builder = new StringBuilder();

                    do
                    {
                        var bytes = socket.Receive(data, data.Length, 0);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    } while (socket.Available > 0);

                    Console.WriteLine(builder.ToString());

                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}