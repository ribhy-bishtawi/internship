using AirportTrackingSystem.Interfaces;

public class FileReader : IFileReader
{
    public string[] ReadAllLines(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        return File.ReadAllLines(filePath);
    }
}
