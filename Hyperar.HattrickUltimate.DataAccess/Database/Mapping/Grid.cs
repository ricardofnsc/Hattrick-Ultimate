﻿//-----------------------------------------------------------------------
// <copyright file="Grid.cs" company="Hyperar">
//     Copyright (c) Hyperar. All rights reserved.
// </copyright>
// <author>Matías Ezequiel Sánchez</author>
//-----------------------------------------------------------------------
namespace Hyperar.HattrickUltimate.DataAccess.Database.Mapping
{
    using Constants;
    using Interface;

    /// <summary>
    /// Grid entity mapping implementation.
    /// </summary>
    internal class Grid : Entity<BusinessObjects.App.Grid>, IMapping
    {
        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Grid"/> class.
        /// </summary>
        internal Grid()
        {
            this.RegisterTable();
            this.RegisterProperties();
            this.RegisterRelationships();
        }

        #endregion Internal Constructors

        #region Public Methods

        /// <summary>
        /// Registers property column mapping.
        /// </summary>
        public void RegisterProperties()
        {
            this.Property(p => p.Name)
                .HasColumnName(ColumnName.Name)
                .HasColumnOrder(1)
                .HasColumnType(ColumnType.UnicodeVarChar)
                .HasMaxLength(ColumnLength.ShortText)
                .IsRequired();

            this.Property(p => p.GridType)
                .HasColumnName(ColumnName.GridType)
                .HasColumnType(ColumnType.TinyInt)
                .IsRequired();
        }

        /// <summary>
        /// Register entity relationships.
        /// </summary>
        public void RegisterRelationships()
        {
            this.HasMany(r => r.GridColumns)
                .WithRequired(r => r.Grid)
                .HasForeignKey(r => r.GridId)
                .WillCascadeOnDelete(false);

            this.HasMany(r => r.GridLayouts)
                .WithRequired(r => r.Grid)
                .HasForeignKey(r => r.GridId)
                .WillCascadeOnDelete(false);
        }

        /// <summary>
        /// Register entity table mapping.
        /// </summary>
        public void RegisterTable()
        {
            this.ToTable(TableName.Grid, SchemaName.Application);
        }

        #endregion Public Methods
    }
}