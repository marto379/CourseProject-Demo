using HouseRentingSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HouseRentingSystem.Data.Configurations
{
    public class CategotyEntityConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(GenerateCategories());
        }

        private Category[] GenerateCategories()
        {
            var categories = new List<Category>();

            Category category;
            category = new()
            {
                Id = 1,
                Name = "Cottage"
            };

            categories.Add(category);

           
            category = new()
            {
                Id = 2,
                Name = "Singe-Family"
            };

            categories.Add(category);

            category = new()
            {
                Id = 3,
                Name = "Duplex"
            };

            categories.Add(category);

            return categories.ToArray();
        }
    }
}
