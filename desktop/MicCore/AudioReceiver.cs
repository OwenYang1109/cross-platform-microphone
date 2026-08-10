using NAudio.Wave;
using System.Net;
using System.Net.Sockets;

namespace MicCore;

public class AudioReceiver
{   
    private UdpClient? receiver;
    private WaveOutEvent? waveOut;
    private bool isRunning = false;

    public void Start(int port)
    {
        var waveFormat = new WaveFormat(48000, 1);
        var buffer = new BufferedWaveProvider(waveFormat);
        waveOut = new WaveOutEvent();
        waveOut.DeviceNumber = 1; // Cable Output
        waveOut.Init(buffer);
        waveOut.Play();

        receiver = new UdpClient(port);
        isRunning = true;

        Task.Run(() =>
        {
            while (isRunning)
            {
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = receiver.Receive(ref remoteEndPoint);
                buffer.AddSamples(data, 0, data.Length);
            }
        });
    }

    public void Stop()
    {
        isRunning = false;
        receiver?.Close();
        waveOut?.Stop();
    }
}