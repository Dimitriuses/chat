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
            byte IPx1 = Convert.ToByte(numericUpDown1.Value);
            byte IPx2 = Convert.ToByte(numericUpDown2.Value);
            byte IPx3 = Convert.ToByte(numericUpDown3.Value);
            byte IPx4 = Convert.ToByte(numericUpDown4.Value);
            int Port = Convert.ToInt32(numericUpDownPort.Value);
            var ServerIP = new IPAddress(new byte[] { IPx1, IPx2, IPx3, IPx4 });
            IP_Info = new IPEndPoint(ServerIP, Port);
        }
    }
}
