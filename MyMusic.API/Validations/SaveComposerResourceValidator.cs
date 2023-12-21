using FluentValidation;
using MyMusic.API.Resources;

namespace MyMusic.API.Validations
{
    public class SaveComposerResourceValidator : AbstractValidator<SaveComposerResource>
    {
        public SaveComposerResourceValidator()
        {
            RuleFor(m => m.FirstName)
           .NotEmpty()
           .MaximumLength(50);

            RuleFor(m => m.LastName)
                .NotEmpty()
               .MaximumLength(50);
        }
    }
}
