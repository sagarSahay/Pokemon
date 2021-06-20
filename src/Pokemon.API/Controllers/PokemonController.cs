using Microsoft.AspNetCore.Mvc;

namespace Pokemon.API.Controllers
{
    using System.Threading.Tasks;
    using Models;

    public class PokemonController : Controller
    {
        private readonly IPokemonService _pokemon;

        public PokemonController(IPokemonService pokemon)
        {
            _pokemon = pokemon;
        }
        
        [HttpGet]
        
        public async Task<IActionResult> BasicInformation(PokemonRequest request)
        {
            var result = await _pokemon.GetBasicInformation(request.Name);
            return new OkObjectResult(result);
        }

        [HttpGet]
        [Route("/translated/{pokemonName}")]
        public async Task<IActionResult> TranslatedInformation(PokemonRequest request)
        {
            var result = await _pokemon.GetTranslatedInformation(request.Name);
            return new OkObjectResult(result);
        }
    }
}