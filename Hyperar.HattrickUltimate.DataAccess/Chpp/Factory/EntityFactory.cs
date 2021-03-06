﻿//-----------------------------------------------------------------------
// <copyright file="EntityFactory.cs" company="Hyperar">
//     Copyright (c) Hyperar. All rights reserved.
// </copyright>
// <author>Matías Ezequiel Sánchez</author>
//-----------------------------------------------------------------------
namespace Hyperar.HattrickUltimate.DataAccess.Chpp.Factory
{
    using System;
    using BusinessObjects.Hattrick.Enums;
    using BusinessObjects.Hattrick.Interface;
    using Constants;
    using Interface;

    /// <summary>
    /// Hattrick Entity factory implementation.
    /// </summary>
    internal class EntityFactory : IEntityFactory
    {
        #region Public Methods

        /// <summary>
        /// Gets the corresponding Hattrick entity for the specified file name.
        /// </summary>
        /// <param name="fileName">Hattrick XML file name (from the FileName tag).</param>
        /// <returns>IHattrickEntity object.</returns>
        public IXmlEntity GetEntity(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            IXmlEntity result = null;

            switch (fileName.ToLower())
            {
                case XmlFileName.Avatars:
                    result = new BusinessObjects.Hattrick.Avatars.Root();
                    break;

                case XmlFileName.CheckToken:
                    result = new BusinessObjects.Hattrick.CheckToken.Root();
                    break;

                case XmlFileName.ManagerCompendium:
                    result = new BusinessObjects.Hattrick.ManagerCompendium.Root();
                    break;

                case XmlFileName.Players:
                    result = new BusinessObjects.Hattrick.Players.Root();
                    break;

                case XmlFileName.TeamDetails:
                    result = new BusinessObjects.Hattrick.TeamDetails.Root();
                    break;

                case XmlFileName.WorldDetails:
                    result = new BusinessObjects.Hattrick.WorldDetails.Root();
                    break;

                case XmlFileName.YouthAvatars:
                    result = new BusinessObjects.Hattrick.YouthAvatars.Root();
                    break;

                case XmlFileName.YouthPlayerList:
                    result = new BusinessObjects.Hattrick.YouthPlayerList.Root();
                    break;

                case XmlFileName.YouthTeamDetails:
                    result = new BusinessObjects.Hattrick.YouthTeamDetails.Root();
                    break;

                default:
                    throw new NotImplementedException(
                                  string.Format(
                                             Localization.Messages.NotImplemented,
                                             typeof(IXmlEntity).Name,
                                             fileName));
            }

            result.FileName = fileName;

            return result;
        }

        /// <summary>
        /// Gets the corresponding Hattrick entity for the specified XmlFile.
        /// </summary>
        /// <param name="xmlFile">XmlFile object.</param>
        /// <returns>IHattrickEntity object.</returns>
        public IXmlEntity GetEntity(XmlFile xmlFile)
        {
            IXmlEntity entity = null;

            switch (xmlFile)
            {
                case XmlFile.CheckToken:
                    entity = new BusinessObjects.Hattrick.CheckToken.Root();
                    break;

                case XmlFile.ManagerCompendium:
                    entity = new BusinessObjects.Hattrick.ManagerCompendium.Root();
                    break;

                case XmlFile.Players:
                    entity = new BusinessObjects.Hattrick.Players.Root();
                    break;

                case XmlFile.WorldDetails:
                    entity = new BusinessObjects.Hattrick.WorldDetails.Root();
                    break;

                default:
                    throw new NotImplementedException(
                                  string.Format(
                                             Localization.Messages.NotImplemented,
                                             typeof(IXmlEntity).Name,
                                             xmlFile.ToString()));
            }

            return entity;
        }

        #endregion Public Methods
    }
}