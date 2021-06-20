namespace Pokemon.API.Models
{
    public class PokemonResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Habitat { get; set; }
        public bool Is_Legendary { get; set; }
    }
}