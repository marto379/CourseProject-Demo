namespace HouseRentingSystem.Web.Controllers
{
    using HouseRentingSystem.Services.Data.Interfaces;
    using HouseRentingSystem.Services.Data.Models.House;
    using HouseRentingSystem.Web.Infrastructure.Extensions;
    using HouseRentingSystem.Web.ViewModels.House;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using static Common.NotificationMessagesConstants;
    [Authorize]
    public class HouseController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IAgentService agentService;
        private readonly IHouseService houseService;
        public HouseController(ICategoryService categoryService, IAgentService agentService, IHouseService houseService)
        {
            this.categoryService = categoryService;
            this.agentService = agentService;
            this.houseService = houseService;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery]AllHousesQueryModel queryModel)
        {
            AllHousesFilteredAndPagedServiceModel serviceModel =
                await this.houseService.AllAsync(queryModel);

            queryModel.Houses = serviceModel.Houses;
            queryModel.TotalHouses = serviceModel.TotalHousesCount;
            queryModel.Categories = await categoryService.AllCategoryNamesAsync();

            return View(queryModel);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            bool isAgent = await agentService.AgentExistByUserIdAsync(this.User.GetId()); 
            if (!isAgent)
            {
                TempData[ErrorMessage] = "You must become an agent in order to add new houses!";
                return RedirectToAction("Become", "Agent");
            }

            HouseFormModel formModel = new()
            {
                Categories = await categoryService.AllCategoriesAsync()
            };

            return View(formModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(HouseFormModel model)
        {
            bool isAgent = await agentService.AgentExistByUserIdAsync(this.User.GetId());
            if (!isAgent)
            {
                TempData[ErrorMessage] = "You must become an agent in order to add new houses!";
                return RedirectToAction("Become", "Agent");
            }

            bool categoryExist = await categoryService.ExistById(model.CategoryId);
            if (!categoryExist)
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Selected category does not exist!");
            }
            if (!ModelState.IsValid)
            {
                model.Categories = await categoryService.AllCategoriesAsync();

                return View(model);
            }

            try
            {
                string agentId = await this.agentService.GetAgentIdByUserIdAsync(this.User.GetId());
                await this.houseService.CreateAsync(model, agentId);
            }
            catch (Exception _)
            {
                ModelState.AddModelError(string.Empty, "Unexpected error occured while trying to add your new house! Please try again later or contact your administrator!");
                model.Categories = await categoryService.AllCategoriesAsync();
                return View(model);
            }

            return RedirectToAction("All", "House"); 
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            HouseDetailsViewModel? viewModel = await houseService
                .GetDetailsByHouseIdAsync(id);

            if (viewModel == null)
            {
                this.TempData[ErrorMessage] = "House with the provided id does not exist!";
                return RedirectToAction("All", "House");
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            List<HouseAllViewModel> myHouses = new List<HouseAllViewModel>();

            string userId = this.User.GetId();
            bool isUserAgent = await agentService.AgentExistByUserIdAsync(userId);

            if (isUserAgent)
            {
                string agentId = await agentService.GetAgentIdByUserIdAsync(userId);

                myHouses.AddRange(await houseService.AllByAgentIdAsync(agentId));
            }
            else
            {
                myHouses.AddRange(await this.houseService.AllByUserIdAsync(userId));
            }
            return View(myHouses);
        }
    }
}
