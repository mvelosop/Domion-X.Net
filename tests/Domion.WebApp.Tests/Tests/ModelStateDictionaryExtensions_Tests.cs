using Domion.WebApp.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace Domion.WebApp.Tests.Tests
{
    [Trait("Type", "Unit")]
    public class ModelStateDictionaryExtensions_Tests
    {
        [Fact]
        public void SetValidationResults_ReplacesErrors()
        {
            // Arrange ---------------------------

            var modelState = new ModelStateDictionary();

            modelState.AddModelError(string.Empty, "Initial General Error");
            modelState.AddModelError("Property", "Initial Property Error");

            var errors = new List<ValidationResult>
            {
                new ValidationResult("New General Error"),
                new ValidationResult("New Properties Error A", new[]{ "PropertyA", "PropertyB" }),
                new ValidationResult("New Properties Error B", new[]{ "PropertyA", "PropertyB" }),
            };

            // Act -------------------------------

            modelState.SetValidationResults(errors);

            // Assert ----------------------------

            modelState.IsValid.Should().BeFalse();
            modelState.Count.Should().Be(3);
            modelState.Select(mse => mse.Value.Errors.Select(me => me.ErrorMessage)).SelectMany(e => e.Select(t => t)).Distinct()
                .Should().BeEquivalentTo(errors.Select(vr => vr.ErrorMessage));
        }
    }
}
