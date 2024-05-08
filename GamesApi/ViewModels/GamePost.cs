using System.ComponentModel.DataAnnotations;

namespace GamesApi.ViewModels
{
    public class GamePost
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime LaunchDate { get; set; }
        [Required]
        public string Developer { get; set; } = string.Empty;
    }
}
