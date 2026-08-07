using NAudio.Wave;
using System.Net.Sockets;

UdpClient sender = new UdpClient();
string targetIP = "127.0.0.1";
int targetPort = 5500;

using var waveIn = new WaveInEvent();
waveIn.WaveFormat = new WaveFormat(48000, 1);
waveIn.DeviceNumber = 1;
waveIn.BufferMilliseconds = 20;

waveIn.DataAvailable += (s, e) =>
{
    sender.Send(e.Buffer, e.BytesRecorded, targetIP, targetPort);
};

waveIn.StartRecording();
Console.WriteLine("Streaming mic audio... press Enter to stop");
Console.ReadLine();
waveIn.StopRecording();