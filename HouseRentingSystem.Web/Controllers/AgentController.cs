using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Web.Controllers
{
    public class AgentController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Become()
        {
            return View();
        }
    }
}
