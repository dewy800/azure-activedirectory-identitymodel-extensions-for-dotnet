// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Contains the result of validating the issuer.
    /// The <see cref="TokenValidationResult"/> contains a collection of <see cref="ValidationResult"/> for each step in the token validation.
    /// </summary>
    public class IssuerValidationResult : ValidationResult
    {
        private Exception _exception;

        /// <summary>
        /// Creates an instance of <see cref="IssuerValidationResult"/>
        /// </summary>
        public IssuerValidationResult()
        {
        }

        /// <summary>
        /// Gets the <see cref="Exception"/> that occurred during validation.
        /// </summary>
        public override Exception Exception
        {
            get
            {
                // TODO - how to handle if exception was set?
                if (_exception != null || ExceptionDetails == null)
                    return _exception;

                HasValidOrExceptionWasRead = true;
                _exception = ExceptionDetails.GetException();
                SecurityTokenInvalidIssuerException securityTokenInvalidIssuerException = _exception as SecurityTokenInvalidIssuerException;
                if (securityTokenInvalidIssuerException != null)
                {
                    securityTokenInvalidIssuerException.InvalidIssuer = Issuer;
                    securityTokenInvalidIssuerException.ExceptionDetails = ExceptionDetails;
                    securityTokenInvalidIssuerException.Source = "Microsoft.IdentityModel.Tokens";
                }

                return _exception;
            }
            set
            {
                _exception = value;
            }
        }

        /// <summary>
        /// Gets or sets the issuer that was validated.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Adds a new stack frame to the exception details.
        /// </summary>
        /// <param name="stackFrame"></param>
        public void AddStackFrame(StackFrame stackFrame)
        {
            ExceptionDetails.StackFrames.Add(stackFrame);
        }
    }
}
