using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace chat
{
    class Program
    {
        public static string data = null;
        static void Main(string[] args)
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
                    Console.WriteLine("Waitting for connection...");
                    Socket handler = socket.Accept();
                    data = null;

                    //loop for data receiving
                    while (true)
                    {
                        buffer = new byte[1024];
                        int receivedBytes = handler.Receive(buffer);
                        data += Encoding.ASCII.GetString(buffer, 0, receivedBytes);
                        
                        if (data.IndexOf("/Connected") > -1)
                        {
                            data += " " + handler.RemoteEndPoint;
                            //handler.Send(buffer);
                            break;
                        }
                        if (data.IndexOf("EOT") > -1)
                        {
                            break;
                        }
                    }
                    Console.WriteLine($"Received: {data.Length} bytes" +
                        $" \nUseless data is :{data}");
                    //echo effect
                    byte[] msg = Encoding.ASCII.GetBytes(data);
                    //handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
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
    }
}
