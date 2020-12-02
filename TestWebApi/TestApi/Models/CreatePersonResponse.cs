
using System.ComponentModel.DataAnnotations;

namespace TestApi.Models
{
    public class CreatePersonResponse
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string Name { get; set; }
    }
}