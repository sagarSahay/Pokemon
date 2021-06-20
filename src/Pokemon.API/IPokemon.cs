namespace Pokemon.API
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Xml.XPath;
    using Models;

    public interface IPokemon
    {
         public Task<PokemonResponse> GetBasicInformation(string pokemonName);
    }

    public class Pokemon : IPokemon
    {
        private readonly IHttpClientFactory _clientFactory;

        public Pokemon(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<PokemonResponse> GetBasicInformation(string pokemonName)
        {
            var result = new PokemonResponse();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{pokemonName}");

            var client = _clientFactory.CreateClient("basicInformation");

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode) return result;
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var rawPokemonResponse = await JsonSerializer.DeserializeAsync<PokemonApiResponse>(responseStream);

            result.Description = rawPokemonResponse.flavor_text_entries[0].flavor_text;
            result.Habitat = rawPokemonResponse.habitat?.name ?? string.Empty;
            result.Is_Legendary = rawPokemonResponse.is_legendary;
            result.Name = rawPokemonResponse.name;
            
            return result;
        }

        private class PokemonApiResponse
        {
            public string name { get; set; }
            public bool is_legendary { get; set; }
            public NamedResource? habitat { get; set; }
            public IList<FlavorText> flavor_text_entries { get; set; }
        }
        private class FlavorText
        {
            public string flavor_text { get; set; }
            public NamedResource language { get; set; }
            public NamedResource version { get; set; }
        }

        private class NamedResource
        {
            public string name { get; set; }
            public string uri { get; set; }
        }
    }
}