﻿using Domion.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Domion.Lib.Extensions
{
    /// <summary>
    ///     Extensions for generic IQueryManager < T >.
    /// </summary>
    public static class IQueryManagerExtensions
    {
        /// <summary>
        ///     Returns the first object that satisfies the condition or raises InvalidOperationException if none.
        /// </summary>
        public static T First<T>(this IRepositoryQuery<T> repo, Expression<Func<T, bool>> where = null) where T : class
        {
            return repo.Query(where).First();
        }

        /// <summary>
        ///     Returns the first object that satisfies the condition or null if none.
        /// </summary>
        public static T FirstOrDefault<T>(this IRepositoryQuery<T> repo, Expression<Func<T, bool>> where = null) where T : class
        {
            return repo.Query(where).FirstOrDefault<T>();
        }

        /// <summary>
        ///     <para>
        ///         Specifies the related objects to include in the query result and returns and IIncludableQueryable that allows chaining of other IQueryable methods.
        ///     </para>
        ///     <para>
        ///         For more information and examples visit:
        ///         https://docs.microsoft.com/en-us/ef/core/api/microsoft.entityframeworkcore.entityframeworkqueryableextensions#Microsoft_EntityFrameworkCore_EntityFrameworkQueryableExtensions_Include__2_System_Linq_IQueryable___0__System_Linq_Expressions_Expression_System_Func___0___1___
        ///     </para>
        /// </summary>
        /// <param name="includeExpression">The navigation property to include</param>
        public static IIncludableQueryable<T, TProperty> Include<T, TProperty>(this IRepositoryQuery<T> repo, Expression<Func<T, TProperty>> includeExpression) where T : class
        {
            return repo.Query().Include(includeExpression);
        }

        /// <summary>
        ///     Returns the single object that satisfies the condition or raises InvalidOperationException if none or more than one.
        /// </summary>
        public static T Single<T>(this IRepositoryQuery<T> repo, Expression<Func<T, bool>> where = null) where T : class
        {
            return repo.Query(where).Single<T>();
        }

        /// <summary>
        ///     Returns the single object that satisfies the condition or null if none or raises InvalidOperationException if more than one.
        /// </summary>
        public static T SingleOrDefault<T>(this IRepositoryQuery<T> repo, Expression<Func<T, bool>> where = null) where T : class
        {
            return repo.Query(where).SingleOrDefault<T>();
        }
    }
}
