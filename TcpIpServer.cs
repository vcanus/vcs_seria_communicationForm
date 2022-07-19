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
    class TcpIpServer
    {
        private TcpListener server;
        private Dictionary<string, TcpIpClient> clientDictionary = new Dictionary<string, TcpIpClient>();
        public Dictionary<string, TcpIpClient> GetClientDictionary()
        {
            Dictionary<string, TcpIpClient> dictionary = null;
            if (clientDictionary != null)
            {
                dictionary = new Dictionary<string, TcpIpClient>();
                mutex.WaitOne();
                foreach (var v in clientDictionary)
                {
                    dictionary.Add(v.Key, v.Value);
                }
                mutex.ReleaseMutex();
            }
            return dictionary;
        }

        private Thread threadAcceptClient;
        private Dictionary<string, Thread> threadDictionary;
        private string ipAddress;
        private int port;
        private bool isStarted;
        private CommandExecute commandExecute;

        private Mutex mutex = null;

        private TextWriterTraceListener logfile = new TextWriterTraceListener(System.IO.File.CreateText("logfile.txt"));

        public TcpIpServer(string ipAddress, int port)
        {
            Debug.Listeners.Add(logfile);

            threadAcceptClient = null;
            threadDictionary = new Dictionary<string, Thread>();
            commandExecute = new CommandExecute();

            Guid guid = Guid.NewGuid();
            mutex = new Mutex();

            isStarted = false;

            this.ipAddress = ipAddress;
            this.port = port;
        }

        ~TcpIpServer()
        {
            mutex.Close();
            if (clientDictionary != null)
            {
                clientDictionary.Clear();
            }
        }

        public bool IsStarted { get => isStarted; }

        public void Start()
        {
            if (isStarted)
            {
                return;
            }

            try
            {
                if (server == null)
                {
                    IPAddress ipAddress = IPAddress.Parse(this.ipAddress);
                    System.Console.WriteLine(this.ipAddress);
                    Debug.WriteLine(this.ipAddress + " server start");
                    server = new TcpListener(ipAddress, port);
                }
                server.Start();
            }
            catch (Exception e)
            {
                Debug.Print("TcpIpServer::Connect(), {0}", e.Message);
                //MessageBox.Show("Invalid ip or port to start server");
                return;
            }

            isStarted = true;

            if (threadAcceptClient == null)
            {
                threadAcceptClient = new Thread(new ThreadStart(AcceptClient));
                threadAcceptClient.Start();
            }

            System.Console.WriteLine("server start");
        }

        public void Stop()
        {
            isStarted = false;

            mutex.WaitOne();
            foreach (KeyValuePair<string, TcpIpClient> pair in clientDictionary)
            {
                try
                {
                    pair.Value.Disconnect();
                }
                catch (Exception e)
                {
                    Debug.Print("[Exception] TcpIpServer::Disconnect() {0}", e.Message);
                }
            }
            mutex.ReleaseMutex();

            foreach (KeyValuePair<string, Thread> pair in threadDictionary)
            {
                pair.Value.Interrupt();
            }
            if (server != null)
            {
                System.Console.WriteLine("server stop");
                Debug.WriteLine("server stop");
                server.Stop();
            }
        }

        private void AcceptClient()
        {
            while (isStarted)
            {
                try
                {
                    TcpClient c = new TcpClient();
                    TcpIpClient client = new TcpIpClient(server.AcceptTcpClient());
                    string key = client.EndPoint();
                    bool keyExist = false;
                    // mutex to access clientDictionary
                    mutex.WaitOne();
                    if (!clientDictionary.ContainsKey(key))
                    {
                        clientDictionary.Add(key, client);
                        System.Console.WriteLine(key + "is Connected");
                        Debug.WriteLine(key + "is Connected");
                    }
                    else
                    {
                        keyExist = true;
                    }
                    mutex.ReleaseMutex();
                    if (!keyExist)
                    {
                        WriteToAll(key + " is connected to server");
                        Thread threadListen = new Thread(new ParameterizedThreadStart(Listen));
                        this.threadDictionary.Add(key, threadListen);
                        threadListen.Start(client);
                    }
                }
                catch (Exception e)
                {
                    Debug.Print("[Exception] TcpIpServer::AddClient() {0}", e.Message);
                }
            }
        }

        private void Listen(Object obj)
        {
            TcpIpClient client = (TcpIpClient)(obj);
            while (isStarted)
            {
                try
                {
                    string message = client.Read();
                    if (message != null)
                    {
                        System.Console.WriteLine(message);
                        Debug.WriteLine("read:" + message);
                        if (commandExecute != null)
                        {
                            String result = commandExecute.commandExecuteFunction(Parse(message));
                            WriteToAll("result : " + result);
                            Debug.WriteLine("result:" + result);
                        }
                        
                    }
                }
                catch (Exception e)
                {
                    Debug.Print("[Exception] TcpIpServer::Listen() {0}", e.Message);
                    mutex.WaitOne();
                    clientDictionary.Remove(client.EndPoint());
                    mutex.ReleaseMutex();
                    break;
                }
            }
        }

        public async void WriteToAll(string message)
        {
            //            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(message);
            //            stream.Write(bytes, 0, bytes.Length);

            var task = Task.Run(() =>
            {
                mutex.WaitOne();
                foreach (KeyValuePair<string, TcpIpClient> pair in clientDictionary)
                {
                    pair.Value.Write(message);
                }
                mutex.ReleaseMutex();
            });

            await task;
        }

        public async void Write(string clientName, string message)
        {
            var task = Task.Run(() =>
            {
                mutex.WaitOne();
                TcpIpClient client = null;
                if (clientDictionary.TryGetValue(clientName, out client))
                {
                    client.Write(message);
                }
                mutex.ReleaseMutex();
            });
            await task;
        }

        public string Parse(string command)
        {
            //command = command.Trim();
            command = command.Replace("\0", "");
            return command;
        }
    }
}
