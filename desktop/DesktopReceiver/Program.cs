using System.Net;
using System.Net.Sockets;

UdpClient receiver = new UdpClient(5500);
Console.WriteLine("Listening on port 5500...");

while (true)
{
    IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
    byte[] data = receiver.Receive(ref remoteEndPoint);
    Console.WriteLine($"Received {data.Length} bytes from {remoteEndPoint}");
}