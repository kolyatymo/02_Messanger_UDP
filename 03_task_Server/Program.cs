
using System.Net.Sockets;
using System.Net;
using _03_task_db;
using Microsoft.EntityFrameworkCore;

public class ChatServer
{
    const int port = 4040;
    const string JOIN_CMD = "$<Join>";
    TcpListener server;
    IPEndPoint address = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
    public ChatServer()
    {
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
            string message = sr.ReadLine()!;

            if (message == "$<close>")
            {
                ns.Close();
                server?.Stop();
                break;
            }

            if (message.Length > 2)
            {
                Console.WriteLine($"{client.Client.LocalEndPoint} --> {message} from Numb {ShowNumb(message)}");
                sw.WriteLine(ShowNumb(message));
                sw.Flush();
            }
            else
            {
                Console.WriteLine($"{client.Client.LocalEndPoint} --> {message} from Region {ShowRegion(message)}");
                sw.WriteLine(ShowRegion(message));
                sw.Flush();
            }

        }
    }

    public string ShowRegion(string numb)
    {
        dbNumberCars db = new dbNumberCars();

        var result = db.Cars.Include(c => c.Region).FirstOrDefault(f => f.RegionCode == numb);

        if (result == null)
        {
            return "region not fount";
        }
        return result.Region.Name;
    }

    public string ShowNumb(string region)
    {
        dbNumberCars db = new dbNumberCars();

        var result = db.Region.Include(c => c.Cars).FirstOrDefault(f => f.Name == region);

        if (result == null)
        {
            return "region not fount";
        }
        return string.Join(". ",result.Cars.Select(c => c.RegionCode));
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