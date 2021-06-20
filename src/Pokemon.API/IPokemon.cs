namespace Pokemon.API
{
    using System.Threading.Tasks;
    using Models;

    public interface IPokemonService
    {
         public Task<PokemonResponse> GetBasicInformation(string pokemonName);

         public Task<PokemonResponse> GetTranslatedInformation(string pokemonName);
    }

    /*
     * https://api.funtranslations.com/translate/yoda.json?&text=It%20was%20created%20by%0Aa%20scientist%20after%0Ayears%20of%20horrific%0Cgene%20splicing%20and%0ADNA%20engineering%0Aexperiments.
     */
}