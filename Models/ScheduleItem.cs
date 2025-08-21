using System.ComponentModel.DataAnnotations;

namespace ScheduleGenerator.Models
{
    public class ScheduleItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;
    }
}
