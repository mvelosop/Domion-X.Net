using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domion.WebApp.Extensions
{
    public static class ModelStateDictionaryExtensions
    {
        /// <summary>
        ///     Clears ModelStateDictionary and adds the validation results
        /// </summary>
        /// <param name="modelState">The ModelStateDictionary to reset</param>
        /// <param name="validationResults">The ValidationResults to add</param>
        public static void ResetModelErrors(this ModelStateDictionary modelState, IEnumerable<ValidationResult> validationResults)
        {
            modelState.Clear();

            foreach (var item in validationResults)
            {
                if (item.MemberNames.Any())
                {
                    foreach (var member in item.MemberNames)
                    {
                        modelState.TryAddModelError(member, item.ErrorMessage);
                    }
                }
                else
                {
                    modelState.TryAddModelError(string.Empty, item.ErrorMessage);
                }
            }

        }
    }
}
