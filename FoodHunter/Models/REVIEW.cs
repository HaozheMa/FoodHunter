//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FoodHunter.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class REVIEW
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public string Review1 { get; set; }
        public decimal Rating { get; set; }
        public string CustomerId { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual Restaurants Restaurants { get; set; }
    }
}