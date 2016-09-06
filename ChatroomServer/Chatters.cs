using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatroomServer
{
    class Participant : IChatters
    {
        string name;
        //int port;

        public Participant(string name)
        {
            this.name = name;
        }

        public void Update()
        {
            Console.WriteLine($"{name} has entered the room...");
        }



    }
}