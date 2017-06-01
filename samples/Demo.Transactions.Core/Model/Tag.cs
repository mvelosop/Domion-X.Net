//------------------------------------------------------------------------------
// Tag.cs
//
// Implementation of: Tag (Class) <<ef-entity>>
// Generated with Domion-MDA - http://www.coderepo.blog/domion
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Demo.Transactions.Core.Model
{
    public class Tag
    {
        public Tag()
        {
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public virtual string Name { get; set; }

        public virtual Byte[] RowVersion { get; set; }
    }
}
