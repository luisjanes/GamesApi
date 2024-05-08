namespace GamesApi.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime LaunchDate { get; set; }
        public string Developer { get; set; } = string.Empty;
    }
}
