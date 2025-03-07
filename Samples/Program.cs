using Samples;
using Samples.PlaySound;

string? filePath;

while (true)
{
    Console.WriteLine("Enter sound path");
    filePath = Console.ReadLine();

    if (File.Exists(filePath))
        break;
    else
        Console.WriteLine("Sound doesn't exist");

    Console.WriteLine();
}

SoundWatcher fileWatcher    = new(filePath);
SoundPlayer soundPlayer     = new(filePath);

while (true)
{
    Console.WriteLine();
    Console.WriteLine(
        "[1] Show sound info\n" +
        "[2] Play sound\n" +
        "[3] Exit");
    Console.WriteLine();

    string? result = Console.ReadLine();

    if (int.TryParse(result, out int value))
    {
        if (value == 1)
            fileWatcher.ShowInfo();
        else if (value == 2)
            soundPlayer.Play();
        else if (value == 3)
            break;
        else
            Console.WriteLine("Wrong choice");
    }
    else
        Console.WriteLine("Wrong choice");
}
