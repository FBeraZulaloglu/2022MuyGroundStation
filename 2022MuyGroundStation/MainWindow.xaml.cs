using SimpleTCP;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Windows;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace _2022MuyGroundStation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Window1 satellite;
        int port;
        string ip;
        bool stopCommunication = false;
        bool isConnectedToServer = false;
        Socket groundStationSocket;
        //SimpleTcpClient client;

        public MainWindow()
        {
            InitializeComponent();
            OpenModelSatellite();

            // Default values
            host_ip.Text = "localhost";
            host_port.Text = "11000";

            /*
            client = new SimpleTcpClient();
            client.StringEncoder = Encoding.UTF8;
            
            client.DataReceived += ClientDataReceived;*/
        }


        private void OpenModelSatellite()
        {
            satellite = new Window1();
            satellite.Visibility = Visibility.Visible;
            
        }


        private void SendData(object sender, RoutedEventArgs e)
        {
            if (isConnectedToServer)
            {
                string msg = sendData_txt.Text;
                byte[] bmsg = Encoding.ASCII.GetBytes(msg+"<EOF>");
                int byteSent = groundStationSocket.Send(bmsg);
                Console.WriteLine(byteSent+"byte has sent to Model Satellite" );
            }        
        }

        private void ConnectServer(object sender, RoutedEventArgs e)
        {

            Thread n_server = new Thread(new ThreadStart(ConnectClientToServer));
            n_server.IsBackground = true;
            n_server.Start();
        }

        private void ConnectClientToServer()
        {
            byte[] bytes = new byte[1024];
            
            try
            {
                Dispatcher.Invoke((Action)delegate () {
                    this.port = Int32.Parse(host_port.Text);
                    this.ip = host_ip.Text;
                });
                IPHostEntry host = Dns.GetHostEntry(this.ip);
                IPAddress ipAdress = host.AddressList[0];
                IPEndPoint remoteEp = new IPEndPoint(ipAdress,this.port);

                groundStationSocket = new Socket(ipAdress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    groundStationSocket.Connect(remoteEp);
                    isConnectedToServer = true;
                    Dispatcher.Invoke((Action)delegate () { status.Text = "Socket connected to {0}" + groundStationSocket.RemoteEndPoint.ToString(); });

                    while (groundStationSocket.IsBound)
                    {
                        int bytesRec = groundStationSocket.Receive(bytes);
                        Dispatcher.Invoke((Action)delegate () { allDataFromServer_txt.Text += "\r" + Encoding.ASCII.GetString(bytes, 0, bytesRec)+"\n"; });
                        if (stopCommunication)
                        {
                            groundStationSocket.Shutdown(SocketShutdown.Both);
                            groundStationSocket.Close();
                            isConnectedToServer = false;
                        }
                        
                    }
                    
                }
                catch (ArgumentNullException ane)
                {
                    Dispatcher.Invoke((Action)delegate () { status.Text = "[ERROR] : Argument is null!"; });
                    
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Dispatcher.Invoke((Action)delegate () { status.Text = "[ERROR] : Socket is not connected!"; });
                    
                    Console.WriteLine("Socket Exception : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Dispatcher.Invoke((Action)delegate () { status.Text = "[ERROR] : {0}" + e.ToString(); });
                   
                    Console.WriteLine("Exception : {0}", e.ToString());
                }
            }

            catch (Exception e)
            {
                Dispatcher.Invoke((Action)delegate () { status.Text = "[ERROR] : {0}" + e.ToString(); });
                Console.WriteLine(e.ToString());
            }
        }
    }
}
