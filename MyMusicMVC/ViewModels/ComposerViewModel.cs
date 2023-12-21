using System.ComponentModel.DataAnnotations;

namespace MyMusicMVC.ViewModels
{
    public class ComposerViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
