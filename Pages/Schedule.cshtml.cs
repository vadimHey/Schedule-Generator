using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ScheduleGenerator.Pages
{
    public class ScheduleModel : PageModel
    {
        [BindProperty]
        public string Items { get; set; } = string.Empty;

        public List<string> Schedules { get; set; } = new List<string>();

        public void OnPost()
        {
            if(!string.IsNullOrEmpty(Items))
            {
                Schedules = Items
                    .Split(new[] { ',', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToList();
            }
        }
    }
}
