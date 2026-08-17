using NAudio.Wave;
using System.Net;
using System.Net.Sockets;
using NAudio.CoreAudioApi;

namespace MicCore;

/// <summary>
/// Listens on a given UDP port for incoming audio and plays it
/// through a specified output device. Intended to output into a
/// virtual audio device (VB-CABLE) so other apps can pick up
/// the received audio as a real microphone input.
/// </summary>

public class AudioReceiver
{   
    private UdpClient? receiver;
    private WaveOutEvent? waveOut;
    private bool isRunning = false;

    // Define Start method for DesktopReceiver.cs 
    // Starts listening on the given UDP port and plays
    // incoming audio through the given output device.
    // parameters are port number(5500) and name of 
    // output device(CABLE Input (VB-Audio Virtual Cable))
    public void Start(int port, string outputDeviceName)
    {
        var enumerator = new MMDeviceEnumerator();
        var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

        // Find device number for given name parameter
        int deviceIndex = -1;
        for(int i = 0; i < devices.Count; i++) {
            if(devices[i].FriendlyName == outputDeviceName) {
                deviceIndex = i;
                break;
            }
        }

        if(deviceIndex == -1) {
            throw new ArgumentException($"Device '{outputDeviceName}' not found.");

        }

        var waveFormat = new WaveFormat(48000, 1);
        var buffer = new BufferedWaveProvider(waveFormat);
        waveOut = new WaveOutEvent();
        // set DeviceNumber as deviceIndex found previously
        waveOut.DeviceNumber = deviceIndex;
        waveOut.Init(buffer);
        waveOut.Play();

        receiver = new UdpClient(port);
        isRunning = true;

        // Runs on a background thread so Start() doesn't block the caller
        // Loops until Stop() sets isRunning to false, receiving one UDP
        // packet at a time and feeding it straight into the playback buffer.
        Task.Run(() =>
        {
            while (isRunning)
            {
                // try catch block in case of exception in audioReceiver 
                // background loop(network drops, etc)
                try {
                    IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data = receiver.Receive(ref remoteEndPoint);
                    //Console.WriteLine($"Received {data.Length} bytes");
                    buffer.AddSamples(data, 0, data.Length);
                } catch(Exception ex) when(isRunning) {
                    Console.WriteLine($"Receive error: {ex.Message}");
                }
            }
        });
    }

    // Stops receiving and playback
    public void Stop()
    {
        isRunning = false;
        receiver?.Close();
        waveOut?.Stop();
    }

    // Intended for Input and Output Device selection menu in UI.
    // Returns the friendly names of all active output devices Windows
    // currently sees (e.g. speakers, headphones, "CABLE Input"). Used to
    // populate a device picker in the UI, and to get an exact name to
    // pass into Start().
    public static List<string> GetOutputDevices() {
        var enumerator = new MMDeviceEnumerator();
        var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
        return devices.Select(d => d.FriendlyName).ToList();
    }
}