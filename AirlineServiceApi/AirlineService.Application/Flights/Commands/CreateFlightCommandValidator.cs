using FluentValidation;

namespace AirlineService.Application.Flights.Commands;

/// <summary>
/// Validator for CreateFlightCommand.
/// </summary>
public class CreateFlightCommandValidator : AbstractValidator<CreateFlightCommand>
{
    public CreateFlightCommandValidator()
    {
        RuleFor(x => x.Origin)
            .NotEmpty().WithMessage("Origin is required.")
            .MaximumLength(256).WithMessage("Origin must not exceed 256 characters.");

        RuleFor(x => x.Destination)
            .NotEmpty().WithMessage("Destination is required.")
            .MaximumLength(256).WithMessage("Destination must not exceed 256 characters.");

        RuleFor(x => x.Departure)
            .NotEmpty().WithMessage("Departure time is required.");

        RuleFor(x => x.Arrival)
            .NotEmpty().WithMessage("Arrival time is required.")
            .GreaterThan(x => x.Departure).WithMessage("Arrival time must be after departure time.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid flight status.");
    }
}

