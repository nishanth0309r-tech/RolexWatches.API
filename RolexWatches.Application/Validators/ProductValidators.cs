using FluentValidation;
using RolexWatches.Application.Dto;

namespace RolexWatches.Application.Validators
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Sku).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.DiscountPrice)
                .LessThan(x => x.Price)
                .When(x => x.DiscountPrice.HasValue)
                .WithMessage("Discount price must be less than the regular price.");
            RuleFor(x => x.StockQuantity).GreaterThanOrEqualTo(0);
            RuleFor(x => x.BrandId).GreaterThan(0);
            RuleFor(x => x.CategoryId).GreaterThan(0);
        }
    }

    public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidator()
        {
            Include(new CreateProductDtoValidator());
        }
    }
}