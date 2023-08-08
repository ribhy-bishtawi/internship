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

    public void AddFlightsFromCsvFile(string filePath)
    {
        int tempCount = 0;
        try
        {
            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    tempCount++;
                    var line = reader.ReadLine();
                    var fields = line.Split(',');

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
        catch (Exception ex)
        {
            Console.WriteLine("Error reading CSV file: " + ex.Message);
        }
    }

    public void ShowAllAvaliableFlights()
    {
        Console.WriteLine("Available Flights:");
        Console.WriteLine("{0,-10} {1,-15} {2,-20} {3,-15} {4,-15} {5,-15} {6,-10}", "Flight #", "Price", "Departure Date", "Dep. Country", "Dep. Airport", "Arr. Airport", "Flight Class");
        Console.WriteLine(new string('-', 85));
        int tempIndex = 1;

        foreach (var flight in flights)
        {

            Console.WriteLine("{0,-10} {1,-8:C} {2,-15} {3,-20} {4,-15} {5,-15} {6,-10} ", tempIndex++, flight.Price, flight.DepartureDate, flight.DepartureCountry, flight.DepartureAirport, flight.ArrivalAirport, flight.TripClass);
        }
    }
    public void FiltterFlightsByParameters(int? price, DateTime? depDate, string? depCountry, string? depAirport, string? arrAirport, TripClass? flightClass)
    {

        var filteredFlights = flights
    .Where(flight =>
        (price == null || flight.Price == price) &&
        (depDate == null || flight.DepartureDate == depDate) &&
        (depCountry == null || flight.DepartureCountry == depCountry) &&
        (depAirport == null || flight.DepartureAirport == depAirport) &&
        (arrAirport == null || flight.ArrivalAirport == arrAirport) &&
        (flightClass == null || flight.TripClass == flightClass)
    )
    .ToList();

        foreach (var flight in filteredFlights)
        {
            Console.WriteLine("{0,-10:C} {1,-20} {2,-15} {3,-15} {4,-15} {5,-10}", flight.Price, flight.DepartureDate, flight.DepartureCountry, flight.DepartureAirport, flight.ArrivalAirport, flight.TripClass);
        }


    }

    public Flight FindFlight(int flightNum)
    {
        return flights.ElementAt(--flightNum);
    }
}
