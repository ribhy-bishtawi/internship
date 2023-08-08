using AirportTrackingSystem.Models;
namespace AirportTrackingSystem.Controllers;

public class PassengerController
{
    private List<Passenger> passengers = new List<Passenger>();
    public bool IsLoggedIn { set; get; } = false;
    private string CurrentPassengerUsername { set; get; } = string.Empty;

    public bool AddPassenger(string name, string password)
    {
        passengers.Add(new Passenger { Name = name, Password = password, Flights = new List<Flight>() });
        IsLoggedIn = true;
        CurrentPassengerUsername = name;
        return IsLoggedIn;
    }
    public void BookFlight(string username, Flight flight)
    {
        Passenger passenger = passengers.SingleOrDefault(passenger => passenger.Name == username);
        passenger.Flights.Add(flight);

    }
    public void ShowPassengers()
    {
        foreach (var item in passengers)
        {
            Console.WriteLine($"Name={item.Name}");
        }

    }



}
