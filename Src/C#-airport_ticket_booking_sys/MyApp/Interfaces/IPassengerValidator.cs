using AirportTrackingSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace AirportTrackingSystem.Interfaces;
public interface IPassengerValidator
{
    bool Validate(Passenger passenger, out ICollection<ValidationResult> validationResults);
}