namespace Pokemon.API.Tests
{
    using System;
    using System.Net.Http;

    public sealed class DefaultHttpClientFactory : IHttpClientFactory
    {
        private HttpClient client;
        public HttpClient CreateClient(string name)
        {
            client = new HttpClient();
            switch (name)
            {
                case "basicInformation":
                    client.BaseAddress = new Uri("https://pokeapi.co/api/v2/pokemon-species/");
                    break;
                case "yodaTranslation":
                    client.BaseAddress = new Uri("https://api.funtranslations.com/translate/yoda.json");
                    break;
                case "shakespeareTranslation":
                    client.BaseAddress = new Uri("https://api.funtranslations.com/translate/shakespeare.json");
                    break;
            }

            return client;
        }
    }
}