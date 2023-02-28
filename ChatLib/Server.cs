using System;
using System.Net;
using System.Net.Sockets;

namespace ChatLib
{
    public class Server
    {
        private TcpListener _server;
        private TcpClient _client;
        private NetworkStream _stream;

        // Constructor
        public Server()
        {
            try
            {
                // Set the TcpListener on port 13000.
                const int port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                _server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                _server.Start();

                Console.WriteLine("Waiting for any Client connection...");

                // Perform a blocking call to accept requests.
                // You could also use server.AcceptSocket() here.
                _client = _server.AcceptTcpClient();
                Console.WriteLine("Connection established with Client.");

                // Starting listening loop
                Listening();

            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                _server.Stop();
            }
        }
        
        
        // Listening method
        private void Listening()
        {

            while (true)
            {

                // Checking if the client has disconnected
                if (!_client.Connected)
                {
                    Console.WriteLine("Application has been disconnected.");
                    break;
                }

                // Checking for incoming message
                GetMessage();
                
                // If no console key is pressed restart the loop
                if (!Console.KeyAvailable) {continue;}
                
                //User input mode: when user press "I" key.            
                var userKey = Console.ReadKey(true);
                
                if (userKey.Key == ConsoleKey.I)
                {
                    // Insertion mode prompt
                    Console.Write(">> ");
                    
                    //Getting message
                    string msg = Console.ReadLine();
                    
                    // Checking if user typed quit
                    if (msg.Equals("quit"))
                    {
                        break;
                    }
                    else
                    {
                        SendMessage(msg);
                    }
                }
                else if (userKey.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("You typed Escape to exit.");
                    break;
                }
            }
        }

        
        // Message to check for and receive a message from client
        private void GetMessage()
        {
            // Buffer for reading data
            var byteMsg = new byte[256];

            // Get a stream object for reading and writing
            _stream = _client.GetStream();
            
            // Checking if there is data to read and returning if there is none
            if (!_stream.DataAvailable) return;

            // Reading the incoming message and storing the length
            var i = _stream.Read(byteMsg, 0, byteMsg.Length);
            
            // Translate data bytes to a ASCII string.
            var msg = System.Text.Encoding.ASCII.GetString(byteMsg, 0, i);
            Console.WriteLine(msg);
            
        }
        
        
        // Method to send message to client
        private void SendMessage(string msg)
        {
            // Converting message to bytes
            var byteMsg = System.Text.Encoding.ASCII.GetBytes(msg);
            
            // Sending converted message over stream
            _stream.Write(byteMsg, 0, msg.Length);
        }
        
        
    }
}
