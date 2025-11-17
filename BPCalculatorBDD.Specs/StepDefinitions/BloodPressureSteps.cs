using BPCalculator;
using TechTalk.SpecFlow;
using Xunit;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace BPCalculatorBDD.Specs.StepDefinitions
{
    [Binding]
    public class BloodPressureSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private BloodPressure _bp;
        private string _displayCategory = string.Empty;   // Fixed initialization

        public BloodPressureSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _bp = new BloodPressure();
        }

        [Given(@"my systolic is (.*)")]
        public void GivenMySystolicIs(int systolic)
        {
            _bp.Systolic = systolic;
        }

        [Given(@"my diastolic is (.*)")]
        public void GivenMyDiastolicIs(int diastolic)
        {
            _bp.Diastolic = diastolic;
        }

        [When(@"I calculate the blood pressure category")]
        public void WhenICalculateTheBloodPressureCategory()
        {
            var category = _bp.Category;

            // Read Display(Name="...") attribute safely
            var field = category.GetType().GetField(category.ToString());
            var attr = field?
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .Cast<DisplayAttribute>()
                .FirstOrDefault();

            _displayCategory = attr?.Name ?? category.ToString();
        }

        [Then(@"the category should be ""(.*)""")]
        public void ThenTheCategoryShouldBe(string expectedCategory)
        {
            Assert.Equal(expectedCategory, _displayCategory);
        }
    }
}
