using HouseRentingSystem.Data;
using HouseRentingSystem.Data.Models;
using HouseRentingSystem.Services.Data.Interfaces;
using HouseRentingSystem.Services.Data.Models.House;
using HouseRentingSystem.Services.Data.Models.Statistics;
using HouseRentingSystem.Web.ViewModels.Agent;
using HouseRentingSystem.Web.ViewModels.Home;
using HouseRentingSystem.Web.ViewModels.House;
using HouseRentingSystem.Web.ViewModels.House.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Services.Data
{
    public class HouseService : IHouseService
    {
        private readonly HouseRentingDbContext dbContext;

        public HouseService(HouseRentingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<AllHousesFilteredAndPagedServiceModel> AllAsync(AllHousesQueryModel queryModel)
        {
            IQueryable<House> housesQuery = this.dbContext
                .Houses
                .Where(h => h.IsActive)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryModel.Category))
            {
                housesQuery = housesQuery
                    .Where(h => h.Category.Name == queryModel.Category);
            }

            if (!string.IsNullOrWhiteSpace(queryModel.SearchString))
            {
                string wildCard = $"%{queryModel.SearchString.ToLower()}%";
                housesQuery = housesQuery
                    .Where(h => EF.Functions.Like(h.Title, wildCard) ||
                                EF.Functions.Like(h.Address, wildCard) ||
                                EF.Functions.Like(h.Description, wildCard));
            }

            housesQuery = queryModel.HouseSorting switch
            {
                HouseSorting.Newest => housesQuery
                .OrderByDescending(h => h.CreatedOn),
                HouseSorting.Oldest => housesQuery
                .OrderBy(h => h.CreatedOn),
                HouseSorting.PriceDescending => housesQuery
                .OrderByDescending(h => h.PricePerMonth),
                HouseSorting.PriceAscending => housesQuery
                .OrderBy(h => h.PricePerMonth),
                _ => housesQuery
                    .OrderBy(h => h.RenterId != null)
                    .ThenByDescending(h => h.CreatedOn)
            };

            IEnumerable<HouseAllViewModel> allHouses = await housesQuery
                .Skip((queryModel.CurrentPage - 1) * queryModel.HousesPerPage)
                .Take(queryModel.HousesPerPage)
                .Select(h => new HouseAllViewModel()
                {
                    Id = h.Id.ToString(),
                    Title = h.Title,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    PricePerMonth = h.PricePerMonth,
                    IsRented = h.RenterId.HasValue
                })
                .ToArrayAsync();
            int totalHouses = housesQuery.Count();

            return new AllHousesFilteredAndPagedServiceModel()
            {
                TotalHousesCount = totalHouses,
                Houses = allHouses
            };
        }

        public async Task<IEnumerable<HouseAllViewModel>> AllByAgentIdAsync(string agentId)
        {
            IEnumerable<HouseAllViewModel> allAgentHouses = await dbContext
                .Houses
                .Where(h => h.IsActive && h.AgentId.ToString() == agentId)
                .Select(h => new HouseAllViewModel
                {
                    Id = h.Id.ToString(),
                    Title = h.Title,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    PricePerMonth = h.PricePerMonth,
                    IsRented = h.RenterId.HasValue
                }).ToListAsync();

            return allAgentHouses;
        }

        public async Task<IEnumerable<HouseAllViewModel>> AllByUserIdAsync(string userId)
        {
            IEnumerable<HouseAllViewModel> allUserHouses = await dbContext
                .Houses
                .Where(h => h.IsActive && h.RenterId.HasValue && h.RenterId.ToString() == userId)
                .Select(h => new HouseAllViewModel
                {
                    Id = h.Id.ToString(),
                    Title = h.Title,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    PricePerMonth = h.PricePerMonth,
                    IsRented = h.RenterId.HasValue
                }).ToListAsync();

            return allUserHouses;
        }

        public async Task<string> CreateAsync(HouseFormModel model, string agentId)
        {
            House newHouse = new()
            {
                Title = model.Title,
                Address = model.Address,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                PricePerMonth = model.PricePerMonth,
                CategoryId = model.CategoryId,
                AgentId = Guid.Parse(agentId),
            };

            await this.dbContext.Houses.AddAsync(newHouse);
            await this.dbContext.SaveChangesAsync();

            return newHouse.Id.ToString();
        }

        public async Task DeleteHouseByIdAsync(string houseId)
        {
            House houseToDelete = await dbContext
                .Houses
                .Where(h => h.IsActive)
                .FirstAsync(h => h.Id.ToString() == houseId);

            houseToDelete.IsActive = false;
            await dbContext.SaveChangesAsync();
        }

        public async Task EditHouseByIdAndFormModelAsync(string houseId, HouseFormModel formModel)
        {
            House house = await dbContext
                .Houses
                .Where(h => h.IsActive)
                .FirstAsync(h => h.Id.ToString() == houseId);

            house.Title = formModel.Title;
            house.Address = formModel.Address;
            house.Description = formModel.Description;
            house.ImageUrl = formModel.ImageUrl;
            house.PricePerMonth = formModel.PricePerMonth;
            house.CategoryId = formModel.CategoryId;

            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistByIdAsync(string houseId)
        {
            bool result = await dbContext
                .Houses
                .Where(h => h.IsActive)
                .AnyAsync(h => h.Id.ToString() == houseId);

            return result;
        }

        public async Task<HouseDetailsViewModel> GetDetailsByHouseIdAsync(string houseId)
        {
            House? house = await dbContext
                .Houses
                .Include(h => h.Category)
                .Include(h => h.Agent)
                .ThenInclude(a => a.User)
                .Where(h => h.IsActive)
                .FirstAsync(h => h.Id.ToString() == houseId);


            return new HouseDetailsViewModel()
            {
                Id = house.Id.ToString(),
                Title = house.Title,
                Address = house.Address,
                ImageUrl = house.ImageUrl,
                PricePerMonth = house.PricePerMonth,
                IsRented = house.RenterId.HasValue,
                Description = house.Description,
                Category = house.Category.Name,
                Agent = new AgentInfoOnHouseViewModel()
                {
                    Email = house.Agent.User.Email,
                    PhoneNumber = house.Agent.PhoneNumber
                }
            };
        }

        public async Task<HouseDeleteDetailsViewModel> GetHouseForDeleteByIdAsync(string houseId)
        {
            House house = await dbContext.Houses
                .Where(h => h.IsActive)
                .FirstAsync(h => h.Id.ToString() == houseId);

            return new HouseDeleteDetailsViewModel()
            {
                Title = house.Title,
                Address = house.Address,
                ImageUrl = house.ImageUrl
            };
        }

        public async Task<HouseFormModel> GetHouseForEditByIdAsync(string houseId)
        {
            House? house = await dbContext
               .Houses
               .Include(h => h.Category)
               .Where(h => h.IsActive)
               .FirstAsync(h => h.Id.ToString() == houseId);

            return new HouseFormModel()
            {
                Title = house.Title,
                Address = house.Address,
                Description = house.Description,
                ImageUrl = house.ImageUrl,
                PricePerMonth = house.PricePerMonth,
                CategoryId = house.CategoryId
            };
        }

        public async Task<StatisticsServiceModel> GetStatisticsAsync()
        {
            return new StatisticsServiceModel()
            {
                TotalHouses = await dbContext.Houses.Where(h => h.IsActive).CountAsync(),
                TotalRents = await dbContext.Houses.Where(h => h.RenterId.HasValue).CountAsync()
            };
        }

        public async Task<bool> IsAgentWithIdIsOwnerOfHouseWithIdAsync(string houseId, string agentId)
        {
            House house = await dbContext.Houses
                .Where(h => h.IsActive)
                .FirstAsync(h => h.Id.ToString() == houseId);

            return house.AgentId.ToString() == agentId;
        }

        public async Task<IEnumerable<IndexViewModel>> LastThreeHousesAsync()
        {
            IEnumerable<IndexViewModel> lastThreeHouses = await this.dbContext
                .Houses
                .Where(h => h.IsActive)
                .OrderByDescending(h => h.CreatedOn)
                .Take(3)
                .Select(h => new IndexViewModel()
                {
                    Id = h.Id.ToString(),
                    Title = h.Title,
                    ImageUrl = h.ImageUrl
                })
                .ToArrayAsync();

            return lastThreeHouses;
        }
    }
}
