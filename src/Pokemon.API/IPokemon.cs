namespace Pokemon.API
{
    using System.Threading.Tasks;
    using Models;

    public interface IPokemonService
    {
         public Task<PokemonResponse> GetBasicInformation(string pokemonName);

         public Task<PokemonResponse> GetTranslatedInformation(string pokemonName);
    }
}