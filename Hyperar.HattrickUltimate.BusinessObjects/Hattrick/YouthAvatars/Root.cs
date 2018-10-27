﻿// -----------------------------------------------------------------------
// <copyright file="Root.cs" company="Hyperar">
//     Copyright (c) Hyperar. All rights reserved.
// </copyright>
// <author>Matías Ezequiel Sánchez</author>
// -----------------------------------------------------------------------
namespace Hyperar.HattrickUltimate.BusinessObjects.Hattrick.YouthAvatars
{
    /// <summary>
    /// Avatars XML file root.
    /// </summary>
    public class Root : XmlEntityBase
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the Youth Team.
        /// </summary>
        public YouthTeam YouthTeam { get; set; }

        #endregion Public Properties
    }
}