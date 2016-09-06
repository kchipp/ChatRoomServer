using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Collections;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace ChatroomServer
{
    public class Monitor
    {
        string userName;
        TcpClient clientSocket;
        Dictionary<string, TcpClient> clientsList;

        public void startClient(TcpClient ClientSocket, string userName, Dictionary<string, TcpClient> clientsList)
        {
            this.userName = userName;
            this.clientSocket = ClientSocket;
            this.clientsList = clientsList;
            Thread thread = new Thread(Communicate);
            thread.Start();
        }

        private void Communicate()
        {

            try
            {
                while (true)
                {
                    byte[] bytesFrom = new byte[1024];
                    string message = null;
                    NetworkStream networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, bytesFrom.Length);

                    message = Encoding.ASCII.GetString(bytesFrom);

                    message = message.Substring(0, message.IndexOf("\0"));



                    if (message.ToLower() == "esc")
                    {
                        message = ($"{userName} has exited the chat room");
                        Server.chatters.Remove(this.userName);
                    }
                    {
                        message = ($" {this.userName} >>> {message}");
                    }

                    Console.WriteLine(message);

                    Server.Broadcast(message);

                }

            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}