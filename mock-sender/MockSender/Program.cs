using System.Net.Sockets;
using System.Text;

UdpClient sender = new UdpClient();
byte[] message = Encoding.UTF8.GetBytes("test packet");

sender.Send(message, message.Length, "127.0.0.1", 5500);
Console.WriteLine("Sent");