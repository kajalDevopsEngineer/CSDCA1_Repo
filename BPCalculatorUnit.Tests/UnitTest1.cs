using Xunit;
using BPCalculator;

namespace BPCalculatorUnit.Tests
{
    public class BloodPressureCategoryTests
    {
        [Fact]
        public void Category_IsHigh_When_Systolic_AtLeast_140()
        {
            // Arrange
            var bp = new BloodPressure { Systolic = 140, Diastolic = 70 };

            // Act
            var category = bp.Category;

            // Assert
            Assert.Equal(BPCategory.High, category);
        }

        [Fact]
        public void Category_IsHigh_When_Diastolic_AtLeast_90()
        {
            var bp = new BloodPressure { Systolic = 120, Diastolic = 90 };

            var category = bp.Category;

            Assert.Equal(BPCategory.High, category);
        }

        [Fact]
        public void Category_IsPreHigh_When_Systolic_Between_120_And_139()
        {
            var bp = new BloodPressure { Systolic = 130, Diastolic = 70 };

            var category = bp.Category;

            Assert.Equal(BPCategory.PreHigh, category);
        }

        [Fact]
        public void Category_IsPreHigh_When_Diastolic_Between_80_And_89()
        {
            var bp = new BloodPressure { Systolic = 110, Diastolic = 85 };

            var category = bp.Category;

            Assert.Equal(BPCategory.PreHigh, category);
        }

        [Fact]
        public void Category_IsIdeal_When_Within_Ideal_Range()
        {
            var bp = new BloodPressure { Systolic = 110, Diastolic = 70 };

            var category = bp.Category;

            Assert.Equal(BPCategory.Ideal, category);
        }

        [Fact]
        public void Category_IsLow_When_Below_Ideal_Range()
        {
            var bp = new BloodPressure { Systolic = 85, Diastolic = 55 };

            var category = bp.Category;

            Assert.Equal(BPCategory.Low, category);
        }
    }
}
