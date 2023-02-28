using ChatLib;
using System;
using System.Linq;

namespace SimpleChat
{
    class Program
    {
        static void Main(string[] args)
        {
            
            //Console.WriteLine(args[0]);
            if (args.Contains("-server"))  // if args[0] = "-server"
            {
                Server server = new Server();   //create a new object
            }
            else
            {
                Client client = new Client();   //create a new object
            }

        }
        
    }
}
