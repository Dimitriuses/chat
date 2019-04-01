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

namespace Client___GUI
{
    public partial class Form1 : Form
    {
        public string Lodin { get; set; }
        public IPEndPoint IPServer;

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
            
        }

        private void UpdateConnectionStatus()
        {
            if(IPServer == null)
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
            if(dlg.ShowDialog() == DialogResult.OK)
            {
                Lodin = dlg.Login;
                IPServer = dlg.IP_Info;
                SendMSG("/Connected");

            }
            //UpdateConnectionStatus();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private bool SendMSG(string message)
        {
            Socket socket = new Socket(IPServer.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(IPServer);
                byte[] msg = Encoding.ASCII.GetBytes(message + "EOT");
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
                throw;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateConnectionStatus();
        }
    }
}
