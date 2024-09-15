using AirportTrackingSystem.Models;
using AirportTrackingSystem.Controllers;
using AirportTrackingSystem.Enums;
using AirportTrackingSystem.Views;
using System;

namespace C__airport_ticket_booking_sys;

class Program
{
    static void Main(string[] args)
    {
        var fileReader = new FileReader();
        var fileWriter = new FileWriter();
        var loger = new ConsoleLogger();

        PassengerController passengerController = new PassengerController(fileReader, fileWriter, loger);
        FlightController flightController = new FlightController();

        PassengerView passengerView = new PassengerView(passengerController, flightController);
        ManagerView managerView = new ManagerView(passengerController, flightController);
        bool exitApplication = false;

        passengerController.AddPassengersFromCsvFile("Data/Passengers.csv");
        flightController.AddFlightsFromCsvFile("Data/Flights.csv");
        do
        {
            Console.ResetColor();
            Console.Clear();
            Console.WriteLine("Welcome to the Airport Ticket Booking Application");
            Console.WriteLine("===============================================");
            Console.WriteLine("Please choose the role you want to log in as:");
            Console.WriteLine("1. Passenger");
            Console.WriteLine("2. Manager");
            Console.WriteLine("3. Exit");
            Console.Write("Enter your choice (1, 2, or 3): ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    exitApplication = passengerView.ShowMainScreen();
                    break;
                case "2":
                    exitApplication = managerView.ShowMainScreen();
                    break;
                case "3":
                    exitApplication = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please choose a valid option.");
                    break;
            }

        } while (!exitApplication);
    }
}

