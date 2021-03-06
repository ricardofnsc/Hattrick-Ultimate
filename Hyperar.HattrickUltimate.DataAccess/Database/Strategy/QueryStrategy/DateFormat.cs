﻿//-----------------------------------------------------------------------
// <copyright file="DateFormat.cs" company="Hyperar">
//     Copyright (c) Hyperar. All rights reserved.
// </copyright>
// <author>Matías Ezequiel Sánchez</author>
//-----------------------------------------------------------------------
namespace Hyperar.HattrickUltimate.DataAccess.Database.Strategy.QueryStrategy
{
    using System.Data.Entity;
    using System.Linq;
    using Interface;

    /// <summary>
    /// DateFormat Query Strategy.
    /// </summary>
    public class DateFormat : IQueryStrategy<BusinessObjects.App.DateFormat>
    {
        #region Public Methods

        /// <summary>
        /// Applies the corresponding Includes depending on the class.
        /// </summary>
        /// <param name="query">Query to apply includes to.</param>
        /// <returns>Query with included child entities.</returns>
        public IQueryable<BusinessObjects.App.DateFormat> ApplyIncludes(IQueryable<BusinessObjects.App.DateFormat> query)
        {
            return query.Include(p => p.Countries);
        }

        #endregion Public Methods
    }
}