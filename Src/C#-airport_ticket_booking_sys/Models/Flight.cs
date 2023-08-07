using AirportTrackingSystem.Enums;
using System;
namespace AirportTrackingSystem.Models;
using System.ComponentModel.DataAnnotations;

public class Flight
{
    [Required(ErrorMessage = "Price is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public int? Price { get; set; }

    [Required(ErrorMessage = "Departure country is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Departure country must be between 2 and 50 characters.")]
    public string? DepartureCountry { get; set; }

    [Required(ErrorMessage = "Departure airport is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Departure airport must be between 2 and 50 characters.")]
    public string? DepartureAirport { get; set; }

    [Required(ErrorMessage = "Arrival airport is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Arrival airport must be between 2 and 50 characters.")]
    public string? ArrivalAirport { get; set; }

    [Required(ErrorMessage = "Departure date is required.")]
    [DataType(DataType.DateTime, ErrorMessage = "Invalid date format.")]
    public DateTime? DepartureDate { get; set; }

    [Required(ErrorMessage = "Flight class is required. Valid options are Economy, Business, or FirstClass.")]
    public TripClass? TripClass { get; set; }

}
