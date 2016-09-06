using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;


namespace ChatroomServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowHeight = 35;
            Console.WindowWidth = 50;
            Console.SetWindowPosition(0, 0);
            Server server = new Server();
            server.startServer();

        }
    }
}
