using System;
using System.ComponentModel.DataAnnotations;

namespace DFlow.WebApp.Features.Tenants
{
    public class TenantViewModel
    {
        //----------------------------------------
        // UI properties
        //----------------------------------------

        [Display(Name = "Nombre")]
        [MaxLength(250)]
        public string Owner { get; set; }

        [Display(Name = "Notas")]
        [MaxLength(1000)]
        public string Notes { get; set; }

        //----------------------------------------
        // Support properties
        //----------------------------------------

        public int Id { get; set; }

        public virtual Byte[] RowVersion { get; set; }
    }
}
