using NAudio.Wave;
using System.Net;
using System.Net.Sockets;
using NAudio.CoreAudioApi;

namespace MicCore;

public class AudioReceiver
{   
    private UdpClient? receiver;
    private WaveOutEvent? waveOut;
    private bool isRunning = false;

    public void Start(int port, string outputDeviceName)
    {
        var enumerator = new MMDeviceEnumerator();
        var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

        int deviceIndex = -1;
        for(int i = 0; i < devices.Count; i++) {
            if(devices[i].FriendlyName == outputDeviceName) {
                deviceIndex = i;
                break;
            }
        }

        if(deviceIndex == -1) {
            throw new ArgumentException($"DEvice '{outputDeviceName}' not found.");

        }

        var waveFormat = new WaveFormat(48000, 1);
        var buffer = new BufferedWaveProvider(waveFormat);
        waveOut = new WaveOutEvent();
        waveOut.DeviceNumber = deviceIndex;
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

    public static List<string> GetOutputDevices() {
        var enumerator = new MMDeviceEnumerator();
        var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
        return devices.Select(d => d.FriendlyName).ToList();
    }
}