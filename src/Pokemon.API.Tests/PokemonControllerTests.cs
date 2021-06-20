namespace Pokemon.API.Tests
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Controllers;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using NUnit.Framework;

    public sealed class DefaultHttpClientFactory : IHttpClientFactory
    {
        private HttpClient client;
        public HttpClient CreateClient(string name)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://pokeapi.co/api/v2/pokemon-species/");
            return client;
        }
    }
    [TestFixture]
    public class PokemonControllerTests
    {
        private PokemonController _sut;
        private IPokemon _pokemon;
        
        [SetUp]
        public void SetUp()
        {
            _pokemon = new Pokemon(new DefaultHttpClientFactory());
            _sut = new PokemonController(_pokemon);
        }


        [Test]
        public async Task Basic_information_endpoint_when_receives_pokemon_name_returns_basic_information()
        {
            // Arrange
            var pokemonName = "wormadam";

            // Act
            var result =  await _sut.BasicInformation(new PokemonRequest(){Name = pokemonName});

            var okObjectresult = result as OkObjectResult;

            var pokemonBasicInfo = (PokemonResponse) okObjectresult.Value;
            
            // Assert
            Assert.AreEqual("wormadam", pokemonBasicInfo.Name);
            Assert.AreEqual("Forms have different stats and movepools.  During evolution, Burmy's current cloak becomes Wormadam's form, and can no longer be changed.", pokemonBasicInfo.Description);
            Assert.AreEqual("", pokemonBasicInfo.Habitat);
            Assert.AreEqual(false, pokemonBasicInfo.Is_Legendary);
        }
        
    }
}