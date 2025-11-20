using System;
using BPCalculator;
using BPCalculator.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Xunit;

namespace BPCalculatorUnit.Tests
{
    public class BloodPressureModelTests
    {
        private BloodPressureModel CreatePageModel(TestSession? session = null)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Session = session ?? new TestSession();

            var pageContext = new PageContext
            {
                HttpContext = httpContext
            };

            var model = new BloodPressureModel
            {
                PageContext = pageContext
            };

            return model;
        }

        [Fact]
        public void OnGet_Initialises_Default_BloodPressure()
        {
            // Arrange
            var model = CreatePageModel();

            // Act
            model.OnGet();

            // Assert
            Assert.NotNull(model.BP);
            Assert.Equal(100, model.BP.Systolic);
            Assert.Equal(60, model.BP.Diastolic);
        }

        [Fact]
        public void OnPost_Invalid_When_Systolic_Not_Greater_Than_Diastolic()
        {
            var model = CreatePageModel();
            model.BP = new BloodPressure
            {
                Name = "Alice",
                Systolic = 80,
                Diastolic = 80
            };

            var result = model.OnPost();

            Assert.False(model.ModelState.IsValid);
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public void OnPost_First_Reading_Sets_FirstTime_Message()
        {
            var model = CreatePageModel();
            model.BP = new BloodPressure
            {
                Name = "Alice",
                Systolic = 120,
                Diastolic = 80
            };

            model.OnPost();

            Assert.True(model.ModelState.IsValid);
            Assert.Contains("first reading", model.TrendMessage, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void OnPost_Higher_Reading_Sets_Increased_Message()
        {
            var session = new TestSession();

            // previous reading for alice: 110/70
            session.SetInt32("LastSystolic_alice", 110);
            session.SetInt32("LastDiastolic_alice", 70);

            var model = CreatePageModel(session);
            model.BP = new BloodPressure
            {
                Name = "Alice",
                Systolic = 130,
                Diastolic = 85
            };

            model.OnPost();

            Assert.Contains("increased", model.TrendMessage, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void OnPost_Lower_Reading_Sets_Decreased_Message()
        {
            var session = new TestSession();

            // previous reading: 150/95
            session.SetInt32("LastSystolic_alice", 150);
            session.SetInt32("LastDiastolic_alice", 95);

            var model = CreatePageModel(session);
            model.BP = new BloodPressure
            {
                Name = "Alice",
                Systolic = 120,
                Diastolic = 80
            };

            model.OnPost();

            Assert.Contains("decreased", model.TrendMessage, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void OnPost_Equal_Reading_Sets_Unchanged_Message()
        {
            var session = new TestSession();

            session.SetInt32("LastSystolic_bob", 120);
            session.SetInt32("LastDiastolic_bob", 80);

            var model = CreatePageModel(session);
            model.BP = new BloodPressure
            {
                Name = "Bob",
                Systolic = 120,
                Diastolic = 80
            };

            model.OnPost();

            Assert.Contains("unchanged", model.TrendMessage, StringComparison.OrdinalIgnoreCase);
        }
    }
}
