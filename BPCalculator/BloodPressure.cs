using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace BPCalculator
{
    // BP categories
    public enum BPCategory
    {
        [Display(Name = "Low Blood Pressure")] Low,
        [Display(Name = "Ideal Blood Pressure")] Ideal,
        [Display(Name = "Pre-High Blood Pressure")] PreHigh,
        [Display(Name = "High Blood Pressure")] High
    };

    public class BloodPressure
    {
        public const int SystolicMin = 70;
        public const int SystolicMax = 190;
        public const int DiastolicMin = 40;
        public const int DiastolicMax = 100;

        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Patient Name")]

        public string Name { get; set; } = string.Empty;

        [Range(SystolicMin, SystolicMax, ErrorMessage = "Invalid Systolic Value")]
        public int Systolic { get; set; }                       // mmHG

        [Range(DiastolicMin, DiastolicMax, ErrorMessage = "Invalid Diastolic Value")]
        public int Diastolic { get; set; }                      // mmHG



        // calculate BP category
        public BPCategory Category
        {
            get
            {
                // High blood pressure if systolic >= 140 or diastolic >= 90
                if (Systolic >= 140 || Diastolic >= 90)
                {
                    return BPCategory.High;
                }

                // Pre-high blood pressure if systolic in [120,139] or diastolic in [80,89]
                if ((Systolic >= 120 && Systolic <= 139) || (Diastolic >= 80 && Diastolic <= 89))
                {
                    return BPCategory.PreHigh;
                }

                // Ideal blood pressure if systolic in [90,119] and diastolic in [60,79]
                if (Systolic >= 90 && Systolic <= 119 && Diastolic >= 60 && Diastolic <= 79)
                {
                    return BPCategory.Ideal;
                }

                // Anything else within valid ranges is considered low blood pressure
                return BPCategory.Low;
            }
        }
    }
}
