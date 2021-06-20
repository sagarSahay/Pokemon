using Microsoft.AspNetCore.Mvc;

namespace Pokemon.API.Controllers
{
    using System.Threading.Tasks;
    using Models;

    public class PokemonController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> BasicInformation(PokemonRequest request)
        {
            return new OkObjectResult(new PokemonResponse() {Name = "metwo"});
        }
    }
}