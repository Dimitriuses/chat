using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {

        static void Main(string[] args)
        {
            byte[] buffer = new byte[1024];
            try
            {
                //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                //IPAddress ip = ipHostInfo.AddressList[1]; //moge ne 0

                //foreach (var item in ipHostInfo.AddressList)
                //{
                //    Console.WriteLine($"ip addr - {item}");
                //}
                //Conection
                byte[] conect = new byte[4];
                conect[0] = Convert.ToByte(Convert.ToInt32(Console.ReadLine()));
                conect[1] = Convert.ToByte(Convert.ToInt32(Console.ReadLine()));
                conect[2] = Convert.ToByte(Convert.ToInt32(Console.ReadLine()));
                conect[3] = Convert.ToByte(Convert.ToInt32(Console.ReadLine()));
                var serverIP = new IPAddress(conect);
                //IPEndPoint RemotePort = new IPEndPoint(ip, 11000);
                IPEndPoint RemotePort = new IPEndPoint(serverIP, 11000);
                //Socet creation
                Socket socket = new Socket(serverIP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    Console.WriteLine("Press any key for connect");
                    Console.ReadKey();
                    socket.Connect(RemotePort);
                    Console.WriteLine($"Conected to {socket.RemoteEndPoint}");

                    Console.WriteLine("Enter MSG to transmit");

                    byte[] msg = Encoding.ASCII.GetBytes(Console.ReadLine() + "EOT");
                    // send to server
                    int sendBytes = socket.Send(msg);
                    //receive ansver fron server
                    int receivedBytes = socket.Receive(buffer);
                    Console.WriteLine($"Received: {Encoding.ASCII.GetString(buffer,0,receivedBytes)}");
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
    }
}
