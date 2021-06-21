namespace Pokemon.API.Integration.Tests
{
    using System.Threading.Tasks;
    using Controllers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class PokemonControllerTests
    {
        private PokemonController _sut;
        private IPokemonService _pokemon;
        private Mock<ILogger<PokemonService>> _logger;
        
        [SetUp]
        public void SetUp()
        {
            _logger = new Mock<ILogger<PokemonService>>();
            _pokemon = new PokemonService(new DefaultHttpClientFactory(), _logger.Object);
            _sut = new PokemonController(_pokemon);
        }

        [Test]
        public async Task Basic_information_endpoint_when_receives_pokemon_name_returns_basic_information()
        {
            // Arrange
            var pokemonName = "wormadam";

            // Act
            var result =  await _sut.BasicInformation(pokemonName);

            var okObjectresult = result as OkObjectResult;

            var pokemonBasicInfo = (PokemonResponse) okObjectresult.Value;
            
            // Assert
            Assert.AreEqual("wormadam", pokemonBasicInfo.Name);
            Assert.AreEqual("When BURMY evolved, its cloak\nbecame a part of this Pokémon’s\nbody. The cloak is never shed.", pokemonBasicInfo.Description);
            Assert.AreEqual("", pokemonBasicInfo.Habitat);
            Assert.AreEqual(false, pokemonBasicInfo.Is_Legendary);
        }


        [Test]
        public async Task Translated_information_endpoint_when_receives_pokemon_name_which_is_legendary_returns_yoda_translation()
        {
            // Arrange
            var pokemonName = "mewtwo";
            
            // Act
            var result =  await _sut.TranslatedInformation(pokemonName);

            var okObjectresult = result as OkObjectResult;

            var pokemonBasicInfo = (PokemonResponse) okObjectresult.Value;
            
            // Assert
            Assert.AreEqual("mewtwo", pokemonBasicInfo.Name);
            Assert.AreEqual("Created by a scientist after years of horrific gene splicing and dna engineering experiments,  it was.", pokemonBasicInfo.Description);
            Assert.AreEqual("rare", pokemonBasicInfo.Habitat);
            Assert.AreEqual(true, pokemonBasicInfo.Is_Legendary);
        }
        
        [Test]
        public async Task Translated_information_endpoint_when_receives_pokemon_name_which_is_not_or_has_cave_as_habitat_legendary_returns_shakespeare_translation()
        {
            // Arrange
            var pokemonName = "wormadam";
            
            // Act
            var result =  await _sut.TranslatedInformation(pokemonName);

            var okObjectresult = result as OkObjectResult;

            var pokemonBasicInfo = (PokemonResponse) okObjectresult.Value;
            
            // Assert
            Assert.AreEqual("wormadam", pokemonBasicInfo.Name);
            Assert.AreEqual("At which hour burmy evolved,  its cloak did doth becometh a part of this pokémon’s corse. The cloak is nev'r did shed.", pokemonBasicInfo.Description);
            Assert.AreEqual("", pokemonBasicInfo.Habitat);
            Assert.AreEqual(false, pokemonBasicInfo.Is_Legendary);
        }
    }
}