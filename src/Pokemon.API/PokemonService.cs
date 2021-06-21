namespace Pokemon.API
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;
    using Models;

    public class PokemonService : IPokemonService
    {
        private const string YodaClientName = "yodaTranslation";
        private const string ShakespeareClientName = "shakespeareTranslation";
        private const string PokemonClientName = "basicInformation";
        private const string Cave = "cave";

        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger _logger;

        public PokemonService(IHttpClientFactory clientFactory,ILogger<PokemonService> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }
        public async Task<PokemonResponse> GetBasicInformation(string pokemonName)
        {
            
            var result = new PokemonResponse();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{pokemonName}");

            var client = _clientFactory.CreateClient(PokemonClientName);

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Pokemon information not found");
                return result;
            }
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var rawPokemonResponse = await JsonSerializer.DeserializeAsync<PokemonApiResponse>(responseStream);

            result.Description = rawPokemonResponse.flavor_text_entries[0].flavor_text;
            result.Habitat = rawPokemonResponse.habitat?.name ?? string.Empty;
            result.Is_Legendary = rawPokemonResponse.is_legendary;
            result.Name = rawPokemonResponse.name;
            
            return result;
        }

        public async Task<PokemonResponse> GetTranslatedInformation(string pokemonName)
        {
            var basicInformation = await GetBasicInformation(pokemonName);

            if (string.IsNullOrWhiteSpace(basicInformation.Name))
            {
                _logger.LogError("Pokemon information not found");
                return new PokemonResponse();
            }
            try
            {
                if (basicInformation.Habitat == Cave || basicInformation.Is_Legendary)
                {
                    return await GetTranslation(basicInformation,YodaClientName);
                }
                else
                {
                    return await GetTranslation(basicInformation, ShakespeareClientName);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Error with translation , will return basic information");
                return basicInformation;
            }
        }

        private async Task<PokemonResponse> GetTranslation(PokemonResponse basicInformation, string clientName)
        {
            var queryString = new Dictionary<string, string>()
            {
                { "text", basicInformation.Description.Replace("\n", " ") }
            };
            var client = _clientFactory.CreateClient(clientName);
            var requestUri = QueryHelpers.AddQueryString(client.BaseAddress.AbsoluteUri, queryString);

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode) return basicInformation;
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var rawTranslationResponse = await JsonSerializer.DeserializeAsync<TranslationResponse>(responseStream);

            if (rawTranslationResponse.success.total == 1)
            {
                basicInformation.Description = rawTranslationResponse.contents.translated;
            }

            return basicInformation;
        }

        #region Translation models

        private class TranslationResponse
        {
            public Success success { get; set; }
            public Contents contents { get; set; }
        }
        private class Success
        {
            public int total { get; set; }
        }
        private class Contents
        {
            public string translated { get; set; }
            public string text { get; set; }
            public string translation { get; set; }
        }

        #endregion

        #region Raw pokemon response models

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

        #endregion

    }
}