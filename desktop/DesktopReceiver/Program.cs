using NAudio.Wave;
using System.Net;
using System.Net.Sockets;

UdpClient receiver = new UdpClient(5500);
Console.WriteLine("Listening on port 5500...");

var waveFormat = new WaveFormat(48000, 1);
var buffer = new BufferedWaveProvider(waveFormat);
using var waveOut = new WaveOutEvent();
waveOut.Init(buffer);
waveOut.Play();

while (true);
{
    IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
    byte[] data = receiver.Receive(ref remoteEndPoint);
    buffer.AddSamples(data, 0, data.Length);
}