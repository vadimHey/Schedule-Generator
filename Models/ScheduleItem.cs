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

        [DataType(DataType.Date)]
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        [Required]
        [DataType(DataType.Time)]
        public TimeOnly StartTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeOnly EndTime { get; set; }
    }
}
