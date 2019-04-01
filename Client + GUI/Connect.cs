using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client___GUI
{
    public partial class Connect : Form
    {
        public IPEndPoint IP_Info { get; set; }
        public string Login { get; set; }
        public Connect()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            Login = textBoxLogin.Text;
            byte[] IP = new byte[4];
            IP[0] = Convert.ToByte(numericUpDown1.Value);
            IP[1] = Convert.ToByte(numericUpDown2.Value);
            IP[2] = Convert.ToByte(numericUpDown3.Value);
            IP[3] = Convert.ToByte(numericUpDown4.Value);

            int Port = Convert.ToInt32(numericUpDownPort.Value);
            var ServerIP = new IPAddress(IP);
            IP_Info = new IPEndPoint(ServerIP, Port);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
