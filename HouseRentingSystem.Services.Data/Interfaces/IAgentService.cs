namespace HouseRentingSystem.Services.Data.Interfaces
{
    using HouseRentingSystem.Web.ViewModels.Agent;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IAgentService
    {
        Task<bool> AgentExistByUserIdAsync(string userId);

        Task<bool> AgentExistByPhoneNumberAsync(string phoneNumber);

        Task<bool> UserHasRentsByUserIdAsync(string userId);

        Task Create(string userId, BecomeAgentFormModel model);

        Task<string> GetAgentIdByUserIdAsync(string userId);
    }
}
