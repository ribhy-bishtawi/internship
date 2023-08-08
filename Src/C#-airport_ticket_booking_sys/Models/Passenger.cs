using System.ComponentModel.DataAnnotations;

namespace AirportTrackingSystem.Models;
public class Passenger
{

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }
    public List<Flight> Flights { get; set; }
}
