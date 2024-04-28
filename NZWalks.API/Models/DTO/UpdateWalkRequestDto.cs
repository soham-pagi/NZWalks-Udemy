using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateWalkRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Name has to be a minimum of 3 characters")]
        [MaxLength(100, ErrorMessage = "Name has to be a maximum of 3 characters")]
        public string Name { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Description has to be a minimum of 3 characters")]
        [MaxLength(1000, ErrorMessage = "Description has to be a maximum of 3 characters")]
        public string Description { get; set; }

        [Required]
        [Range(0, 50, ErrorMessage = "The length should be between 0-50")]
        public double LengthInKm { get; set; }

        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }
    }
}
