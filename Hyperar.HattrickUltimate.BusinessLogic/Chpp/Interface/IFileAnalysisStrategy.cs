﻿//-----------------------------------------------------------------------
// <copyright file="IFileAnalysisStrategy.cs" company="Hyperar">
//     Copyright (c) Hyperar. All rights reserved.
// </copyright>
// <author>Matías Ezequiel Sánchez</author>
//-----------------------------------------------------------------------
namespace Hyperar.HattrickUltimate.BusinessLogic.Chpp.Interface
{
    using System.Collections.Generic;
    using BusinessObjects.Hattrick.Interface;
    using Hyperar.HattrickUltimate.BusinessObjects.App;

    /// <summary>
    /// File Analysis Strategy Contract.
    /// </summary>
    public interface IFileAnalysisStrategy
    {
        #region Public Methods

        /// <summary>
        /// Analyses the specified entity.
        /// </summary>
        /// <param name="entity">Entity to analyze.</param>
        /// <param name="downloadSettings">Download Settings.</param>
        /// <returns>Additional Files Tasks list.</returns>
        List<ChppFile> Analyze(IXmlEntity entity, DownloadSettings downloadSettings);

        #endregion Public Methods
    }
}