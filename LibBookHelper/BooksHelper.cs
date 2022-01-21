using System;
using System.Text.Json;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using LibData;
using Microsoft.Extensions.Configuration;

// added v
using System.Text;

namespace BookHelperSolution
{
    public struct Setting                                   
    {
        public int BookHelperPortNumber { get; set; }
        public string BookHelperIPAddress { get; set; }
        public int ServerListeningQueue { get; set; }
    }

    abstract class AbsSequentialServerHelper
    {
        protected Setting settings;
        protected string booksDataFile;

        /// <summary>
        /// Report method can be used to print message to console in standaard formaat. 
        /// It is not mandatory to use it, but highly recommended.
        /// </summary>
        /// <param name="type">For example: [Exception], [Error], [Info] etc</param>
        /// <param name="msg"> In case of [Exception] the message of the exection can be passed. Same is valud for other types</param>
        protected void report(string type, string msg)
        {
            // Console.Clear();
            Console.Out.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>");
            if (!String.IsNullOrEmpty(msg))
            {
                msg = msg.Replace(@"\u0022", " ");
            }

            Console.Out.WriteLine("[Server Helper] {0} : {1}", type, msg);
        }

        /// <summary>
        /// This methid loads required settings.
        /// </summary>
        protected void GetConfigurationValue()
        {
            settings = new Setting();
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                IConfiguration Config = new ConfigurationBuilder()
                    .SetBasePath(Path.GetFullPath(Path.Combine(path, @"../../../../")))
                    .AddJsonFile("appsettings.json")
                    .Build();

                settings.BookHelperIPAddress = Config.GetSection("BookHelperIPAddress").Value;
                settings.BookHelperPortNumber = Int32.Parse(Config.GetSection("BookHelperPortNumber").Value);
                settings.ServerListeningQueue = Int32.Parse(Config.GetSection("ServerListeningQueue").Value);
            }
            catch (Exception e) { report("[Exception]", e.Message); }
        }

        protected abstract void loadDataFromJson();
        protected abstract void createSocket();
        public abstract void handelListening();
        protected abstract Message processMessage(Message message);


        //Zelfgemaakte functies v

        protected abstract MessageType typeCheck(string inp);

        protected abstract string correctType(string badType);

        protected abstract string correctContent(string badContent);

        protected abstract void strSendMsg(string data);

        protected abstract string[] receiveMsg(Socket sock);

    }

    class SequentialServerHelper : AbsSequentialServerHelper
    {
        // check all the required parameters for the server. How are they initialized? 
        public Socket listener;
        public IPEndPoint listeningPoint;
        public IPAddress ipAddress;
        public List<BookData> booksList;
        byte[] buffer = new byte[1000];

        public SequentialServerHelper() : base()
        {
            booksDataFile = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../../") + "Books.json");
            GetConfigurationValue();
            loadDataFromJson();
        }

        /// <summary>
        /// This method loads data items provided in booksDataFile into booksList.
        /// </summary>
        protected override void loadDataFromJson()
        {
            //todo: To meet the assignment requirement, implement this method 
            try
            {
                BookData jsnBook;
                string configContent = File.ReadAllText(booksDataFile);
                jsnBook = JsonSerializer.Deserialize<BookData>(configContent);
                Console.WriteLine(jsnBook);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// This method establishes required socket: listener.
        /// </summary>
        protected override void createSocket()
        {
            //todo: To meet the assignment requirement, implement this method
<<<<<<< HEAD
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
=======
            ipAddress = IPAddress.Parse(settings.BookHelperIPAddress);
            listeningPoint = new IPEndPoint(ipAddress, settings.BookHelperPortNumber);

            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

>>>>>>> origin/CodeTester
            try
            {
                listener.Bind(listeningPoint);
                Console.WriteLine("Socket binding");
                listener.Listen(settings.ServerListeningQueue);
<<<<<<< HEAD
                Console.WriteLine("Listening... (bookhelpserver)");
=======
                Console.WriteLine("Listening...");
>>>>>>> origin/CodeTester

            }
            catch (Exception e)
            {
<<<<<<< HEAD
=======
                Console.WriteLine("Error in createSocket()");
>>>>>>> origin/CodeTester
                Console.WriteLine(e.Message);
            }

        }

        /// <summary>
        /// This method is optional. It delays the execution for a short period of time.
        /// Note: Can be used only for testing purposes.
        /// </summary>
        void delay()
        {
            int m = 10;
            for (int i = 0; i <= m; i++)
            {
                Console.Out.Write("{0} .. ", i);
                Thread.Sleep(200);
            }
            Console.WriteLine("\n");
        }

        /// <summary>
        /// This method handles all the communications with the LibServer.
        /// </summary>
        


        public override void handelListening()
        {
            createSocket();
<<<<<<< HEAD
            loadDataFromJson();
=======
            //loadDataFromJson(); doesn't work yet
>>>>>>> origin/CodeTester

            //todo: To meet the assignment requirement, finish the implementation of this method 
            while (true)
            {
<<<<<<< HEAD
                break;
=======

                try
                {
                    Socket newSock = listener.Accept();
                    Console.WriteLine("Accepting socket");

                    // &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
                    // &&---------------------------------------------------------------TO DO----------------------------------------------------------------------------------&&
                    // &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
                    // here we should receive a message with type bookinquiry and content booktitle v (the conection is there but we are sending wrong data / see libserver)
                    string[] typeAndContent = receiveMsg(newSock);
                    Console.WriteLine(typeAndContent);
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine("Accepting socket error");
                    Console.WriteLine(e.Message);
                    break;
                }
                
>>>>>>> origin/CodeTester
            }


        }

        /// <summary>
        /// Given the message received from the Server, this method processes the message and returns a reply.
        /// </summary>
        /// <param name="message">The message received from the LibServer.</param>
        /// <returns>The message that needs to be sent back as the reply.</returns>
        protected override Message processMessage(Message message)
        {
            Message reply = new Message();
            //todo: To meet the assignment requirement, finish the implementation of this method .
            // try
            // {

            // }
            // catch (Exception e)
            // {

            // }
            return reply;
        }



        //ZELFGEMAAKTE FUNCTIES -------------------------------------------------------------------------------

        protected override MessageType typeCheck(string inp)
        {
            MessageType ret;
            try
            {
                if (inp == "0")
                    ret = MessageType.Hello;
                if (inp == "1")
                    ret = MessageType.Welcome;
                if (inp == "2")
                    ret = MessageType.BookInquiry;
                if (inp == "3")
                    ret = MessageType.BookInquiryReply;
                if (inp == "4")
                    ret = MessageType.Error;
                if (inp == "5")
                    ret = MessageType.NotFound;
                else ret = MessageType.Error;
            }
            catch (Exception e) { Console.WriteLine(e.Message); ret = MessageType.Error; }

            return ret;
        }



        protected override string correctType(string badType)
        {
            string removeBegin = badType.Remove(0, 5);
            return removeBegin;
        }



        protected override string correctContent(string badContent)
        {
            string removedEnd = badContent.Remove(badContent.Length - 2);
            string removeBegin = removedEnd.Remove(0, 11);
            Console.WriteLine(removeBegin);
            return removeBegin;
        }



        protected override void strSendMsg(string data)
        {
            string[] typeAndContent = new string[2];
            typeAndContent = data.Split(",");



            Message msg = new Message();
            msg.Type = typeCheck(correctType(typeAndContent[0]));
            msg.Content = correctContent(typeAndContent[1]);



            processMessage(msg);
        }

        protected override string[] receiveMsg(Socket sock)
        {
            int receiveBytes = sock.Receive(buffer);
            string data = Encoding.ASCII.GetString(buffer, 0, receiveBytes);
            string[] typeAndContent = new string[2];
            typeAndContent = data.Split(",");
            Console.WriteLine(typeAndContent[0] + typeAndContent[1] + " - Received from server with receiveMsgClient");
            return typeAndContent;
        }
    }
}
