using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BPCalculator;
using Xunit;

namespace BPCalculatorUnit.Tests
{
    public class BloodPressureValidationTests
    {
        private static bool TryValidate(object model, out List<ValidationResult> results)
        {
            var context = new ValidationContext(model);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(model, context, results, validateAllProperties: true);
        }

        [Fact]
        public void Validation_Fails_When_Name_Is_Empty()
        {
            var bp = new BloodPressure { Name = string.Empty, Systolic = 110, Diastolic = 70 };

            var ok = TryValidate(bp, out var results);

            Assert.False(ok);
            Assert.Contains(results, r => r.ErrorMessage?.Contains("Name is required") == true || r.ErrorMessage?.Contains("required") == true);
        }

        [Fact]
        public void Validation_Fails_When_Systolic_Too_Low()
        {
            var bp = new BloodPressure { Name = "T", Systolic = BloodPressure.SystolicMin - 1, Diastolic = 60 };

            var ok = TryValidate(bp, out var results);

            Assert.False(ok);
            Assert.Contains(results, r => r.ErrorMessage?.Contains("Systolic") == true || r.ErrorMessage?.Contains("Invalid Systolic") == true);
        }

        [Fact]
        public void Validation_Fails_When_Systolic_Too_High()
        {
            var bp = new BloodPressure { Name = "T", Systolic = BloodPressure.SystolicMax + 1, Diastolic = 60 };

            var ok = TryValidate(bp, out var results);

            Assert.False(ok);
            Assert.Contains(results, r => r.ErrorMessage?.Contains("Systolic") == true || r.ErrorMessage?.Contains("Invalid Systolic") == true);
        }

        [Fact]
        public void Validation_Fails_When_Diastolic_Too_Low()
        {
            var bp = new BloodPressure { Name = "T", Systolic = 100, Diastolic = BloodPressure.DiastolicMin - 1 };

            var ok = TryValidate(bp, out var results);

            Assert.False(ok);
            Assert.Contains(results, r => r.ErrorMessage?.Contains("Diastolic") == true || r.ErrorMessage?.Contains("Invalid Diastolic") == true);
        }

        [Fact]
        public void Validation_Fails_When_Diastolic_Too_High()
        {
            var bp = new BloodPressure { Name = "T", Systolic = 100, Diastolic = BloodPressure.DiastolicMax + 1 };

            var ok = TryValidate(bp, out var results);

            Assert.False(ok);
            Assert.Contains(results, r => r.ErrorMessage?.Contains("Diastolic") == true || r.ErrorMessage?.Contains("Invalid Diastolic") == true);
        }

        [Fact]
        public void Category_Boundaries_Are_Correct()
        {
            // Exactly at upper/lower bounds to ensure correct category selection
            var bpHighBySystolic = new BloodPressure { Name = "T", Systolic = 140, Diastolic = 60 };
            Assert.Equal(BPCategory.High, bpHighBySystolic.Category);

            var bpHighByDiastolic = new BloodPressure { Name = "T", Systolic = 120, Diastolic = 90 };
            Assert.Equal(BPCategory.High, bpHighByDiastolic.Category);

            var bpPreHighLower = new BloodPressure { Name = "T", Systolic = 120, Diastolic = 60 };
            Assert.Equal(BPCategory.PreHigh, bpPreHighLower.Category);

            var bpPreHighUpper = new BloodPressure { Name = "T", Systolic = 139, Diastolic = 89 };
            Assert.Equal(BPCategory.PreHigh, bpPreHighUpper.Category);

            var bpIdealLower = new BloodPressure { Name = "T", Systolic = 90, Diastolic = 60 };
            Assert.Equal(BPCategory.Ideal, bpIdealLower.Category);

            var bpIdealUpper = new BloodPressure { Name = "T", Systolic = 119, Diastolic = 79 };
            Assert.Equal(BPCategory.Ideal, bpIdealUpper.Category);

            var bpLow = new BloodPressure { Name = "T", Systolic = 70, Diastolic = 40 };
            Assert.Equal(BPCategory.Low, bpLow.Category);
        }
    }
}
