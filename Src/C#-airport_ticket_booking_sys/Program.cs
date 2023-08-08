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
        PassengerController passengerController = new PassengerController();
        FlightController flightController = new FlightController();

        PassengerView passengerView = new PassengerView(passengerController, flightController);
        ManagerView managerView = new ManagerView(passengerController, flightController);

        Console.WriteLine("Welcome to the Airport Ticket Booking Application");
        Console.WriteLine("===============================================");
        Console.WriteLine("Please choose the role you want to log in as:");
        Console.WriteLine("1. Passenger");
        Console.WriteLine("2. Manager");
        Console.Write("Enter your choice (1 or 2): ");
        int choice = Convert.ToInt32(Console.ReadLine());
        switch (choice)
        {
            case 1:
                passengerView.ShowMainScreen();
                break;
            case 2:
                managerView.ShowMainScreen();
                break;
            default:
                Console.WriteLine("Invalid choice. Please choose a valid option.");
                break;
        }
    }
}

