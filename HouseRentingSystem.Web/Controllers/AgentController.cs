﻿namespace HouseRentingSystem.Web.Controllers
{
    using HouseRentingSystem.Services.Data.Interfaces;
    using HouseRentingSystem.Web.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using static HouseRentingSystem.Common.NotificationMessagesConstants;

    [Authorize]
    public class AgentController : Controller
    {
        private readonly IAgentService agentService;
        public AgentController(IAgentService agentService)
        {
            this.agentService = agentService;
        }

        [HttpGet]
        public async Task<IActionResult> Become()
        {
            string userId = User.GetId();
            bool isAgent = await this.agentService.AgentExistByUserId(userId);
            if (isAgent)
            {
                TempData[ErrorMessage] = "You are alredy an agent!";

                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}
