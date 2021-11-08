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
            //Tells the operating system that all TCP connections on port 7 should be sent to this application
            TcpListener listener = new TcpListener(System.Net.IPAddress.Loopback, 7);
            //Starts listening
            listener.Start();

            //Infinite loop to be able to handle more than one client
            while (true)
            {
                //Blocks the thread until a client connects
                TcpClient socket = listener.AcceptTcpClient();
                //Starts a new thread with the incoming client, so that the application can handle several clients at the same time
                Task.Run(() => { 
                    HandleClient(socket);
                });
            }
        }

        public static void HandleClient(TcpClient socket)
        {
            //Gets the stream (bi-directional) that connects the server and the client
            NetworkStream ns = socket.GetStream();
            //Creates a reader for easy access to what the client sends
            StreamReader reader = new StreamReader(ns);
            //Creates a writer for easily writing to the client
            StreamWriter writer = new StreamWriter(ns);

            //Reads all data until the client sends a newline (\r\n) and stores it in a string
            string message = reader.ReadLine();
            Console.WriteLine("Client wrote: " + message);
            //writes the same data back to the client and ends with newline (\r\n)
            writer.WriteLine(message);
            //makes sure that the server sends the data immediately (it should wait for potentially more data)
            writer.Flush();
            //closes the connection, single use server.
            socket.Close();
        }
    }
}
