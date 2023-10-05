namespace HouseRentingSystem.Services.Data
{
    using HouseRentingSystem.Data;
    using HouseRentingSystem.Data.Models;
    using HouseRentingSystem.Services.Data.Interfaces;
    using HouseRentingSystem.Web.ViewModels.Agent;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AgentService : IAgentService
    {
        private readonly HouseRentingDbContext dbContext;
        public AgentService(HouseRentingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> AgentExistByPhoneNumberAsync(string phoneNumber)
        {
            bool result = await dbContext
                 .Agents
                 .AnyAsync(a => a.PhoneNumber == phoneNumber);

            return result;
        }

        public async Task<bool> AgentExistByUserIdAsync(string userId)
        {
            bool result = await dbContext
                 .Agents
                 .AnyAsync(a => a.UserId.ToString() == userId);

            return result;
        }

        public async Task Create(string userId, BecomeAgentFormModel model)
        {
            Agent agent = new Agent()
            {
                PhoneNumber = model.PhoneNumber,
                UserId = Guid.Parse(userId)
            };

            await this.dbContext.Agents.AddAsync(agent);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<string> GetAgentIdByUserIdAsync(string userId)
        {
            Agent? agent = await dbContext
                .Agents
                .FirstOrDefaultAsync(a => a.UserId.ToString() == userId);

            if (agent is null)
            {
                return null;
            }

            return agent.Id.ToString(); 
        }

        public async Task<bool> UserHasRentsByUserIdAsync(string userId)
        {
            ApplicationUser? user = await dbContext
                .Users
                .FirstOrDefaultAsync(u => u.Id.ToString() == userId);

            if (user == null)
            {
                return false;
            }

            return user.RentedHouses.Any();
        }
    }
}
