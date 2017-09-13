﻿// -----------------------------------------------------------------------
// <copyright file="Country.cs" company="Hyperar">
//     Copyright (c) Hyperar. All rights reserved.
// </copyright>
// <author>Matías Ezequiel Sánchez</author>
// -----------------------------------------------------------------------
namespace Hyperar.HattrickUltimate.BusinessObjects.Hattrick.ManagerCompendium
{
    /// <summary>
    /// Country node within ManagerCompendium XML file.
    /// </summary>
    public class Country
    {
        #region Properties

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        public uint CountryId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string CountryName { get; set; }

        #endregion Properties
    }
}