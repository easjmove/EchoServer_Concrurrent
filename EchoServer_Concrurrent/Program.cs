using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace EchoServer_Concrurrent
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server:");
            TcpListener listener = new TcpListener(System.Net.IPAddress.Loopback, 7);
            listener.Start();//

            while (true)
            {
                TcpClient socket = listener.AcceptTcpClient();
                Task.Run(() => { 
                    HandleClient(socket);
                });
            }
        }

        public static void HandleClient(TcpClient socket)
        {
            NetworkStream ns = socket.GetStream();
            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);

            string message = reader.ReadLine();
            Console.WriteLine("Client wrote: " + message);
            writer.WriteLine(message);
            writer.Flush();
            socket.Close();
        }
    }
}
