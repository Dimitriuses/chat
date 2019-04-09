using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Threading;

namespace Client___GUI
{
    public partial class Form1 : Form
    {
        public string Lodin { get; set; }
        public IPEndPoint IPServer;
        Thread server;

        public string[] Status = { "None Conection", "Disconected", "Connected" };

        enum ConnectStatus
        {
            None_Conection,
            Disconnected,
            Connected,
        }

        public Form1()
        {
            InitializeComponent();
            server = new Thread(Server);
        }

        private void UpdateConnectionStatus()
        {
            if (IPServer == null)
            {
                statusStrip1.Items[0].ForeColor = Color.Gray;
                statusStrip1.Items[0].Text = Status[0];
            }
            else
            {
                Socket socket = new Socket(IPServer.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    //socket.Connect(IPServer);
                    //socket.Shutdown(SocketShutdown.Both);
                    //socket.Close();
                    statusStrip1.Items[0].ForeColor = Color.Green;
                    statusStrip1.Items[0].Text = Status[2];


                }
                catch (Exception e)
                {
                    statusStrip1.Items[0].ForeColor = Color.Red;
                    statusStrip1.Items[0].Text = Status[1];
                    //throw;
                }

            }
        }

        private void підключенняToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Connect dlg = new Connect();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Lodin = dlg.Login;
                IPServer = dlg.IP_Info;
                ChatMessage message = new ChatMessage();
                message.Login = Lodin;
                message.Message = "/Connected";
                string json = JsonConvert.SerializeObject(message);
                SendMSG(json);
                
            }
            //UpdateConnectionStatus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                ChatMessage message = new ChatMessage();
                message.Login = Lodin;
                message.Message = textBox1.Text + "EOT";
                string json = JsonConvert.SerializeObject(message);
                if (SendMSG(json))
                {

                }
                //DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(string[]));
            }
        }

        private bool SendMSG(string message)
        {
            Socket socket = new Socket(IPServer.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(IPServer);
                byte[] msg = Encoding.ASCII.GetBytes(message); //+ "EOT");
                int sendBytes = socket.Send(msg);
                //byte[] buffer = new byte[1024];
                //int receivedBytes = socket.Receive(buffer);
                //richTextBox1.Text+= Encoding.ASCII.GetString(buffer, 0, receivedBytes);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
                //throw;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateConnectionStatus();
        }

        public void Server()
        {
            string data = null;
            byte[] buffer = new byte[1024];
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ip = ipHostInfo.AddressList[1]; //moge ne 0

            //foreach (var item in ipHostInfo.AddressList)
            //{
            //    Console.WriteLine($"ip addr - {item}");
            //}
            //Conection
            IPEndPoint localPort = new IPEndPoint(ip, 12000);
            //Console.WriteLine("Port: " + localPort.Port);
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
                    //Console.WriteLine("Waitting for connection/command...");
                    Socket handler = socket.Accept();
                    data = null;
                    string ConOut = "";
                    //loop for data receiving
                    while (true)
                    {
                        buffer = new byte[1024];
                        int receivedBytes = handler.Receive(buffer);
                        data += Encoding.ASCII.GetString(buffer, 0, receivedBytes);
                        ChatMessage message = new ChatMessage();
                        message = JsonConvert.DeserializeObject<ChatMessage>(data);
                        //if (message.Message.IndexOf("/Connected") > -1)
                        //{
                        //    //Console.WriteLine(handler.RemoteEndPoint + " " + message.Login + ": Conected");
                        //    //handler.Send(buffer);
                        //    break;
                        //}
                        if (message.Message.IndexOf("EOT") > -1)
                        {

                            NewMess(message.Message.Replace("EOT",string.Empty));
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        private void NewMess(string message)
        {
            richTextBox1.Lines[richTextBox1.Lines.Length] = message;
        }
    }
}
