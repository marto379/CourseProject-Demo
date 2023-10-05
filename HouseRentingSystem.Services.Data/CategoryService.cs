namespace HouseRentingSystem.Services.Data
{
    using HouseRentingSystem.Data;
    using HouseRentingSystem.Services.Data.Interfaces;
    using HouseRentingSystem.Web.ViewModels.Category;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CategoryService : ICategoryService
    {
        private readonly HouseRentingDbContext dbContext;
        public CategoryService(HouseRentingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<HouseCategoryFormModel>> AllCategoriesAsync()
        {
            IEnumerable<HouseCategoryFormModel> allCategories = await dbContext
                .Categories
                .AsNoTracking()
                .Select(c => new HouseCategoryFormModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToArrayAsync();

            return allCategories;
        }

        public async Task<bool> ExistById(int id)
        {
            bool result = await dbContext
                .Categories
                .AnyAsync(c => c.Id == id);

            return result;
        }
    }
}
