using NAudio.Wave;

using var waveIn = new WaveInEvent();
waveIn.DeviceNumber = 1;
waveIn.BufferMilliseconds = 20;
waveIn.WaveFormat = new WaveFormat(48000, 1);

var buffer = new BufferedWaveProvider(waveIn.WaveFormat);
using var waveOut = new WaveOutEvent();
waveOut.Init(buffer);

waveIn.DataAvailable += (sender, e) =>
{
    buffer.AddSamples(e.Buffer, 0, e.BytesRecorded);
};

waveIn.StartRecording();
waveOut.Play();
Console.WriteLine("Recording... press Enter to stop.");
Console.ReadLine();
waveIn.StopRecording();
waveOut.Stop();

/*
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
*/