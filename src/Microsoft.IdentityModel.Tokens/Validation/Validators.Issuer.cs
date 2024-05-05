// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// AudienceValidator
    /// </summary>
    public static partial class Validators
    {
        /// <summary>
        /// Determines if an issuer found in a <see cref="SecurityToken"/> is valid.
        /// </summary>
        /// <param name="issuer">The issuer to validate</param>
        /// <param name="securityToken">The <see cref="SecurityToken"/> that is being validated.</param>
        /// <param name="validationParameters"><see cref="TokenValidationParameters"/> required for validation.</param>
        /// <returns>The issuer to use when creating the "Claim"(s) in a "ClaimsIdentity".</returns>
        /// <exception cref="ArgumentNullException">If 'validationParameters' is null.</exception>
        /// <exception cref="ArgumentNullException">If 'issuer' is null or whitespace and <see cref="TokenValidationParameters.ValidateIssuer"/> is true.</exception>
        /// <exception cref="SecurityTokenInvalidIssuerException">If <see cref="TokenValidationParameters.ValidIssuer"/> is null or whitespace and <see cref="TokenValidationParameters.ValidIssuers"/> is null.</exception>
        /// <exception cref="SecurityTokenInvalidIssuerException">If 'issuer' failed to matched either <see cref="TokenValidationParameters.ValidIssuer"/> or one of <see cref="TokenValidationParameters.ValidIssuers"/>.</exception>
        /// <remarks>An EXACT match is required.</remarks>
        public static async Task<IssuerValidationResult> ValidateIssuerAsync(
            string issuer,
            SecurityToken securityToken,
            TokenValidationParameters validationParameters )
        {
            if (string.IsNullOrWhiteSpace(issuer))
            {
                return new IssuerValidationResult()
                {
                    ExceptionDetails = new ExceptionDetails(
                     new MessageDetails(
                        LogMessages.IDX10211.AsMemory(),
                        null),
                    typeof(SecurityTokenInvalidIssuerException),
                    new StackFrame(true),
                    null),
                    IsValid = false,
                    ValidationFailureType = ValidationFailureType.NullArgument
                };
            }

            if (validationParameters == null)
                throw LogHelper.LogArgumentNullException(nameof(validationParameters));

            if (securityToken == null)
                throw LogHelper.LogArgumentNullException(nameof(securityToken));

            BaseConfiguration configuration = null;
            if (validationParameters.ConfigurationManager != null)
                configuration = await validationParameters.ConfigurationManager.GetBaseConfigurationAsync(CancellationToken.None).ConfigureAwait(false);

            // Throw if all possible places to validate against are null or empty
            if (string.IsNullOrWhiteSpace(validationParameters.ValidIssuer)
                && validationParameters.ValidIssuers.IsNullOrEmpty()
                && string.IsNullOrWhiteSpace(configuration?.Issuer))
            {
                return new IssuerValidationResult
                {
                    IsValid = false,
                    ValidationFailureType = ValidationFailureType.IssuerValidationFailed
                };
            }

            // TODO - we should distinguish if configuration, TVP.ValidIssuer or TVP.ValidIssuers was used to validate the issuer.
            if (configuration != null)
            {
                if (string.Equals(configuration.Issuer, issuer))
                {
                    // TODO - how and when to log
                    // Logs will have to be passed back to Wilson
                    // so that they can be written to the correct place and in the correct format respecting PII.
                    if (LogHelper.IsEnabled(EventLogLevel.Informational))
                        LogHelper.LogInformation(LogMessages.IDX10236, LogHelper.MarkAsNonPII(issuer));

                    return new IssuerValidationResult
                    {
                        Issuer = issuer,
                        IsValid = true,
                        ValidationFailureType = ValidationFailureType.ValidationSucceeded
                    };
                }
            }

            if (string.Equals(validationParameters.ValidIssuer, issuer))
            {
                return new IssuerValidationResult
                {
                    Issuer = issuer,
                    IsValid = true,
                    ValidationFailureType = ValidationFailureType.ValidationSucceeded
                };
            }

            if (validationParameters.ValidIssuers != null)
            {
                foreach (string str in validationParameters.ValidIssuers)
                {
                    if (string.IsNullOrEmpty(str))
                    {
                        if (LogHelper.IsEnabled(EventLogLevel.Informational))
                            LogHelper.LogInformation(LogMessages.IDX10262);

                        continue;
                    }

                    if (string.Equals(str, issuer))
                    {
                        if (LogHelper.IsEnabled(EventLogLevel.Informational))
                            LogHelper.LogInformation(LogMessages.IDX10236, LogHelper.MarkAsNonPII(issuer));

                        return new IssuerValidationResult
                        {
                            Issuer = issuer,
                            IsValid = true,
                            ValidationFailureType = ValidationFailureType.ValidationSucceeded
                        };
                    }
                }
            }

            IssuerValidationResult result = new()
            {
                ExceptionDetails = new ExceptionDetails(
                    new MessageDetails(
                        LogMessages.IDX10205.AsMemory(),
                        LogHelper.MarkAsNonPII(issuer),
                        LogHelper.MarkAsNonPII(validationParameters.ValidIssuer ?? "null"),
                        LogHelper.MarkAsNonPII(Utility.SerializeAsSingleCommaDelimitedString(validationParameters.ValidIssuers)),
                        LogHelper.MarkAsNonPII(configuration?.Issuer)),
                    typeof(SecurityTokenInvalidIssuerException),
                    new StackFrame(true)),
                Issuer = issuer,
                IsValid = false,
                ValidationFailureType = ValidationFailureType.IssuerValidationFailed
            };

            return result;
        }
    }
}
