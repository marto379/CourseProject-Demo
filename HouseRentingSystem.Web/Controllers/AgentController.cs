namespace HouseRentingSystem.Web.Controllers
{
    using HouseRentingSystem.Services.Data.Interfaces;
    using HouseRentingSystem.Web.Infrastructure.Extensions;
    using HouseRentingSystem.Web.ViewModels.Agent;
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
            bool isAgent = await this.agentService.AgentExistByUserIdAsync(userId);
            if (isAgent)
            {
                TempData[ErrorMessage] = "You are alredy an agent!";

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Become(BecomeAgentFormModel model)
        {
            string userId = User.GetId();
            bool isAgent = await this.agentService.AgentExistByUserIdAsync(userId);
            if (isAgent)
            {
                TempData[ErrorMessage] = "You are alredy an agent!";

                return RedirectToAction("Index", "Home");
            }

            bool isPhoneNumberTaken = await this.agentService.AgentExistByPhoneNumberAsync(model.PhoneNumber);

            if (isPhoneNumberTaken)
            {
                ModelState.AddModelError(nameof(model.PhoneNumber), "Agent with provided phone number alredy exist!");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            bool userHasActiveRents = await this.agentService.UserHasRentsByUserIdAsync(userId);

            if (userHasActiveRents)
            {
                this.TempData[ErrorMessage] = "You must not have any active rents to become an agent";

                return RedirectToAction("Mine", "House");
            }
            try
            {
                await this.agentService.Create(userId, model);

            }
            catch (Exception)
            {

                this.TempData[ErrorMessage] = "Unexpected error occured while registering you as agent! Please try agent later or contact administrator!";

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("All", "House");
        }
    }
}
