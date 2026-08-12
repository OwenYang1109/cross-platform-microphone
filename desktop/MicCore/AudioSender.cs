using NAudio.Wave;
using System.Net.Sockets;
using System.Net;
using NAudio.CoreAudioApi;

namespace MicCore;

public class AudioSender
{
    private UdpClient sender = new UdpClient();
    private WaveInEvent? waveIn;

    public void Start(string targetIp, int targetPort, string inputDeviceName) {
    var enumerator = new MMDeviceEnumerator();
    var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);

    int deviceIndex = -1;
    for (int i = 0; i < devices.Count; i++)
    {
        if (devices[i].FriendlyName == inputDeviceName)
        {
            deviceIndex = i;
            break;
        }
    }

    if (deviceIndex == -1)
    {
        throw new ArgumentException($"Device '{inputDeviceName}' not found.");
    }

    waveIn = new WaveInEvent();
    waveIn.DeviceNumber = deviceIndex;
    waveIn.WaveFormat = new WaveFormat(48000, 1);
    waveIn.BufferMilliseconds = 20;

    waveIn.DataAvailable += (s, e) =>
    {
        sender.Send(e.Buffer, e.BytesRecorded, targetIp, targetPort);
    };

    waveIn.StartRecording();
    }

    public void Stop() {
        waveIn?.StopRecording();
    }

    public static List<string> GetInputDevices() {
    var enumerator = new MMDeviceEnumerator();
    var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
    return devices.Select(d => d.FriendlyName).ToList();
    }
}
