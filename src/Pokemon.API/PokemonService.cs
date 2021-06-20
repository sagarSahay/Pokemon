namespace Pokemon.API
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.WebUtilities;
    using Models;

    public class PokemonService : IPokemonService
    {
        private readonly IHttpClientFactory _clientFactory;

        public PokemonService(IHttpClientFactory clientFactory)
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

        public async Task<PokemonResponse> GetTranslatedInformation(string pokemonName)
        {
            var basicInformation = await GetBasicInformation(pokemonName);

            try
            {
                if (basicInformation.Habitat == "cave" || basicInformation.Is_Legendary)
                {
                    return await GetTranslation(basicInformation,"yodaTranslation");
                }
                else
                {
                    return await GetTranslation(basicInformation, "shakespeareTranslation");
                }
            }
            catch (Exception e)
            {
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
        /*
         *https://api.funtranslations.com/translate/yoda.json?text=It%20was%20created%20by%0Aa%20scientist%20after%0Ayears%20of%20horrific%0Cgene%20splicing%20and%0ADNA%20engineering%0Aexperiments.
         *https://api.funtranslations.com/translate/yoda.json?text=You%20gave%20Mr.%20Tim%20a%20hearty%20meal%2C%20but%20unfortunately%20what%20he%20ate%20made%20him%20die.
         */

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