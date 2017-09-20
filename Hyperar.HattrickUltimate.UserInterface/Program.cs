﻿//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Hyperar">
//     Copyright (c) Hyperar. All rights reserved.
// </copyright>
// <author>Matías Ezequiel Sánchez</author>
//-----------------------------------------------------------------------
namespace Hyperar.HattrickUltimate.UserInterface
{
    using System;
    using System.IO;
    using System.Windows.Forms;
    using SimpleInjector.Lifestyles;

    /// <summary>
    /// Application entry point.
    /// </summary>
    internal static class Program
    {
        #region Private Methods

        /// <summary>
        /// Gets the best LocalDb instance.
        /// </summary>
        private static void GetDatabaseInstance()
        {
            try
            {
                string localDbInstance = DatabaseUtils.GetLocalDbInstance();

                if (string.IsNullOrWhiteSpace(localDbInstance))
                {
                    throw new Exception(Localization.Strings.LocalDbInstanceNotFound);
                }

                AppDomain.CurrentDomain.SetData("LocalDbInstance", localDbInstance);
            }
            catch (Exception ex)
            {
                throw new Exception(Localization.Strings.CannotRetrieveLocalDbInstance, ex);
            }
        }

        /// <summary>
        /// Gets the user selected database folder.
        /// </summary>
        private static void GetDataFolder()
        {
            try
            {
                string dataFolder = Properties.Settings.Default.DataFolder;

                if (string.IsNullOrWhiteSpace(dataFolder))
                {
                    using (var form = ApplicationObjects.Container.GetInstance<FormDataFolder>())
                    {
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            dataFolder = form.DataFolder;

                            if (!Directory.Exists(dataFolder))
                            {
                                Directory.CreateDirectory(dataFolder);
                            }
                        }
                        else
                        {
                            throw new Exception(Localization.Strings.DataFolderNotSet);
                        }
                    }
                }

                AppDomain.CurrentDomain.SetData("DataDirectory", dataFolder);
            }
            catch (Exception ex)
            {
                throw new Exception(Localization.Strings.CannotSetDataFolder, ex);
            }
        }

        /// <summary>
        /// Initializes the database prerequisites.
        /// </summary>
        private static void Initialize()
        {
            GetAppName();
            GetDatabaseInstance();
            GetDataFolder();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ApplicationObjects.RegisterContainer();

            using (var scope = ThreadScopedLifestyle.BeginScope(ApplicationObjects.Container))
            {
                Initialize();
                Application.Run(ApplicationObjects.Container.GetInstance<FormMain>());
            }
        }

        /// <summary>
        /// Gets the App Name.
        /// </summary>
        private static void GetAppName()
        {
            string appName = $"{Application.ProductName} v{Application.ProductVersion}";

#if DEBUG
            appName += " [DEBUG]";
#endif

            AppDomain.CurrentDomain.SetData("AppName", appName);
        }

        #endregion Private Methods
    }
}