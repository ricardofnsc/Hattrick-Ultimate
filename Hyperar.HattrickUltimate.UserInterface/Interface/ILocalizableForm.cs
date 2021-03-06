﻿//-----------------------------------------------------------------------
// <copyright file="ILocalizableForm.cs" company="Hyperar">
//     Copyright (c) Hyperar. All rights reserved.
// </copyright>
// <author>Matías Ezequiel Sánchez</author>
//-----------------------------------------------------------------------
namespace Hyperar.HattrickUltimate.UserInterface.Interface
{
    /// <summary>
    /// Localized form definition.
    /// </summary>
    internal interface ILocalizableForm
    {
        #region Public Methods

        /// <summary>
        /// Populates controls' properties with the corresponding localized string.
        /// </summary>
        void PopulateLanguage();

        #endregion Public Methods
    }
}