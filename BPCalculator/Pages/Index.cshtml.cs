using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System.Collections.Generic;

// page model

namespace BPCalculator.Pages
{
    public class BloodPressureModel : PageModel
    {

        private readonly TelemetryClient? _telemetry;

        [BindProperty]                              // bound on POST
        public BloodPressure BP { get; set; } = new BloodPressure();
        public string TrendMessage { get; set; } = string.Empty;

        public BloodPressureModel(TelemetryClient? telemetry = null)
        {
            _telemetry = telemetry;
        }

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

            var calcProps = new Dictionary<string, string>
            {
                ["Name"] = BP.Name ?? string.Empty,
                ["Systolic"] = BP.Systolic.ToString(),
                ["Diastolic"] = BP.Diastolic.ToString(),
                ["Category"] = BP.Category.ToString()
            };

            _telemetry?.TrackEvent("BloodPressureCalculated", calcProps);

            // ✅ Telemetry for your new feature (trend message)
            if (!string.IsNullOrWhiteSpace(TrendMessage))
            {
                var trendProps = new Dictionary<string, string>
                {
                    ["Name"] = BP.Name ?? string.Empty,
                    ["TrendMessage"] = TrendMessage
                };

                _telemetry?.TrackTrace("BloodPressureTrend", SeverityLevel.Information, trendProps);
            }

            return Page();
        }
    }
}