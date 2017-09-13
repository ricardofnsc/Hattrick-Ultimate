﻿//-----------------------------------------------------------------------
// <copyright file="Token.cs" company="Hyperar">
//     Copyright (c) Hyperar. All rights reserved.
// </copyright>
// <author>Matías Ezequiel Sánchez</author>
//-----------------------------------------------------------------------
namespace Hyperar.HattrickUltimate.DataAccess.Database.Mapping
{
    /// <summary>
    /// Token entity mapping definition.
    /// </summary>
    internal class Token : Entity<BusinessObjects.App.Token>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Token" /> class.
        /// </summary>
        internal Token()
        {
            this.RegisterTable();
            this.RegisterProperties();
        }

        /// <summary>
        /// Registers entity properties.
        /// </summary>
        private void RegisterProperties()
        {
            this.Property(e => e.Key)
                .HasColumnName(Constants.ColumnName.Token)
                .HasColumnOrder(2)
                .HasColumnType(Constants.ColumnType.UnicodeChar)
                .HasMaxLength(Constants.ColumnLength.Token)
                .IsRequired();

            this.Property(e => e.Secret)
                .HasColumnName(Constants.ColumnName.TokenSecret)
                .HasColumnOrder(3)
                .HasColumnType(Constants.ColumnType.UnicodeChar)
                .HasMaxLength(Constants.ColumnLength.Token)
                .IsRequired();

            this.Property(e => e.Scope)
                .HasColumnName(Constants.ColumnName.AccessScope)
                .HasColumnOrder(4)
                .HasColumnType(Constants.ColumnType.TinyInteger)
                .IsRequired();
        }

        /// <summary>
        /// Register entity table.
        /// </summary>
        private void RegisterTable()
        {
            this.ToTable(Constants.TableName.Token);
        }
    }
}