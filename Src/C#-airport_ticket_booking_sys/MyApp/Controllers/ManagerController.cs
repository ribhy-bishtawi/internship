using AirportTrackingSystem.Models;
namespace AirportTrackingSystem.Controllers;

public class ManagerController
{
    private List<Passenger> passengers = new List<Passenger>();
    public bool IsLoggedIn { set; get; } = false;

    public bool AddPassenger(string name, string password)
    {
        passengers.Add(new Passenger { Name = name, Password = password, Flights = new List<Flight>() });
        IsLoggedIn = true;
        return IsLoggedIn;
    }
    public void BookFlight(Passenger passenger, Flight flight)
    {
        passenger.Flights?.Add(flight);

    }



}
