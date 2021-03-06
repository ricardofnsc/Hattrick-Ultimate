﻿//-----------------------------------------------------------------------
// <copyright file="HattrickEntity.cs" company="Hyperar">
//     Copyright (c) Hyperar. All rights reserved.
// </copyright>
// <author>Matías Ezequiel Sánchez</author>
//-----------------------------------------------------------------------
namespace Hyperar.HattrickUltimate.DataAccess.Database.Mapping
{
    using BusinessObjects.App;
    using BusinessObjects.App.Interface;
    using Constants;

    /// <summary>
    /// IHattrickEntity base mapping.
    /// </summary>
    /// <typeparam name="TEntity">IEntity entity.</typeparam>
    internal class HattrickEntity<TEntity> : Entity<TEntity> where TEntity : HattrickEntityBase, IEntity, IHattrickEntity
    {
        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HattrickEntity{TEntity}"/> class.
        /// </summary>
        internal HattrickEntity()
        {
            this.Property(p => p.HattrickId)
                .HasColumnName(ColumnName.HattrickId)
                .HasColumnOrder(1)
                .HasColumnType(ColumnType.BigInt)
                .IsRequired();
        }

        #endregion Internal Constructors
    }
}