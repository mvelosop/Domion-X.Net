using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DFlow.WebApp.Features.Tenants
{
    public class TenantViewModel
    {
        public TenantViewModel()
        {
            LastIndexRouteDictionary = new Dictionary<string, string>();
        }

        public string Title { get; set; }

        //----------------------------------------
        // Form properties
        //----------------------------------------

        [Display(Name = "Nombre")]
        [MaxLength(250)]
        [Required(ErrorMessage = "Debe indicar el nombre del cliente.")]
        public string Owner { get; set; }

        [Display(Name = "Notas")]
        [MaxLength(1000)]
        public string Notes { get; set; }

        //----------------------------------------
        // Support properties
        //----------------------------------------

        public int? Id { get; set; }

        public virtual Byte[] RowVersion { get; set; }

        public Dictionary<string, string> LastIndexRouteDictionary { get; set; }
    }
}
