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

    public bool AddFlightsFromCsvFile(string? filePath)
    {
        int tempCount = 0;
        // TODO Possible null reference argument for parameter 'path'
        // Ask if there's another way to handle null
        if (filePath != null)
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        tempCount++;
                        var line = reader.ReadLine();
                        var fields = line?.Split(',');
                        // TODO same question as above 
                        if (fields != null)
                        {

                            if (fields.Length >= 6) // Check if there are enough fields
                            {
                                var flight = new Flight
                                {
                                    Price = int.Parse(fields[0]),
                                    DepartureCountry = fields[1],
                                    DepartureDate = DateTime.TryParse(fields[2], out DateTime departureDate) ? departureDate : (DateTime?)null,
                                    DepartureAirport = fields[3],
                                    ArrivalAirport = fields[4],
                                    TripClass = Enum.TryParse(fields[5], out TripClass flightClass) ? flightClass : (TripClass?)null
                                };
                                var validationContext = new ValidationContext(flight);
                                var validationResults = new List<ValidationResult>();

                                if (!Validator.TryValidateObject(flight, validationContext, validationResults, validateAllProperties: true))
                                {
                                    Console.WriteLine($"The following validation errors were detected in the values entered on line {tempCount}:");
                                    foreach (var validationResult in validationResults)
                                    {
                                        Console.WriteLine($"\t{validationResult.ErrorMessage}");
                                    }
                                    continue;
                                }

                                flights.Add(flight);

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
        Console.WriteLine("Please enter a vail file path");
        return false;
    }

    public List<Flight> ShowAllAvaliableFlights()
    {
        return flights;
    }
    public List<Flight>? FiltterFlightsByParameters(int? price, DateTime? depDate, string? depCountry, string? depAirport, string? arrAirport, TripClass? flightClass, Passenger? passenger = null)
    {
        List<Flight>? tempFlights = passenger != null ? passenger.Flights : flights;
        var filteredFlights = tempFlights?
    .Where(flight =>
        (price == null || flight.Price == price) &&
        (depDate == null || flight.DepartureDate == depDate) &&
        (depCountry == null || flight.DepartureCountry == depCountry) &&
        (depAirport == null || flight.DepartureAirport == depAirport) &&
        (arrAirport == null || flight.ArrivalAirport == arrAirport) &&
        (flightClass == null || flight.TripClass == flightClass)
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

