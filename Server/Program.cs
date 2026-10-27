using System.Net;
using System.Net.Sockets;
using System.Text;


public class ChatServer
{
    const int port = 4040;
    const string JOIN_CMD = "$<Join>";
    //const string Leave_CMD = "$(Leave)";

    TcpListener server;

    //List<IPEndPoint> members;
    //UdpClient udpClient;
    IPEndPoint address = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
    //IPEndPoint clientEndPoint = null;

    public ChatServer()
    {
        /*udpClient = new UdpClient(port);
        members = new List<IPEndPoint>();*/

        server = new TcpListener(address);
    }

    public void Start()
    {
        server.Start();
        Console.WriteLine("Waiting for connection");
        TcpClient client = server.AcceptTcpClient();
        Console.WriteLine("Connected!!!");
        NetworkStream ns = client.GetStream();
        StreamReader sr = new StreamReader(ns);
        StreamWriter sw = new StreamWriter(ns);
        while (true)
        {
            string message = sr.ReadLine()!; // ! --> not null

            if(message == "$<close>")
            {
                ns.Close();
                server?.Stop();
                break;
            }

            Console.WriteLine($"Got message {message} from {client.Client.LocalEndPoint} {DateTime.Now.ToShortTimeString()} ");

            sw.WriteLine(message);
            sw.Flush();
        }
            /*byte[] data = udpClient.Receive(ref clientEndPoint);
            string message = Encoding.Unicode.GetString(data);*/

            /*if (message.StartsWith(JOIN_CMD))
            {
                string name = message.Substring(JOIN_CMD.Length);
                AddMember(clientEndPoint, name);
                continue;
            }*/
           /* switch (message)
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
            }*/
        
    }
    /*private void AddMember(IPEndPoint clientPoint, string name)
    {
        if (members.Contains(clientPoint))
        {
            Console.WriteLine("member tyt");
        }
        else
        {
            members.Add(clientPoint);
            Console.WriteLine($"{name} was added");
        }
    }

    private void DeleteMember(IPEndPoint clientPoint)
    {
        members.Remove(clientPoint);
        Console.WriteLine("Member was Deleted");

    }*/
    /*private void SendToAll(string message)
    {
        byte[] data = Encoding.Unicode.GetBytes(message);
        foreach (var member in members)
        {
            udpClient.SendAsync(data, data.Length, member);
        }
    }*/
}

internal class Program
{
    private static void Main(string[] args)
    {
        ChatServer chat = new ChatServer();
        chat.Start();
    }
}