using HouseRentingSystem.Services.Data.Models.House;
using HouseRentingSystem.Services.Data.Models.Statistics;
using HouseRentingSystem.Web.ViewModels.Home;
using HouseRentingSystem.Web.ViewModels.House;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Services.Data.Interfaces
{
    public interface IHouseService
    {
        Task<IEnumerable<IndexViewModel>> LastThreeHousesAsync();

        Task<string> CreateAsync(HouseFormModel model, string agentId);

        Task<AllHousesFilteredAndPagedServiceModel> AllAsync(AllHousesQueryModel queryModel);

        Task<IEnumerable<HouseAllViewModel>> AllByAgentIdAsync(string agentId);

        Task<IEnumerable<HouseAllViewModel>> AllByUserIdAsync(string userId);

        Task<HouseDetailsViewModel> GetDetailsByHouseIdAsync(string houseId);

        Task<bool> ExistByIdAsync(string houseId);

        Task<HouseFormModel> GetHouseForEditByIdAsync(string houseId);

        Task<bool> IsAgentWithIdIsOwnerOfHouseWithIdAsync(string houseId, string agentId);

        Task EditHouseByIdAndFormModelAsync(string houseId, HouseFormModel formModel);

        Task<HouseDeleteDetailsViewModel> GetHouseForDeleteByIdAsync(string houseId);

        Task DeleteHouseByIdAsync(string houseId);

        Task<StatisticsServiceModel> GetStatisticsAsync();

        Task<bool> IsRentedByUserWithIdAsync(string houseId, string? userId);
    }
}
