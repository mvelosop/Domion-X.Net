//------------------------------------------------------------------------------
//  Tag.cs
//
//  Implementation of: Tag (Class) <<ef-entity>>
//  Generated by Domion-MDA - http://www.coderepo.blog/domion
//
//  Created on     : 02-jun-2017 10:49:10
//  Original author: Miguel
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DFlow.Transactions.Core.Model
{
	public class Tag
	{
		public Tag()
		{
		}

		public int Id { get; set; }

		[Required]
		[MaxLength(100)]
		public virtual string Name { get; set; } // Key data ----------

		public virtual Byte[] RowVersion { get; set; }
	}
}