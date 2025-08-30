using Microsoft.AspNetCore.Mvc.ModelBinding;
using ScheduleGenerator.Data;
using ScheduleGenerator.Models;

namespace ScheduleGenerator.Service
{
    public class ScheduleService 
    {
        private readonly AppDbContext _context;

        public ScheduleService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить события по дате
        /// </summary>
        public List<ScheduleItem> GetEventsByDate(DateOnly date)
        {
            return _context.ScheduleItems
                .Where(x => x.Date == date)
                .OrderBy(x => x.StartTime)
                .ToList();
        }

        /// <summary>
        /// Добавить событие
        /// </summary>
        public void AddEvent(ScheduleItem item)
        {
            _context.ScheduleItems.Add(item);
            _context.SaveChanges();
        }

        /// <summary>
        /// Редактировать событие
        /// </summary>
        public bool EditEvent(ScheduleItem editItem)
        {
            var newItem = _context.ScheduleItems.FirstOrDefault(x => x.Id == editItem.Id);
            if (newItem == null)
                return false;

            newItem.Title = editItem.Title;
            newItem.StartTime = editItem.StartTime;
            newItem.EndTime = editItem.EndTime;

            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Удалить событие
        /// </summary>
        public bool DeleteEvent(int id)
        {
            var item = _context.ScheduleItems.FirstOrDefault(x => x.Id == id);
            if (item == null) 
                return false;

            _context.ScheduleItems.Remove(item);
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Валидации события
        /// </summary>
        public bool ValidateEvent(ModelStateDictionary modelState, ScheduleItem item, int? id, string prefix)
        {
            if (string.IsNullOrWhiteSpace(item.Title))
            {
                modelState.AddModelError($"{prefix}.Title", "Введите название события");
                return false;
            }

            item.Title = item.Title.Trim();

            if (!System.Text.RegularExpressions.Regex.IsMatch(item.Title, @"^[A-Za-zА-Яа-я0-9].*"))
            {
                modelState.AddModelError($"{prefix}.Title", "Событие должно начинаться с буквы или цифры");
                return false;
            }

            if (item.Title.Length < 3 || item.Title.Length > 50)
            {
                modelState.AddModelError($"{prefix}.Title", "Название должно быть от 3 до 50 символов");
                return false;
            }

            if (item.StartTime == default)
            {
                modelState.AddModelError($"{prefix}.StartTime", "Введите время начала события");
                return false;
            }

            if (item.EndTime <= item.StartTime)
            {
                modelState.AddModelError($"{prefix}.EndTime", "Конец события должен быть позже начала");
                return false;
            }

            var overlaps = _context.ScheduleItems.Any(x =>
                x.Id != id &&
                x.Date == item.Date &&
                x.StartTime < item.EndTime &&
                x.EndTime > item.StartTime);

            if (overlaps)
            {
                modelState.AddModelError($"{prefix}.StartTime", "События пересекаются");
                return false;
            }

            return modelState.IsValid;
        }
    }
}
