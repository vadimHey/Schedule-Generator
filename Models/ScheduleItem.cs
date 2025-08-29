using System.ComponentModel.DataAnnotations;

namespace ScheduleGenerator.Models
{
    public class ScheduleItem
    {
        [Key]
        public int Id { get; set; }

        public string? Title { get; set; }

        [DataType(DataType.Date)]
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        [DataType(DataType.Time)]
        public TimeOnly StartTime { get; set; }

        [DataType(DataType.Time)]
        public TimeOnly EndTime { get; set; }
    }
}
