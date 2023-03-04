using System;
using System.Net;
using System.Net.Sockets;

namespace ChatLib
{
    
    /// <summary>
    /// A class that extends Client to have all of the chat client functionality, but also serves as a server for a
    /// client to connect to.
    /// </summary>
    public class Server : Client
    {
        private TcpListener _server;

        
        /// <summary>
        /// This constructor creates the TcpListener on the given port and IP, then listens for clients connecting.
        /// When a client is connected it gets the client's NetworkStream object, then goes into a listening loop.
        /// Once the listening loop is complete, or an exception is thrown, the server is stopped.
        /// <param name = "port">The port the server will be listening on.</param>
        /// <param name="ip">The IP Address the server will be hosted on.</param>
        /// </summary>
        public Server(int port, string ip)
        {
            try
            {
                // Parsing the IP parameter
                IPAddress localAddr = IPAddress.Parse(ip);

                // assigning the server to a port and ip
                _server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                _server.Start();

                Console.WriteLine("Waiting for any Client connection...");

                // Perform a blocking call to accept requests.
                ChatClient = _server.AcceptTcpClient();

                // Get a stream object for reading and writing
                Stream = ChatClient.GetStream();

                Console.WriteLine("Connection established with Client.");

                // Starting listening loop
                Listening();

            }
            catch (Exception e)
            {
                // A general catch-all for any unexpected exceptions
                Console.WriteLine(e.Message);
            }
            finally
            {
                //Explicitly closing the client is not required because ChatClient.Dispose() will be called automatically.
                _server.Stop();
                Console.WriteLine("Application disconnected.");
            }
        }
    }
    
}
