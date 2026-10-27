using System.Collections.ObjectModel;
using System.IO;
using System.Net.Sockets;
using System.Net;
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

namespace _03_task
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string serveraddress = "127.0.0.1";
        const int serverPort = 4040;
        IPEndPoint serverPoint;
        TcpClient client;
        ObservableCollection<MessageInfo> messages;

        NetworkStream ns = null;
        StreamReader sr = null;
        StreamWriter sw = null;
        public MainWindow()
        {
            InitializeComponent();
            serverPoint = new IPEndPoint(IPAddress.Parse(serveraddress), serverPort);
            messages = new ObservableCollection<MessageInfo>();
            this.DataContext = messages;
        }

        private void Select_Button(object sender, RoutedEventArgs e)
        {
            string message = MyTextBox.Text;
            MyTextBox.Clear();
            if (sw == null)
                return;

            sw.WriteLine(message);
            sw.Flush();
        }

        private void Connect_Button(object sender, RoutedEventArgs e)
        {
            try
            {
                client = new TcpClient();
                client.Connect(serverPoint);
                ns = client.GetStream();

                sr = new StreamReader(ns);
                sw = new StreamWriter(ns);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Listener();
        }

        private void Disconnect_Button(object sender, RoutedEventArgs e)
        {
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
            }
        }

        private void Select_Button_Region(object sender, RoutedEventArgs e)
        {
            string message = MyTextBoxRegion.Text;
            MyTextBoxRegion.Clear();
            if (sw == null)
                return;

            sw.WriteLine(message);
            sw.Flush();
        }
    }
}
public class MessageInfo
{
    public string Text { get; set; }
    public MessageInfo(string message)
    {
        Text = message;
    }
    public override string ToString()
    {
        return $"{Text,-20}";
    }
}