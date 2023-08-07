using AirportTrackingSystem.Enums;
namespace AirportTrackingSystem.Models;
public class Flight
{


    public int Price { get; set; }
    public string DepartureCountry { get; set; }
    public DateTime DepartureDate { get; set; }
    public string DepartureAirport { get; set; }
    public string ArrivalAirport { get; set; }
    public TripClass TripClass { get; set; }

}
