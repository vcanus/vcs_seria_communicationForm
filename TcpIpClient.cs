using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace vcs_seria_communicationForm
{
    class TcpIpClient
    {
        private System.Net.Sockets.TcpClient client;
        private NetworkStream stream;
        private Thread threadReceive;
        private string ipAddress;
        private int port;
        private bool isConnected;

        private int bufferSize = 256;

        public static int waitingTimeToClose = 10; // ms

        public TcpIpClient(string ipAddress, int port)
        {
            isConnected = false;

            this.ipAddress = ipAddress;
            this.port = port;

            threadReceive = null;
        }


        public TcpIpClient(System.Net.Sockets.TcpClient client)
        {
            this.client = client;
            this.stream = client.GetStream();
            this.ipAddress = "Hello!";
        }

        public void Disconnect()
        {
            isConnected = false;
            try
            {
                //                if (threadReceive != null && threadReceive.IsAlive)
                //                {
                //                    threadReceive.Interrupt();
                //                }

                if (stream != null)
                {
                    stream.Close();
                }

                if (client != null)
                {
                    client.Close();
                }
            }
            catch (Exception e)
            {
                Debug.Print("[Exception] TcpIpClient::Disconnect() {0}", e.Message);
            }
        }

        public string Read()
        {
            string message = null;
            try
            {
                if (stream.CanRead)
                {
                    byte[] bytes = new byte[bufferSize];
                    int result = stream.Read(bytes, 0, bytes.Length);
                    if (result > 0)
                    {
                        message = System.Text.Encoding.ASCII.GetString(bytes);
                    }
                }
            }
            catch (Exception e)
            {
                message = null;
                Debug.Print("[Exception] TcpIpClient::Read() {0}", e.Message);
            }
            return message;
        }

        public void Write(string message)
        {
            try
            {
                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(message);
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
            }
            catch (Exception e)
            {
                Debug.Print("[Exception] TcpIpClient::Write() {0}", e.Message);
            }
        }

        public string EndPoint()
        {
            return ((IPEndPoint)(this.client.Client.RemoteEndPoint)).Address.ToString();
        }

    }
}
