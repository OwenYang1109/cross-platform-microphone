using MicCore;

namespace MicCore.Tests;

public class AudioSenderTests {
    [Fact]
    public void Start_WithInvalidDeviceName_ThrowsArgumentException() {
        var sender = new AudioSender();

        Assert.Throws<ArgumentException>(() => {
            sender.Start("127.0.0.1", 5500, "This Device Does Not Exist");
        });
    }
    [Fact]
    public void GetOutputDevices_ReturnsNonEmptyList() {
        var devices = AudioReceiver.GetOutputDevices();
        Assert.NotEmpty(devices);
    }
    [Fact]
    public void Start_WithValidDeviceName_DoesNotThrow() {
        var devices = AudioSender.GetInputDevices();
        var sender = new AudioSender();

        try {
            sender.Start("127.0.0.1", 5500, devices[0]);
        } finally {
            sender.Stop();
        }
    }
}