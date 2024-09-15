namespace AirportTrackingSystem.Interfaces;

public interface IFileReader
{
    string[] ReadAllLines(string filePath);
}
