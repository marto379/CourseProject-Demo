namespace HouseRentingSystem.Services.Data
{
    using HouseRentingSystem.Data;
    using HouseRentingSystem.Services.Data.Interfaces;
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

        public async Task<bool> AgentExistByUserId(string userId)
        {
            bool result = await dbContext
                 .Agents
                 .AnyAsync(a => a.UserId.ToString() == userId);

            return result;
        }
    }
}
