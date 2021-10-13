using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.Products
{
    public class ProductCreateRequestValidator : AbstractValidator<ProductCreateRequest>
    {
        public ProductCreateRequestValidator()
        {
            //Cac validator co san
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");

            RuleFor(x => x.OriginalPrice).NotEmpty().WithMessage("OriginalPrice is required");

            RuleFor(x => x.Stock).NotEmpty().WithMessage("Stock is required");
        }
    }
}