using FluentValidation;

namespace AirlineService.Application.Flights.Commands;

/// <summary>
/// Validator for UpdateFlightStatusCommand.
/// </summary>
public class UpdateFlightStatusCommandValidator : AbstractValidator<UpdateFlightStatusCommand>
{
    public UpdateFlightStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Flight ID must be greater than 0.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid flight status.");
    }
}

