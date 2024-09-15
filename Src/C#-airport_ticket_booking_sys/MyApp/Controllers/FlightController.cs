using AirportTrackingSystem.Models;
using AirportTrackingSystem.Enums;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AirportTrackingSystem.Controllers;

public class FlightController
{
    private List<Flight> flights = new List<Flight>();
    public bool AddFlightsFromCsvFile(string filePath)
    {
        try
        {
            using (var reader = new StreamReader(filePath))
            {
                int lineNum = 0;
                while (!reader.EndOfStream)
                {
                    lineNum++;
                    var line = reader.ReadLine();
                    var fields = line?.Split(',');

                    if (fields?.Length >= 6)
                    {
                        if (TryCreateFlightFromFields(fields, out Flight? flight, lineNum))
                        {
                            flights.Add(flight!);
                        }
                    }
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading CSV file: " + ex.Message);
            return false;
        }
    }

    private bool TryCreateFlightFromFields(string[] fields, out Flight? flight, int lineNum)
    {
        flight = null;

        if (!int.TryParse(fields[0], out int price))
        {
            Console.WriteLine($"Invalid price on line {lineNum}");
            return false;
        }

        DateTime? departureDate = DateTime.TryParse(fields[2], out DateTime parsedDepartureDate)
            ? parsedDepartureDate
            : (DateTime?)null;

        if (!Enum.TryParse(fields[5], out TripClass flightClass))
        {
            Console.WriteLine($"Invalid flight class on line {lineNum}");
            return false;
        }

        flight = new Flight
        {
            Price = price,
            DepartureCountry = fields[1],
            DepartureDate = departureDate,
            DepartureAirport = fields[3],
            ArrivalAirport = fields[4],
            TripClass = flightClass
        };

        var validationContext = new ValidationContext(flight);
        var validationResults = new List<ValidationResult>();

        if (!Validator.TryValidateObject(flight, validationContext, validationResults, validateAllProperties: true))
        {
            Console.WriteLine($"The following validation errors were detected in the values entered on line {lineNum}:");
            foreach (var validationResult in validationResults)
            {
                Console.WriteLine($"\t{validationResult.ErrorMessage}");
            }
            return false;
        }

        return true;
    }

    public List<Flight> ShowAllAvaliableFlights()
    {
        return flights;
    }
    public List<Flight>? FiltterFlightsByParameters(Flight searchFlight, Passenger? passenger = null)
    {
        List<Flight>? tempFlights = passenger != null ? passenger.Flights : flights;
        var filteredFlights = tempFlights?
    .Where(flight =>
        (searchFlight.Price == null || flight.Price == searchFlight.Price) &&
        (searchFlight.DepartureDate == null || flight.DepartureDate == searchFlight.DepartureDate) &&
        (searchFlight.DepartureCountry == null || flight.DepartureCountry == searchFlight.DepartureCountry) &&
        (searchFlight.DepartureAirport == null || flight.DepartureAirport == searchFlight.DepartureAirport) &&
        (searchFlight.ArrivalAirport == null || flight.ArrivalAirport == searchFlight.ArrivalAirport) &&
        (searchFlight.TripClass == null || flight.TripClass == searchFlight.TripClass)
    )
    .ToList();
        return filteredFlights;
    }

    public Flight FindFlight(int flightNum)
    {
        return flights.ElementAt(--flightNum);
    }

    public bool CancelFlight(List<Flight>? bookedFlights, int flightNum)
    {
        try
        {
            bookedFlights!.RemoveAt(--flightNum);
            return true;
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return false;

        }
    }

    public void ChangeClass(Flight? bookedFlight, TripClass newTripClass)
    {
        int? multiplier = newTripClass - bookedFlight?.TripClass;
        if (bookedFlight != null)
        {
            bookedFlight.TripClass = newTripClass;
            bookedFlight.Price += multiplier * 50;
        }
    }

}

