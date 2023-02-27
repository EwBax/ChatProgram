using ChatLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleChat
{
    class Program
    {
        static void Main(string[] args)
        {
            
            //Console.WriteLine(args[0]);
            if (args.Contains("-server"))  // if args[0] = "-server"
            {
                Console.WriteLine("Server");
                Server server = new Server();   //create a new object
            }
            else
            {
                Console.WriteLine("Client");
                Client client = new Client();   //create a new object
            }

        }
        
    }
}
