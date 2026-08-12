using MicCore;

var sender = new AudioSender();
sender.Start("127.0.0.1", 5500, "Microphone Array (Realtek(R) Audio)");

Console.WriteLine("Sending mic audio. Press Enter to stop");
Console.ReadLine();

sender.Stop();