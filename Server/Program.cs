using System.Net;
using System.Net.Sockets;
using System.Text;


public class ChatServer
{
    const int port = 4040;
    const string JOIN_CMD = "$<Join>";
    const string Leave_CMD = "$(Leave)";
    List<IPEndPoint> members;
    UdpClient udpClient;
    IPEndPoint clientEndPoint = null;

    public ChatServer()
    {
        udpClient = new UdpClient(port);
        members = new List<IPEndPoint>();
    }

    public void Start()
    {
        while (true)
        {
            byte[] data = udpClient.Receive(ref clientEndPoint);
            string message = Encoding.Unicode.GetString(data);

            if (message.StartsWith(JOIN_CMD))
            {
                string name = message.Substring(JOIN_CMD.Length);
                AddMember(clientEndPoint, name);
                continue;
            }
            switch (message)
            {
                case JOIN_CMD:
                    break;
                case Leave_CMD:
                    DeleteMember(clientEndPoint);
                    break;
                default:
                    Console.WriteLine($"Got message {message,-20} from {clientEndPoint} at {DateTime.Now.ToShortTimeString()}");
                    SendToAll(message);
                    break;
            }
        }
    }
    private void AddMember(IPEndPoint clientPoint, string name)
    {
        members.Add(clientPoint);
        Console.WriteLine($"{name} was added");
    }

    private void DeleteMember(IPEndPoint clientPoint)
    {
        members.Remove(clientPoint);
        Console.WriteLine("Member was Deleted");

    }
    private void SendToAll(string message)
    {
        byte[] data = Encoding.Unicode.GetBytes(message);
        foreach (var member in members)
        {
            udpClient.SendAsync(data, data.Length, member);
        }
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        ChatServer chat = new ChatServer();
        chat.Start();
    }
}