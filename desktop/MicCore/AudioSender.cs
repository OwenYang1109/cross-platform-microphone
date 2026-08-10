using NAudio.Wave;
using System.Net.Sockets;
using System.Net;

namespace MicCore;

public class AudioSender
{
    private UdpClient sender = new UdpClient();
    private WaveInEvent? waveIn;

    public void Start(string targetIp, int targetPort) {
        waveIn = new WaveInEvent();
        waveIn.WaveFormat = new WaveFormat(48000, 1);
        waveIn.BufferMilliseconds = 20;
        waveIn.DeviceNumber = 2;

        waveIn.DataAvailable += (s, e) => {
            sender.Send(e.Buffer, e.BytesRecorded, targetIp, targetPort);
        };

        waveIn.StartRecording();
    }

    public void Stop() {
        waveIn?.StopRecording();
    }
}
