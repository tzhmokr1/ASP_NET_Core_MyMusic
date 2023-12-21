using FluentValidation;
using MyMusic.API.Resources;

namespace MyMusic.API.Validations
{
    public class SaveArtistResourceValidator : AbstractValidator<SaveArtistResource>
    {
        public SaveArtistResourceValidator()
        {
            RuleFor(a => a.Name)
            .NotEmpty()
            .MaximumLength(50);
        }
    }
}
