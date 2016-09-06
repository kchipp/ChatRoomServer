using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ChatroomServer
{
    public class Server
    {
        public static Dictionary<string, TcpClient> chatters = new Dictionary<string, TcpClient>();
        public static Queue<string> messageQueue = new Queue<string>();

        public Server()//constructor
        {

        }

        public void startServer()
        {
            try
            {
                TcpListener server = new TcpListener(IPAddress.Any, 5253);
                TcpClient clients = default(TcpClient);

                server.Start();
                Console.WriteLine("Waiting for connection...");

                while (true)
                {
                    clients = server.AcceptTcpClient();
                    Console.WriteLine("Connected.  Awaiting messages");

                    byte[] bytesFrom = new byte[1024];
                    byte[] bytesSent = new byte[1024];
                    string client = null;
                    NetworkStream networkStream = clients.GetStream();

                    networkStream.Read(bytesFrom, 0, bytesFrom.Length);
                    client = Encoding.ASCII.GetString(bytesFrom);
                    client = client.Substring(0, client.IndexOf("\0"));

                    Console.WriteLine($"{client} has joined\n");
                    bytesSent = Encoding.ASCII.GetBytes($"Hello, {client}");
                    networkStream.Write(bytesSent, 0, bytesSent.Length);
                    Broadcast($"{client} has joined");

                    chatters.Add(client, clients);

                    Monitor watchClient = new Monitor();
                    watchClient.startClient(clients, client, chatters);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Console.WriteLine("Goodbye");
                Console.ReadLine();
            }

        }

        public static void Broadcast(string message)
        {
            try
            {
                while (messageQueue.Count > 0)
                {
                    byte[] bytesOut = System.Text.Encoding.ASCII.GetBytes(messageQueue.Dequeue());
                    foreach (KeyValuePair<string, TcpClient> user in chatters)
                    {
                        //byte[] bytes = null;
                        bytesOut = System.Text.Encoding.ASCII.GetBytes(message);
                        NetworkStream broadcast = user.Value.GetStream();
                        broadcast.Write(bytesOut, 0, bytesOut.Length);
                        broadcast.Flush();

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }


    }//class
}//namespace