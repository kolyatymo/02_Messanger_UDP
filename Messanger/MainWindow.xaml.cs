using System.Collections.ObjectModel;
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
        UdpClient client;
        ObservableCollection<MessageInfo> messages;
        public MainWindow()
        {
            InitializeComponent();
            client = new UdpClient();
            serverPoint = new IPEndPoint(IPAddress.Parse(serveraddress), serverPort);
            messages = new ObservableCollection<MessageInfo>();
            this.DataContext = messages;
        }

        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {
            if (msgTextBox.Text.Length == 0)
            {
                MessageBox.Show("Write text");
            }
            else
            {
                SendMessage(msgTextBox.Text);
                msgTextBox.Clear();
            }
        }
        private void Join_Button_Click(object sender, RoutedEventArgs e)
        {
            Window_Name Name = new Window_Name();
            Name.ShowDialog();

            if (Name.MyName.Text != "")
            {
                SendMessage("$<Join>" + Name.MyName.Text);
                Listener();
            }    
        }

        private void Leave_Button_Click(object sender, RoutedEventArgs e)
        {
            SendMessage("$(Leave)");
        }

        private async void SendMessage(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);

            await client.SendAsync(data, data.Length, serverPoint);
        }
        private async void Listener()
        {
            while (true)
            {
                var data = await client.ReceiveAsync();
                string message = Encoding.Unicode.GetString(data.Buffer);
                messages.Add(new MessageInfo(message));
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