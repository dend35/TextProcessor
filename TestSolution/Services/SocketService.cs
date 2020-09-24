using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TestSolution.Helper.Interface;

namespace TestSolution.Services
{
    public class SocketService
    {
        public SocketService(IWordHelper wordHelper)
        {
            var listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listenSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8005));
                listenSocket.Listen(10);

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    var handler = listenSocket.Accept();
                    var builder = new StringBuilder();
                    var data = new byte[256];
                    do
                    {
                        var bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    } while (handler.Available > 0);

                    var message = string.Join(Environment.NewLine,
                        wordHelper.GetByPrefix(builder.ToString()).Item1.Select(i => i.Text));
                    data = Encoding.Unicode.GetBytes(message);
                    handler.Send(data);

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}