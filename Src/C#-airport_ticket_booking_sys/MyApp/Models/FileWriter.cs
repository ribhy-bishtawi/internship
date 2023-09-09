using AirportTrackingSystem.Interfaces;

public class FileWriter : IFileWriter
{
    public void WriteToFile(string filePath, string content)
    {
        using (StreamWriter writer = new StreamWriter(filePath, append: true))
        {
            writer.WriteLine(content);
        }
    }
}
