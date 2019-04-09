using Json.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace chat
{
    class Program
    {
        public static string data = null;
        public static List<Socket> Sockets;
        static void Main(string[] args)
        {
            Thread server = new Thread(new ThreadStart(Server));
            server.Start();
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ip = ipHostInfo.AddressList[1];
            byte[] buffer = new byte[1024];
            try
            {
                IPEndPoint RemotePort = new IPEndPoint(ip, 11000);
                Socket socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    //Console.WriteLine("Press any key for connect");
                    //Console.ReadKey();
                    //Console.WriteLine($"Conected to {socket.RemoteEndPoint}");

                    //Console.WriteLine("Enter MSG to transmit");
                    ChatMessage message = new ChatMessage();
                    message.Login = "[Server]";
                    message.Message = Console.ReadLine();
                    byte[] msg = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(message));
                    // send to server
                    socket.Connect(RemotePort);
                    int sendBytes = socket.Send(msg);
                    //receive ansver fron server
                    //int receivedBytes = socket.Receive(buffer);
                    //Console.WriteLine($"Received: {Encoding.ASCII.GetString(buffer, 0, receivedBytes)}");
                    //free socked
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

        }
        static void Server()
        {
            byte[] buffer = new byte[1024];
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ip = ipHostInfo.AddressList[1]; //moge ne 0

            foreach (var item in ipHostInfo.AddressList)
            {
                Console.WriteLine($"ip addr - {item}");
            }
            //Conection
            IPEndPoint localPort = new IPEndPoint(ip, 11000);
            Console.WriteLine("Port: " + localPort.Port);
            //Socet creation
            Socket socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Bind(localPort);
                socket.Listen(100);
                //Console.ReadKey();

                //loop for connection acceptance
                while (true)
                {
                    Console.WriteLine("Waitting for connection/command...");
                    Socket handler = socket.Accept();
                    data = null;
                    Sockets.Add(handler);
                    //string ConOut = "";
                    //loop for data receiving
                    ThreadStart start = delegate { UserConnectrd(handler); };
                    Thread User = new Thread(start);
                    User.Start();

                    //while (true)
                    //{
                    //    buffer = new byte[1024];
                    //    int receivedBytes = handler.Receive(buffer);
                    //    data += Encoding.ASCII.GetString(buffer, 0, receivedBytes);
                    //    ChatMessage message = new ChatMessage();
                    //    message = JsonConvert.DeserializeObject<ChatMessage>(data);
                    //    if (message.Message.IndexOf("/Connected") > -1)
                    //    {
                    //        Console.WriteLine( handler.RemoteEndPoint + " " + message.Login + ": Conected");
                    //        //handler.Send(buffer);
                    //        break;
                    //    }
                    //    if (message.Message.IndexOf("EOT") > -1)
                    //    {

                    //        break;
                    //    }
                    //}
                    ////Console.WriteLine($"Received: {data.Length} bytes" +
                    ////    $" \nUseless data is :{data}");
                    ////echo effect
                    ////byte[] msg = Encoding.ASCII.GetBytes(data);
                    ////handler.Send(msg);
                    //handler.Shutdown(SocketShutdown.Both);
                    //handler.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        public static void UserConnectrd(Socket handler)
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                buffer = new byte[1024];
                int receivedBytes = handler.Receive(buffer);
                data += Encoding.ASCII.GetString(buffer, 0, receivedBytes);
                ChatMessage message = new ChatMessage();
                message = JsonConvert.DeserializeObject<ChatMessage>(data);
                if (message.Message.IndexOf("/Connected") > -1)
                {
                    Console.WriteLine(handler.RemoteEndPoint + " " + message.Login + ": Conected");
                    //handler.Send(buffer);
                    break;
                }
                if (message.Message.IndexOf("EOT") > -1)
                {
                    SEND_MESSAGE_ALL_USERS(data);
                    break;
                }
            }
            //Console.WriteLine($"Received: {data.Length} bytes" +
            //    $" \nUseless data is :{data}");
            //echo effect
            //byte[] msg = Encoding.ASCII.GetBytes(data);
            //handler.Send(msg);
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }

        private static void SEND_MESSAGE_ALL_USERS(string json)
        {
            foreach (Socket socket in Sockets)
            {
                IPEndPoint remoteIpEndPoint = socket.RemoteEndPoint as IPEndPoint;
                IPEndPoint RemotePort = new IPEndPoint(remoteIpEndPoint.Address, 12000);
                try
                {
                    //Console.WriteLine("Press any key for connect");
                    //Console.ReadKey();
                    //Console.WriteLine($"Conected to {socket.RemoteEndPoint}");

                    //Console.WriteLine("Enter MSG to transmit");
                    
                    byte[] msg = Encoding.ASCII.GetBytes(json);
                    // send to server
                    socket.Connect(RemotePort);
                    int sendBytes = socket.Send(msg);
                    //receive ansver fron server
                    //int receivedBytes = socket.Receive(buffer);
                    //Console.WriteLine($"Received: {Encoding.ASCII.GetString(buffer, 0, receivedBytes)}");
                    //free socked
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }
    }
}
