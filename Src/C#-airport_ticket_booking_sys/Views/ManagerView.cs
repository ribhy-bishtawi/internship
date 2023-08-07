using AirportTrackingSystem.Models;
using AirportTrackingSystem.Controllers;
using AirportTrackingSystem.Enums;

namespace AirportTrackingSystem.Views;

public class ManagerView
{
    private FlightController flightController = new FlightController();

    public void ShowMainScreen()
    {
        InitUI();
        Console.WriteLine("Please select an option:");
        Console.WriteLine("1. Display all booked flights");
        Console.WriteLine("2. Import flights from a CSV file");
        Console.Write("Enter your choice (1 or 2): ");
        int choice = Convert.ToInt32(Console.ReadLine());
        switch (choice)
        {
            case 1:
                ShowBookedFlights();
                break;
            case 2:
                AddFlights();
                break;

            default:
                Console.WriteLine("Invalid choice. Please choose a valid option.");
                break;
        }


    }
    public void AddFlights()
    {
        InitUI();
        Console.WriteLine("Please enter you file's path");
        string filePath = Console.ReadLine();
        flightController.AddFlightsFromCsvFile(filePath);
        ShowAllAvaliableFlights();


    }
    public void ShowBookedFlights()
    {
        ;
    }

    public void ShowAllAvaliableFlights()
    {
        flightController.ShowAllAvaliableFlights();
    }
    public void InitUI()
    {
        Console.ResetColor();
        Console.Clear();
    }


}
