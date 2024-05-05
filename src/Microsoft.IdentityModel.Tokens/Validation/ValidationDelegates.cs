// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Definition for IssuerValidator.
    /// </summary>
    /// <param name="issuer">The issuer to validate.</param>
    /// <param name="securityToken">The <see cref="SecurityToken"/> that is being validated.</param>
    /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
    /// <param name="baseConfiguration"></param>
    /// <returns>A <see cref="IssuerValidationResult"/>that has the result of validating the issuer.</returns>
    /// <remarks>This delegate is not expected to throw.</remarks>
    public delegate Task<IssuerValidationResult> IssuerValidator80Async(string issuer, SecurityToken securityToken, TokenValidationParameters validationParameters, BaseConfiguration baseConfiguration);
}
