using NAudio.Wave;
using System.Net.Sockets;
using System.Net;
using NAudio.CoreAudioApi;

namespace MicCore;

/// <summary>
/// Captures audio from a specified input device and streams it
/// over UDP to a target IP and port, following the project's protocol spec
/// (48kHz, mono, ~20ms chunks). Used by both the mock sender (testing)
/// and eventually the real mobile app's equivalent logic.
/// </summary>

public class AudioSender
{
    private UdpClient sender = new UdpClient();
    private WaveInEvent? waveIn;
    
    // Defines Start method for MockSender.cs(Later on from iPhone)
    // Starts capturing from the given input device and streams audio
    // to the given target over UDP, using format defind in protocol spec
    // (48kHz, mono, 20ms chunks per packet).
    // Parameters are the target IP, port to send to(5500), and the name
    // of the input device to capture from(Microphone Array (Realtek(R) Audio)).
    public void Start(string targetIp, int targetPort, string inputDeviceName) {
    var enumerator = new MMDeviceEnumerator();
    var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);

    // Find device number for given name parameter
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

    // Runs every 20ms as new audio arrives from microphone.
    // e.BytesRecorded is the actual amount of new data in this
    // chunk, since NAudio reuses the same buffer array.
    waveIn.DataAvailable += (s, e) =>
    {
        sender.Send(e.Buffer, e.BytesRecorded, targetIp, targetPort);
    };

    waveIn.StartRecording();
    }

    // Stops capturing audio from microphone
    public void Stop() {
        waveIn?.StopRecording();
    }

    // Intended for Input and Output Device selection menu in UI.
    // Returns the friendly names of all active output devices Windows
    // currently sees (e.g. headset mic). Used to
    // populate a device picker in the UI, and to get an exact name to
    // pass into Start().
    public static List<string> GetInputDevices() {
    var enumerator = new MMDeviceEnumerator();
    var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
    return devices.Select(d => d.FriendlyName).ToList();
    }
}
