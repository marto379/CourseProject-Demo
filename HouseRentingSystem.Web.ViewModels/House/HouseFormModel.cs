namespace HouseRentingSystem.Web.ViewModels.House
{
    using HouseRentingSystem.Web.ViewModels.Category;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using static Common.EntityValidationConstants.House;

    public class HouseFormModel
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(AddressMaxLength,MinimumLength =AddressMinLength)]
        public string Address { get; set; } = null!;

        [Required]
        [StringLength(DescriptionMaxLength,MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Required]
        [StringLength(ImageUrlMaxLength)]
        [Display(Name = "Image Link")]
        public string ImageUrl { get; set; } = null!;

        [Range(typeof(decimal), PricePerMonthMinValue, PricePerMonthMaxValue)]
        [Display(Name = "Monthly price")]
        public decimal PricePerMonth { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public IEnumerable<HouseCategoryFormModel> Categories { get; set; } = new List<HouseCategoryFormModel>();
    }
}
