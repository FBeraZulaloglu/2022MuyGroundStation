using System;
using System.Text;
using System.Windows;
using SimpleTCP;
using System.IO.Ports;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace _2022MuyGroundStation
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        int Port;
        string IpAdress;
        Socket listener;
        Thread n_server;
        Thread control_server;
        int server_running = 0;
        Socket handler; // Socket to send message to client or get message from client

        public Window1()
        {
            InitializeComponent();
            // Default values
            server_ip.Text = "localhost";
            server_port.Text = "11000";
            /*server = new SimpleTcpServer();
            server.StringEncoder = Encoding.UTF8;
            server.Delimiter = 0X13; // enter;
            server.DataReceived += ServerDataReceived;
            */
        }

        public void CreateServerPort(string ip,int port)
        {
            if (server_running == 0)
            {
                server_running = 1;
                IPHostEntry host = Dns.GetHostEntry(ip);
                IPAddress ipAdress = host.AddressList[0]; // if a host has multiple adresses u will get a list of adresses
                foreach(IPAddress i in host.AddressList)
                {
                    Console.WriteLine("adress");
                }

                IPEndPoint localEndPoint = new IPEndPoint(ipAdress, port);
                // Create a socket that use TCP protocol
                listener = new Socket(ipAdress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(localEndPoint);
                n_server = new Thread(new ThreadStart(OpenServer));
                n_server.IsBackground = true;
                n_server.Start();
                control_server = new Thread(new ThreadStart(ControlServer));
                control_server.IsBackground = true;
                control_server.Start();
            }
        }

        private void ControlServer()
        {
            while (true)
            {
                if (server_running == 0)
                {
                    Console.WriteLine("CONNECTION CLOSED");
                    handler.Shutdown(SocketShutdown.Both); // disables a socket both sending and recieving
                    handler.Close();
                    listener.Close();
                    Console.WriteLine("ABORTED");
                    Dispatcher.Invoke((Action)delegate () { serverInfo_txt.Text = "[INFO:] Server is closed"; });
                    return;
                }
            }
            
        }

        private void OpenServer()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Listenningggggg!!!");
                    
                    listener.Listen(10); // Listen 10 requests at a time
                    Dispatcher.Invoke((Action)delegate () { serverInfo_txt.Text = "[INFO:] Server is waiting for a connection..."; });
                    
                    handler = listener.Accept(); // wait until a client is connected to server

                    Dispatcher.Invoke((Action)delegate () { serverInfo_txt.Text = "[INFO:] Client has connected to the server"; });

                    string data = null;
                    byte[] bytes = null;

                    while (true)
                    {
                        bytes = new byte[1024];
                        Dispatcher.Invoke((Action)delegate () { getData_txt.Text += "GS said : " + data + "\n"; });
                        Console.WriteLine("Recieving...");
                        int bytesRec = handler.Receive(bytes); // count of bytes that has been recieved
                        Console.WriteLine("Recieved");
                        Console.WriteLine(bytesRec + "byte has recieved from Model Satellite");
                        data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                       
                        if (data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                    }
                    Dispatcher.Invoke((Action)delegate () { getData_txt.Text += "GS said : " + data + "\n"; });
                }
                catch (ThreadAbortException ex)
                {
                    Console.WriteLine("Thread is aborted and the code is "
                                                     + ex.ExceptionState);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Socket is closed " + ex.Message);

                }

            }
            
        }

        private  void StartServer(object sender, RoutedEventArgs e)
        {
            this.IpAdress = server_ip.Text;
            this.Port = Int32.Parse(server_port.Text);
            
            CreateServerPort(this.IpAdress, this.Port);
        }


        private void StopServer(object sender, RoutedEventArgs e)
        {
            server_running = 0;
        }


        private void SendDataToClient(object sender, RoutedEventArgs e)
        {
            if(handler != null)
            {
                byte[] msg = Encoding.ASCII.GetBytes(sendData_txt.Text);
                handler.Send(msg);
            }
        }

        private void StartDataTransmission(object sender, RoutedEventArgs e)
        {
            // send model satellite information from here to ground station at 1Hz
        }
    }
}
