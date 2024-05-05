// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.SignedHttpRequest
{
    /// <summary>
    /// Contains artifacts obtained when a SignedHttpRequest is validated.
    /// </summary>
    public class SignedHttpRequestValidationResult : ValidationResult
    {
        /// <summary>
        /// Gets or sets the access token validation result.
        /// </summary>
        public TokenValidationResult AccessTokenValidationResult { get; set; }

        /// <summary>
        /// Gets or sets SignedHttpRequest in its original encoded form.
        /// </summary>
        public string SignedHttpRequest { get; set; }

        /// <summary>
        /// Gets or sets the validated SignedHttpRequest.
        /// </summary>
        public SecurityToken ValidatedSignedHttpRequest { get; set; }
    }
}
