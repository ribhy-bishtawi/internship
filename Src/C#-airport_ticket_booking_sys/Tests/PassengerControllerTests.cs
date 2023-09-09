
using Moq;
using AirportTrackingSystem.Interfaces;
using AirportTrackingSystem.Controllers;
using AirportTrackingSystem.Models;
using AirportTrackingSystem.Enums;
namespace AirportTrackingSystem.Tests;

public class PassengerControllerTests
{

    [Fact]
    public void AddPassengersFromCsvFile_Should_Add_Passengers_From_CSV_File()
    {
        var filePath = "test.csv";

        var mockFileReader = new Mock<IFileReader>();
        var mockFileWriter = new Mock<IFileWriter>();
        var mockLogger = new Mock<ILogger>();

        mockFileReader.Setup(fr => fr.ReadAllLines(filePath)).Returns(new string[]
    {
        "John,Doe",
        "Jane,Smith"
    });


        var passengerController = new PassengerController(
            mockFileReader.Object,
            mockFileWriter.Object,
            mockLogger.Object
        );

        var result = passengerController.AddPassengersFromCsvFile(filePath);

        Assert.True(result);
    }

    [Fact]
    public void WritePassengersToCSV_Should_Call_WriteToFile_With_Correct_Parameters()
    {
        // Arrange
        var filePath = "test.csv";
        var passenger = new Passenger
        {
            Name = "Ribhi",
            Password = "Bisht"
        };

        var mockFileReader = new Mock<IFileReader>();
        var mockFileWriter = new Mock<IFileWriter>();
        var mockLogger = new Mock<ILogger>();

        var passengerController = new PassengerController(
            mockFileReader.Object,
            mockFileWriter.Object,
            mockLogger.Object
        );


        // Act
        passengerController.WritePassengersToCSV(filePath, passenger);

        // Assert
        mockFileWriter.Verify(
            fw => fw.WriteToFile(filePath, "Ribhi,Bisht"),
            Times.Once
        );
    }
    [Fact]
    public void AddPassenger_Should_Return_Success_For_Valid_Passenger()
    {
        var name = "Ribhi";
        var password = "Bish";

        var mockFileReader = new Mock<IFileReader>();
        var mockFileWriter = new Mock<IFileWriter>();
        var mockLogger = new Mock<ILogger>();


        var passengerController = new PassengerController(
            mockFileReader.Object,
            mockFileWriter.Object,
            mockLogger.Object
        );

        // Act
        var result = passengerController.AddPassenger(name, password);

        // Assert
        Assert.Equal(AccountStatus.Success, result); // Expecting success for a valid passenger
    }
    [Fact]
    public void AddPassenger_Should_Return_AlreadyRegistered_For_Existing_Passenger()
    {
        var name = "Ribhi";
        var password = "Bish";
        var filePath = "test.csv";

        var mockFileReader = new Mock<IFileReader>();
        var mockFileWriter = new Mock<IFileWriter>();
        var mockLogger = new Mock<ILogger>();

        mockFileReader.Setup(fr => fr.ReadAllLines(filePath)).Returns(new string[] { "Ribhi,Bish" });

        var passengerController = new PassengerController(
            mockFileReader.Object,
            mockFileWriter.Object,
            mockLogger.Object
        );
        passengerController.AddPassengersFromCsvFile(filePath);

        var result = passengerController.AddPassenger(name, password);

        Assert.Equal(AccountStatus.AlreadyRegistered, result);
    }
    [Fact]
    public void Login_Should_Return_True_For_Existing_User()
    {
        var name = "Ribhi";
        var password = "Bish";
        var filePath = "test.csv";

        var mockFileReader = new Mock<IFileReader>();
        var mockFileWriter = new Mock<IFileWriter>();
        var mockLogger = new Mock<ILogger>();

        mockFileReader.Setup(fr => fr.ReadAllLines(filePath)).Returns(new string[] { "Ribhi,Bish" });

        var passengerController = new PassengerController(
            mockFileReader.Object,
            mockFileWriter.Object,
            mockLogger.Object
        );
        passengerController.AddPassengersFromCsvFile(filePath);

        var result = passengerController.Login(name, password);
        Assert.True(result);
    }
    [Fact]
    public void Login_Should_Return_False_For_Not_Existing_User()
    {
        var name = "Omar";
        var password = "Bish";
        var filePath = "test.csv";

        var mockFileReader = new Mock<IFileReader>();
        var mockFileWriter = new Mock<IFileWriter>();
        var mockLogger = new Mock<ILogger>();

        mockFileReader.Setup(fr => fr.ReadAllLines(filePath)).Returns(new string[] { "Ribhi,Bish" });

        var passengerController = new PassengerController(
            mockFileReader.Object,
            mockFileWriter.Object,
            mockLogger.Object
        );
        passengerController.AddPassengersFromCsvFile(filePath);

        var result = passengerController.Login(name, password);
        Assert.False(result);
    }
    [Fact]
    public void Logout_Should_Return_False_For_User_Being_Not_Logged_In()
    {
        var name = "Ribhi";
        var password = "Bish";
        var filePath = "test.csv";

        var mockFileReader = new Mock<IFileReader>();
        var mockFileWriter = new Mock<IFileWriter>();
        var mockLogger = new Mock<ILogger>();

        mockFileReader.Setup(fr => fr.ReadAllLines(filePath)).Returns(new string[] { "Ribhi,Bish" });

        var passengerController = new PassengerController(
            mockFileReader.Object,
            mockFileWriter.Object,
            mockLogger.Object
        );
        passengerController.AddPassengersFromCsvFile(filePath);

        passengerController.Login(name, password);
        var result = passengerController.Logout();
        Assert.False(result);

    }


}

