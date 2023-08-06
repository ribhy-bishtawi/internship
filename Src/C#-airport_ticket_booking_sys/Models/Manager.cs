namespace AirportTrackingSystem.Models;
class Manager
{
    public string UserName { get; set; }
    public string Password { get; set; }

    public List<Passenger> Passengers { get; set; } = new List<Passenger>();


}
