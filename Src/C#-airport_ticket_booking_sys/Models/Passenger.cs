namespace AirportTrackingSystem.Models;
public class Passenger
{


    public string Name { get; set; }
    public string Password { get; set; }
    public List<Flight> Flights { get; set; }
}
