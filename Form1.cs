using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vcs_seria_communicationForm
{
    public partial class Form1 : Form
    {

        private TcpIpServer server;
        private TcpIpClient client;

        private string serverIp = "192.168.50.224";
        private int serverPort = 3000;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Console.WriteLine("tcp server start btn click");
            server = new TcpIpServer(serverIp, serverPort);
            server.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Console.WriteLine("tcp server stop btn click");

            if (client != null)
            {
                client.Disconnect();
            }
            if (server != null)
            {
                server.Stop();
            }
        }
    }
}
