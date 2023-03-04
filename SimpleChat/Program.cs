using ChatLib;
using System.Linq;

namespace SimpleChat
{
    class Program
    {
        /// <summary>
        /// Starts the SimpleChat Program, creating either Server or Client object based on command line arguments.
        /// </summary>
        /// <param name="args">Command line arguments entered at runtime. "-server" arg runs program as server.
        /// If "-server" not given as arg, program runs in client mode.</param>
        static void Main(string[] args)
        {
            
            if (args.Contains("-server"))  // if args[0] = "-server"
            {
                Server server = new Server(1300, "127.0.0.1");   //create a new Server object
            }
            else
            {
                Client client = new Client(1300, "127.0.0.1");   //create a new Client object
            }

        }
        
    }
}
