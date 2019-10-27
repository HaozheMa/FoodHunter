using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using FoodHunter.Models;

namespace FoodHunter.Controllers
{
    public class RestaurantsValidator: AbstractValidator<Restaurants>
    {
        public RestaurantsValidator()
        {
            RuleFor(r => r.BookStatus).NotEqual("yes");
        }
    }
}