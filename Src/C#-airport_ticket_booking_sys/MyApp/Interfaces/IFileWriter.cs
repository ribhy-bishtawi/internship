namespace AirportTrackingSystem.Interfaces;

public interface IFileWriter
{
    void WriteToFile(string filePath, string content);
}
