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
        public async Task<IActionResult> All([FromQuery] AllHousesQueryModel queryModel)
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
            try
            {
                HouseFormModel formModel = new()
                {
                    Categories = await categoryService.AllCategoriesAsync()
                };

                return View(formModel);
            }
            catch (Exception)
            {
                return GeneralError();
            }

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
                string houseId = await this.houseService.CreateAsync(model, agentId);

                return RedirectToAction("Details", "House", new { id = houseId });
            }
            catch (Exception _)
            {
                ModelState.AddModelError(string.Empty, "Unexpected error occured while trying to add your new house! Please try again later or contact your administrator!");
                model.Categories = await categoryService.AllCategoriesAsync();
                return View(model);
            }
                      
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            bool houseExist = await houseService.ExistByIdAsync(id);

            if (!houseExist)
            {
                this.TempData[ErrorMessage] = "House with the provided id does not exist!";
                return RedirectToAction("All", "House");
            }
            HouseDetailsViewModel viewModel = await houseService
                .GetDetailsByHouseIdAsync(id);

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            List<HouseAllViewModel> myHouses = new List<HouseAllViewModel>();

            string userId = this.User.GetId();
            bool isUserAgent = await agentService.AgentExistByUserIdAsync(userId);
            try
            {
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
            catch (Exception)
            {

                return GeneralError();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            bool houseExist = await houseService.ExistByIdAsync(id);

            if (!houseExist)
            {
                this.TempData[ErrorMessage] = "House with the provided id does not exist!";
                return RedirectToAction("All", "House");
            }

            bool isUserAgent = await agentService.AgentExistByUserIdAsync(User.GetId());

            if (!isUserAgent)
            {
                this.TempData[ErrorMessage] = "You must become an agent in order to edit house info!";

                return RedirectToAction("Become", "Agent");
            }

            string agentId = await agentService.GetAgentIdByUserIdAsync(User.GetId());

            bool isAgentOwner = await houseService.IsAgentWithIdIsOwnerOfHouseWithIdAsync(id, agentId);

            if (!isAgentOwner)
            {
                this.TempData[ErrorMessage] = "You must be the agent owner of the house you want to edit!";

                return RedirectToAction("Mine", "House");
            }
            try
            {
                HouseFormModel formModel = await houseService.GetHouseForEditByIdAsync(id);

                formModel.Categories = await categoryService.AllCategoriesAsync();

                return View(formModel);
            }
            catch (Exception)
            {
                return GeneralError();
            }

        }


        [HttpPost]
        public async Task<IActionResult> Edit(string id, HouseFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await categoryService.AllCategoriesAsync();
                return View(model);
            }

            bool houseExist = await houseService.ExistByIdAsync(id);

            if (!houseExist)
            {
                this.TempData[ErrorMessage] = "House with the provided id does not exist!";
                return RedirectToAction("All", "House");
            }

            bool isUserAgent = await agentService.AgentExistByUserIdAsync(User.GetId());

            if (!isUserAgent)
            {
                this.TempData[ErrorMessage] = "You must become an agent in order to edit house info!";

                return RedirectToAction("Become", "Agent");
            }

            string agentId = await agentService.GetAgentIdByUserIdAsync(User.GetId());

            bool isAgentOwner = await houseService.IsAgentWithIdIsOwnerOfHouseWithIdAsync(id, agentId);

            if (!isAgentOwner)
            {
                this.TempData[ErrorMessage] = "You must be the agent owner of the house you want to edit!";

                return RedirectToAction("Mine", "House");
            }

            try
            {
                await houseService.EditHouseByIdAndFormModelAsync(id, model);
            }
            catch (Exception)
            {

                ModelState.AddModelError(string.Empty, "Unexpected error accured while trying to update the house! Please try againn later or contact the administrator");

                model.Categories = await categoryService.AllCategoriesAsync();
                return View(model);
            }

            return RedirectToAction("Details", "House", new { id });
        }

        private IActionResult GeneralError()
        {
            TempData[ErrorMessage] = "Unexpected error occured! Please try again leter or contact administrator!";

            return RedirectToAction("Index", "Home");
        }
    }
}
