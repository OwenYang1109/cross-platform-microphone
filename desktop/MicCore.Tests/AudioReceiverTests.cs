using MicCore;

namespace MicCore.Tests;

public class AudioReceiverTests {
    [Fact]
    public void Start_WithInvalidDeviceName_ThrowsArgumentException() {
        var receiver = new AudioReceiver();

        Assert.Throws<ArgumentException>(() => {
            receiver.Start(5500, "This Device Does Not Exist");
        });
    }
    [Fact]
    public void GetOutputDevices_ReturnsNonEmptyList() {
        var devices = AudioReceiver.GetOutputDevices();
        Assert.NotEmpty(devices);
    }
}