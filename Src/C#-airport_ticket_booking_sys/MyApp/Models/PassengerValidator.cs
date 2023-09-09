using AirportTrackingSystem.Interfaces;
using System.ComponentModel.DataAnnotations;
namespace AirportTrackingSystem.Models;
public class PassengerValidator : IPassengerValidator
{
    public bool Validate(Passenger passenger, out ICollection<ValidationResult> validationResults)
    {
        validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(passenger);
        return Validator.TryValidateObject(passenger, validationContext, validationResults, validateAllProperties: true);
    }
}
