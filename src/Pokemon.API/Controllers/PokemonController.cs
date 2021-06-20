using Microsoft.AspNetCore.Mvc;

namespace Pokemon.API.Controllers
{
    using System.Threading.Tasks;
    using Models;

    public class PokemonController : Controller
    {
        private readonly IPokemon _pokemon;

        public PokemonController(IPokemon pokemon)
        {
            _pokemon = pokemon;
        }
        [HttpGet]
        public async Task<IActionResult> BasicInformation(PokemonRequest request)
        {
            var result = await _pokemon.GetBasicInformation(request.Name);
            return new OkObjectResult(result);
        }
    }
}