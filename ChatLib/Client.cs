using System;
using System.IO;
using System.Net.Sockets;

namespace ChatLib
{
    public class Client
    {

        protected TcpClient ChatClient { get; set; }
        protected NetworkStream Stream;
        
        /// <summary>
        /// Default constructor, empty body.
        /// </summary>
        protected Client() { }

        /// <summary>
        /// This constructor creates a TcpClient and attempts to connect to the given port and ip. It then grabs the client's
        /// NetworkStream, and begins the listening loop. Once the listening loop is complete, or an exception is thrown,
        /// the client is closed.
        /// <param name = "port">The port the client will be connecting to.</param>
        /// <param name="ip">The IP Address the server will be connecting to.</param>
        /// </summary>
        public Client(int port, string ip)
        {
            try
            {
                // Connecting to server
                ChatClient = new TcpClient(ip, port);
                
                // Get a stream object for reading and writing
                Stream = ChatClient.GetStream();

                Console.WriteLine("Client is connected to Server!");

                // Starting listening loop
                Listening();

            }
            catch (Exception)
            {
                /*
                 * A catch-all for unexpected exceptions
                 * It is possible that one side of the chat might be sending a message and the other side disconnects
                 * before it is sent, in which case this will catch that exception.
                 */
            }
            finally
            {
                //Explicitly closing the client is not required because ChatClient.Dispose() will be called automatically.
                Console.WriteLine("Application disconnected.");
            }
        }
        
        
        /// <summary>
        /// This method checks if there is any data available on the NetworkStream, and reads it if there is.
        /// Then converts the incoming data into a string and displays on the console.
        /// </summary>
        private void GetMessage()
        {

            // Checking if there is data to read and returning if there is none
            if (!Stream.DataAvailable) return;
            
            // Buffer for reading data
            var byteMsg = new byte[256];

            // Reading the incoming message and storing the length
            var msgLength = Stream.Read(byteMsg, 0, byteMsg.Length);
            
            // Translate data bytes to a ASCII string.
            var msg = System.Text.Encoding.ASCII.GetString(byteMsg, 0, msgLength);

            if (!string.IsNullOrWhiteSpace(msg))
            {
                Console.WriteLine(msg);
            }
            
        }
        
        
        /// <summary>
        /// Listening loop. Tests that the connection is still open, then checks for incoming messages, then checks for
        /// user input to either quit or enter a message to send.
        /// <exception cref="IOException">An attempt to send a message was made but the connection had been closed.</exception>
        /// </summary>
        protected void Listening()
        {
            while (true)
            {
                // Testing the connection, breaking loop if no longer connected
                if (!IsConnected()) {break;}

                // Checking for incoming message
                GetMessage();

                // If no console key is pressed restart the loop
                if (!Console.KeyAvailable) {continue;}

                //User input mode: when user press "I" key.            
                var userKey = Console.ReadKey(true);

                // If user entered insert mode
                if (userKey.Key == ConsoleKey.I)
                {
                    // Insertion mode prompt
                    Console.Write(">> ");

                    //Getting message
                    string msg = Console.ReadLine();
                    
                    // Checking if user typed quit
                    if (msg.ToLower().Equals("quit"))
                    {
                        Console.WriteLine("You typed \"quit\" to exit.");
                        break;
                    }
                    
                    SendMessage(msg);
                }
                else if (userKey.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("You typed Escape to exit.");
                    break;
                }
            }
        }

        /// <summary>
        /// Takes in a string and converts it to bytes, then sends the data over the NetworkStream
        /// <param name="msg">The message to be sent over the NetworkStream</param>
        /// </summary>
        private void SendMessage(string msg)
        {
            // Converting message to bytes
            var byteMsg = System.Text.Encoding.ASCII.GetBytes(msg);
            
            // Sending converted message over stream
            Stream.Write(byteMsg, 0, msg.Length);
        }
        

        /// <summary>
        /// Method that tests that the connection is still open by sending a message of just whitespace. If the sending
        /// throws an exception, the connection is closed and return false.
        /// </summary>
        /// <returns>True if the client is still connected, false if not.</returns>
        private bool IsConnected()
        {
            try
            {
                SendMessage(" ");
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

    }
}
