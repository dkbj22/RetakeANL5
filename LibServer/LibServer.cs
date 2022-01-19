using System;
using System.Text.Json;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using LibData;
using Microsoft.Extensions.Configuration;
using System.Text;



namespace LibServerSolution
{
    public struct Setting
    {
        public int ServerPortNumber { get; set; }
        public string ServerIPAddress { get; set; }
        public int BookHelperPortNumber { get; set; }
        public string BookHelperIPAddress { get; set; }
        public int ServerListeningQueue { get; set; }
    }




    abstract class AbsSequentialServer
    {
        protected Setting settings;



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



            Console.Out.WriteLine("[Server] {0} : {1}", type, msg);
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



                settings.ServerIPAddress = Config.GetSection("ServerIPAddress").Value;
                settings.ServerPortNumber = Int32.Parse(Config.GetSection("ServerPortNumber").Value);
                settings.BookHelperIPAddress = Config.GetSection("BookHelperIPAddress").Value;
                settings.BookHelperPortNumber = Int32.Parse(Config.GetSection("BookHelperPortNumber").Value);
                settings.ServerListeningQueue = Int32.Parse(Config.GetSection("ServerListeningQueue").Value);
                // Console.WriteLine( settings.ServerIPAddress, settings.ServerPortNumber );
            }
            catch (Exception e) { report("[Exception]", e.Message); }
        }




        protected abstract void createSocketAndConnectHelpers();

        public abstract void handelListening();

        protected abstract Message processMessage(Message message);

        protected abstract Message requestDataFromHelpers(string msg);

        protected abstract string[] receiveMsg(Socket sock);

    }



    class SequentialServer : AbsSequentialServer
    {
        // check all the required parameters for the server. How are they initialized?
        Socket serverSocket;
        IPEndPoint listeningPoint;
        Socket bookHelperSocket;
        byte[] buffer;


        public SequentialServer() : base()
        {
            GetConfigurationValue();
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            buffer = new byte[1000];
        }

        /// <summary>
        /// Connect socket settings and connec
        /// </summary>
        protected override void createSocketAndConnectHelpers()
        {
            // Extra Note: If failed to connect to helper. Server should retry 3 times.
            // After the 3d attempt the server starts anyway and listen to incoming messages to clients

            string stringToSent = "Server: Hello client";

            IPAddress ipAddress = IPAddress.Parse(settings.ServerIPAddress);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, settings.ServerPortNumber);
       
            try
            {
                serverSocket.Bind(localEndPoint);
                serverSocket.Listen(settings.ServerListeningQueue);
                Console.WriteLine("Waiting for connection...");

                //DIT MOET IN EEN ANDERE FUNCTIE V

                /*
                Socket handler = serverSocket.Accept();
                
                string recievedString = null;
                byte[] bytes = null;



                bytes = new byte[1024];
                int bytesRecieved = handler.Receive(bytes);
                recievedString += Encoding.ASCII.GetString(bytes, 0, bytesRecieved);
                Console.WriteLine(recievedString);



                byte[] msg = Encoding.ASCII.GetBytes(stringToSent);
                handler.Send(msg);
                */

            }



            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }



        }



        /// <summary>
        /// This method starts the socketserver after initializion and listents to incoming connections.
        /// It tries to connect to the book helpers. If it failes to connect to the helper. Server should retry 3 times.
        /// After the 3d attempt the server starts any way. It listen to clients and waits for incoming messages from clients
        /// </summary>
        public override void handelListening()
        {
            createSocketAndConnectHelpers();
            //todo: To meet the assignment requirement, finish the implementation of this method.



            while (true)
            {
                try
                {
                    Socket newSock = serverSocket.Accept();

                    string[] typeAndContent = receiveMsg(serverSocket);
                    Console.WriteLine(typeAndContent);

                    // WORDT GEREGELD IN receiveMsg() V

                    //int b = serverSocket.Receive(buffer);
                    //string data = Encoding.ASCII.GetString(buffer, 0, b);
                    //Console.WriteLine(data);

                    //strSendMsg(data);

                }
                catch (Exception e)
                {
                    Console.WriteLine("Something went wrong in the handelListening() from server");
                    Console.WriteLine(e.Message);
                    //break;

                }
            }

        }



        /// <summary>
        /// Process the message of the client. Depending on the logic and type and content values in a message it may call
        /// additional methods such as requestDataFromHelpers().
        /// </summary>
        /// <param name="message"></param>
        protected override Message processMessage(Message message)
        {
            Message pmReply = new Message();

            //todo: To meet the assignment requirement, finish the implementation of this method .





            return pmReply;
        }



        /// <summary>
        /// When data is processed by the server, it may decide to send a message to a book helper to request more data.
        /// </summary>
        /// <param name="content">Content may contain a different values depending on the message type. For example "a book title"</param>
        /// <returns>Message</returns>
        protected override Message requestDataFromHelpers(string content)
        {
            Message HelperReply = new Message();
            //todo: To meet the assignment requirement, finish the implementation of this method .



            // try
            // {




            // }
            // catch () { }



            return HelperReply;



        }



        public void delay()
        {
            int m = 10;
            for (int i = 0; i <= m; i++)
            {
                Console.Out.Write("{0} .. ", i);
                Thread.Sleep(200);
            }
            Console.WriteLine("\n");
            //report("round:","next to start");
        }

        //ZELF GEMAAKTE FUNCTIES ABSTRACT MAKEN (VERGEET OVERRIDE NIET V)

        public MessageType typeCheck(string inp)
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



        public string correctType(string badType)
        {
            string removeBegin = badType.Remove(0, 5);
            return removeBegin;
        }



        public string correctContent(string badContent)
        {
            string removedEnd = badContent.Remove(badContent.Length - 2);
            string removeBegin = removedEnd.Remove(0, 11);
            Console.WriteLine(removeBegin);
            return removeBegin;
        }



        public void strSendMsg(string data)
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