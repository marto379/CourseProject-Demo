namespace HouseRentingSystem.Services.Data.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IAgentService
    {
        Task<bool> AgentExistByUserId(string userId);
    }
}
