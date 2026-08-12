using MicCore;

var receiver = new AudioReceiver();
receiver.Start(5500, "CABLE Input (VB-Audio Virtual Cable)");

Console.WriteLine("Receiving on port 5500, Press Enter to stop");
Console.ReadLine();

receiver.Stop();