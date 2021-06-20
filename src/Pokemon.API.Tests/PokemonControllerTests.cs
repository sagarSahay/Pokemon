namespace Pokemon.API.Tests
{
    using System.Threading.Tasks;
    using Controllers;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using NUnit.Framework;

    [TestFixture]
    public class PokemonControllerTests
    {
        private PokemonController _sut;


        [SetUp]
        public void SetUp()
        {
            _sut = new PokemonController();
        }


        [Test]
        public async Task Basic_information_endpoint_when_receives_pokemon_name_returns_basic_information()
        {
            // Arrange
            var pokemonName = "pikachu";

            // Act
            var result =  await _sut.BasicInformation(new PokemonRequest(){Name = pokemonName});

            var okObjectresult = result as OkObjectResult;

            var pokemonBasicInfo = (PokemonResponse) okObjectresult.Value;
            
            // Assert
            Assert.AreEqual("metwo", pokemonBasicInfo.Name);
        }
        
    }
}