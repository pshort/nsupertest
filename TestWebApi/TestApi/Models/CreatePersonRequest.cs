


using System.ComponentModel.DataAnnotations;

namespace TestApi.Models
{
    public class CreatePersonRequest
    {
        [Range(0, 100)]
        public int Age { get; set; }
        [Required]
        public string Name { get; set; }
    }
}