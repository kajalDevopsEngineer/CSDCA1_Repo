using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

// page model

namespace BPCalculator.Pages
{
    public class BloodPressureModel : PageModel
    {
        [BindProperty]                              // bound on POST
        public BloodPressure BP { get; set; } = new BloodPressure();
        public string TrendMessage { get; set; } = string.Empty;

        // setup initial data
        public void OnGet()
        {
            BP = new BloodPressure() { Systolic = 100, Diastolic = 60 };
        }

        // POST, validate
        public IActionResult OnPost()
        {
            // extra validation
            if (!(BP.Systolic > BP.Diastolic))
            {
                ModelState.AddModelError("", "Systolic must be greater than Diastolic");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var nameKey = BP.Name?.Trim().ToLower() ?? string.Empty;
            string sysKey = $"LastSystolic_{nameKey}";
            string diaKey = $"LastDiastolic_{nameKey}";

            int? lastSys = HttpContext.Session.GetInt32(sysKey);
            int? lastDia = HttpContext.Session.GetInt32(diaKey);

            if (lastSys.HasValue && lastDia.HasValue)
            {
                int oldTotal = lastSys.Value + lastDia.Value;
                int newTotal = BP.Systolic + BP.Diastolic;

                if (newTotal > oldTotal)
                    TrendMessage = $"{BP.Name}'s blood pressure has increased since last time.";
                else if (newTotal < oldTotal)
                    TrendMessage = $"{BP.Name}'s blood pressure has decreased since last time.";
                else
                    TrendMessage = $"{BP.Name}'s blood pressure is unchanged since last time.";
            }
            else
            {
                // 👇 NEW: first time we see this name
                TrendMessage = $"This is the first reading we have stored for {BP.Name}.";
            }

            // Save current reading for this person
            HttpContext.Session.SetInt32(sysKey, BP.Systolic);
            HttpContext.Session.SetInt32(diaKey, BP.Diastolic);

            return Page();
        }
    }
}