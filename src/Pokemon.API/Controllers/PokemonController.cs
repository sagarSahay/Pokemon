using Microsoft.AspNetCore.Mvc;

namespace Pokemon.API.Controllers
{
    using System.Threading.Tasks;

    public class PokemonController : Controller
    {
        private readonly IPokemonService _pokemon;

        public PokemonController(IPokemonService pokemon)
        {
            _pokemon = pokemon;
        }
        
        [HttpGet]
        [Route("/{pokemonName}")]
        public async Task<IActionResult> BasicInformation(string pokemonName)
        {
            if (string.IsNullOrWhiteSpace(pokemonName))
            {
                return new BadRequestResult();
            }
            var result = await _pokemon.GetBasicInformation(pokemonName);
            return new OkObjectResult(result);
        }

        [HttpGet]
        [Route("/translated/{pokemonName}")]
        public async Task<IActionResult> TranslatedInformation(string pokemonName)
        {
            if (string.IsNullOrWhiteSpace(pokemonName))
            {
                return new BadRequestResult();
            }
            var result = await _pokemon.GetTranslatedInformation(pokemonName);
            return new OkObjectResult(result);
        }
    }
}