using FluentAssertions.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Domion.FluentAssertions.Extensions
{
    public static class FluentAssertionsExtensions
    {
        /// <summary>
        /// Asserts that the ValidationError collection contains a message that starts with the constant part of errorMessage, 
        /// i.e. up to the first substitution placeholder ("{.*}"), if any.
        /// </summary>
        /// <param name="assertion"></param>
        /// <param name="errorMessage">Error message text, will be trimmed up to the first substitution placeholder ("{.*}")</param>
        public static void ContainErrorMessage(this GenericCollectionAssertions<ValidationResult> assertion, string errorMessage)
        {
            var errorMessageStart = errorMessage.Split('{')[0];

            assertion.Match(c => c.Where(vr => vr.ErrorMessage.StartsWith(errorMessageStart)).Any());
        }
    }
}
