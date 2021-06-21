using Microsoft.AspNetCore.Mvc;

namespace Pokemon.API.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    
    public class PokemonController : Controller
    {
        private readonly IPokemonService _pokemon;

        public PokemonController(IPokemonService pokemon)
        {
            _pokemon = pokemon;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("/pokemon/{pokemonName}")]
        public async Task<IActionResult> BasicInformation(string pokemonName)
        {
            if (string.IsNullOrWhiteSpace(pokemonName))
            {
                return BadRequest("Pokemon Name cannot be empty");
            }
            var result = await _pokemon.GetBasicInformation(pokemonName);

            if (string.IsNullOrWhiteSpace(result.Name))
            {
                return NotFound();
            }
            
            return new OkObjectResult(result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("/pokemon/translated/{pokemonName}")]
        public async Task<IActionResult> TranslatedInformation(string pokemonName)
        {
            if (string.IsNullOrWhiteSpace(pokemonName))
            {
                return BadRequest("Pokemon Name cannot be empty");
            }
            var result = await _pokemon.GetTranslatedInformation(pokemonName);
            
            if (string.IsNullOrWhiteSpace(result.Name))
            {
                return NotFound();
            }
            
            return new OkObjectResult(result);
        }
    }
}