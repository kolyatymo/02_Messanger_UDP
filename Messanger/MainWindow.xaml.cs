using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Messanger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string serveraddress = "127.0.0.1";
        const int serverPort = 4040;
        IPEndPoint serverPoint;
        //UdpClient client;
        TcpClient client;
        ObservableCollection<MessageInfo> messages;

        NetworkStream ns = null;
        StreamReader sr = null;
        StreamWriter sw = null;
        public MainWindow()
        {
            InitializeComponent();
            //client = new UdpClient();
            serverPoint = new IPEndPoint(IPAddress.Parse(serveraddress), serverPort);
            messages = new ObservableCollection<MessageInfo>();
            this.DataContext = messages;
        }

        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {
            /*if (msgTextBox.Text.Length == 0)
            {
                MessageBox.Show("Write text");
            }
            else
            {
                SendMessage(msgTextBox.Text);
                msgTextBox.Clear();
            }*/

            string message = msgTextBox.Text;
            msgTextBox.Clear();
            if (sw == null)
                return;

            sw.WriteLine(message);
            sw.Flush();
            
        }
        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            /*            Window_Name Name = new Window_Name();
                        Name.ShowDialog();*/
            /*
                        if (Name.MyName.Text != "")
                        {*/
            //SendMessage("$<Join>" + Name.MyName.Text);
            //}

            try
            {
                client = new TcpClient();
                client.Connect(serverPoint);
                ns = client.GetStream();

                sr = new StreamReader(ns);
                sw = new StreamWriter(ns);

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Listener();

        }

        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            //SendMessage("$(Leave)");

            try
            {
                sw.WriteLine("$<close>");
                sw.Flush();

                ns.Close();
                client.Close();

                sw = null;
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void SendMessage(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);

            //await client.SendAsync(data, data.Length, serverPoint);
        }
        private async void Listener()
        {
            while (true)
            {

                try
                {
                    string? message = await sr.ReadLineAsync();
                    if (message != null)
                        messages.Add(new MessageInfo(message));
                }
                catch (Exception ex)
                {
                    break;
                }

                /*var data = await client.ReceiveAsync();
                string message = Encoding.Unicode.GetString(data.Buffer);
                messages.Add(new MessageInfo(message));*/
            }
        }
    }
}
public class MessageInfo
{
    public string Text { get; set; }
    public DateTime Time { get; set; }
    public MessageInfo(string message)
    {
        Text = message;
        Time = DateTime.Now;
    }
    public override string ToString()
    {
        return $"{Text, -20} {Time}";
    }
}