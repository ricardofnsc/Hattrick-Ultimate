﻿// -----------------------------------------------------------------------
// <copyright file="GetAuthorizationUrlResponse.cs" company="Hyperar">
//     Copyright (c) Hyperar. All rights reserved.
// </copyright>
// <author>Matías Ezequiel Sánchez</author>
// -----------------------------------------------------------------------
namespace Hyperar.HattrickUltimate.BusinessObjects.OAuth
{
    /// <summary>
    /// GetAuthorization response.
    /// </summary>
    public class GetAuthorizationUrlResponse
    {
        #region Private Fields

        /// <summary>
        /// Authorization URL.
        /// </summary>
        private readonly string authorizationUrl;

        /// <summary>
        /// Request token.
        /// </summary>
        private readonly string token;

        /// <summary>
        /// Request token secret.
        /// </summary>
        private readonly string tokenSecret;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAuthorizationUrlResponse"/> class.
        /// </summary>
        /// <param name="authorizationUrl">Authorization URL generated by Hattrick.</param>
        /// <param name="token">Token key.</param>
        /// <param name="tokenSecret">Token Secret key.</param>
        public GetAuthorizationUrlResponse(string authorizationUrl, string token, string tokenSecret)
        {
            this.authorizationUrl = authorizationUrl;
            this.token = token;
            this.tokenSecret = tokenSecret;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets the authorization URL.
        /// </summary>
        public string AuthorizationUrl
        {
            get
            {
                return this.authorizationUrl;
            }
        }

        /// <summary>
        /// Gets the request token.
        /// </summary>
        public string Token
        {
            get
            {
                return this.token;
            }
        }

        /// <summary>
        /// Gets the request token secret.
        /// </summary>
        public string TokenSecret
        {
            get
            {
                return this.tokenSecret;
            }
        }

        #endregion Public Properties
    }
}