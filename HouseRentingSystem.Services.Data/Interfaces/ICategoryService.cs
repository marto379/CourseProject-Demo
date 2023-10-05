namespace HouseRentingSystem.Services.Data.Interfaces
{
    using HouseRentingSystem.Web.ViewModels.Category;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ICategoryService
    {
        Task<IEnumerable<HouseCategoryFormModel>> AllCategoriesAsync();

        Task<bool> ExistById(int id);
    }
}
