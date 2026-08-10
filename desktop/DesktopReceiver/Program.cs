using MicCore;

var receiver = new AudioReceiver();
receiver.Start(5500);

Console.WriteLine("Receiving on port 5500, Press Enter to stop");
Console.ReadLine();

receiver.Stop();