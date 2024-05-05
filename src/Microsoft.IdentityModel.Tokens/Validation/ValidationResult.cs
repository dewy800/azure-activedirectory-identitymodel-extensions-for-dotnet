// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Contains artifacts obtained when a SecurityToken is validated.
    /// A <see cref="TokenValidationResult"/> returns a collection of <see cref="ValidationResult"/> for each step in the token validation.
    /// </summary>
    public abstract class ValidationResult
    {
        private bool _isValid = false;

        /// <summary>
        /// Creates an instance of <see cref="TokenValidationResult"/>
        /// </summary>
        public ValidationResult()
        {
        }

        /// <summary>
        /// Gets or sets the <see cref="Exception"/> that occurred during validation.
        /// </summary>
        public virtual Exception Exception { get; set; }

        /// <summary>
        /// True if the token was successfully validated, false otherwise.
        /// </summary>
        public bool IsValid
        {
            get
            {
                HasValidOrExceptionWasRead = true;
                return _isValid;
            }
            set
            {
                _isValid = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool HasValidOrExceptionWasRead { get; protected set; }

        /// <summary>
        /// Gets the <see cref="ValidationFailureType"/> indicating why the validation was not satisfied.
        /// This should not be set to null.
        /// </summary>
        public ValidationFailureType ValidationFailureType
        {
            get;
            set;
        } = ValidationFailureType.ValidationNotEvaluated;

        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        public string ValidationMessage { get; }

        internal IList<LogDetails> LogDetails { get; set; } = [];

        internal ExceptionDetails ExceptionDetails { get; set; }
    }
}
