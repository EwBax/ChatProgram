using System;
using System.Dynamic;
using System.Net.Sockets;

namespace ChatLib
{
    public class Client
    {

        private TcpClient _client;
        private NetworkStream _stream;

        public Client()
        {
            try
            {
                // Connecting to server
                const int port = 13000;
                _client = new TcpClient("127.0.0.1", port);

                // starting listening loop
                Listening();

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                _client.Dispose();
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
        
        
        // Listening method
        private void Listening()
        {
            while (true)
            {
                // Checking for incoming message
                GetMessage();
                
                // If no console key is pressed restart the loop
                if (!Console.KeyAvailable) {continue;}
                
                //User input mode: when user press "I" key.            
                var userKey = Console.ReadKey();
                
                if (userKey.Key == ConsoleKey.I)
                {
                    Console.Write(">> ");
                    // Make this sent to the respective sendMessage method
                    SendMessage(Console.ReadLine());
                }
                else if (userKey.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("You typed Escape to exit.");
                    break;
                }
            }
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
