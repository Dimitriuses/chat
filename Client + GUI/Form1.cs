using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client___GUI
{
    public partial class Form1 : Form
    {
        public string Lodin { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void підключенняToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Connect dlg = new Connect();
            if(dlg.ShowDialog() = DialogResult.OK)
            {

            }
        }
    }
}
