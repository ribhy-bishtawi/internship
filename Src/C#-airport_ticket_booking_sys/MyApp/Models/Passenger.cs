using System.ComponentModel.DataAnnotations;

namespace AirportTrackingSystem.Models;
public class Passenger
{

    [Required(ErrorMessage = "Name is required.")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(5, MinimumLength = 2, ErrorMessage = "Password  must be between 2 and 5 characters.")]
    public string? Password { get; set; }
    public List<Flight>? Flights { get; set; }
}
