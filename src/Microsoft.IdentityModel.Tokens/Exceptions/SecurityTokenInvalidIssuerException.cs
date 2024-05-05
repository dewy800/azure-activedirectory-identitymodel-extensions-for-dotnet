// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// This exception is thrown when 'issuer' of a token was not valid.
    /// </summary>
    [Serializable]
    public class SecurityTokenInvalidIssuerException : SecurityTokenValidationException
    {
        [NonSerialized]
        const string _Prefix = "Microsoft.IdentityModel." + nameof(SecurityTokenInvalidIssuerException) + ".";

        [NonSerialized]
        const string _InvalidIssuerKey = _Prefix + nameof(InvalidIssuer);

        [NonSerialized]
        private string _stackTrace;

        /// <summary>
        /// Gets or sets the InvalidIssuer that created the validation exception.
        /// </summary>
        public string InvalidIssuer { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenInvalidIssuerException"/> class.
        /// </summary>
        public SecurityTokenInvalidIssuerException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenInvalidIssuerException"/> class.
        /// </summary>
        /// <param name="message">Addtional information to be included in the exception and displayed to user.</param>
        public SecurityTokenInvalidIssuerException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenInvalidIssuerException"/> class.
        /// </summary>
        /// <param name="message">Addtional information to be included in the exception and displayed to user.</param>
        /// <param name="innerException">A <see cref="Exception"/> that represents the root cause of the exception.</param>
        public SecurityTokenInvalidIssuerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenInvalidIssuerException"/> class.
        /// </summary>
        /// <param name="info">the <see cref="SerializationInfo"/> that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
#if NET8_0_OR_GREATER
        [Obsolete("Formatter-based serialization is obsolete", DiagnosticId = "SYSLIB0051")]
#endif
        protected SecurityTokenInvalidIssuerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            SerializationInfoEnumerator enumerator = info.GetEnumerator();
            while (enumerator.MoveNext())
            {
                switch (enumerator.Name)
                {
                    case _InvalidIssuerKey:
                        InvalidIssuer = info.GetString(_InvalidIssuerKey);
                        break;

                    default:
                        // Ignore other fields.
                        break;
                }
            }
        }

        /// <summary>
        /// Gets the stack trace that is captured when the exception is created.
        /// </summary>
        public override string StackTrace
        {
            get
            {
                if (_stackTrace == null)
                {
                    if (ExceptionDetails == null)
                        return base.StackTrace;
#if NET8_0_OR_GREATER
                    _stackTrace = new StackTrace(ExceptionDetails.StackFrames).ToString();
#else
                    StringBuilder sb = new();
                    foreach (StackFrame frame in ExceptionDetails.StackFrames)
                    {
                        sb.Append(frame.ToString());
                        sb.Append(Environment.NewLine);
                    }

                    _stackTrace = sb.ToString();
#endif
                }

                return _stackTrace;
            }
        }

        /// <summary>
        /// Gets or sets the source of the exception.
        /// </summary>
        public override string Source
        {
            get => base.Source;
            set => base.Source = value;
        }

        internal ExceptionDetails ExceptionDetails
        {
            get; set;
        }

        /// <inheritdoc/>
#if NET8_0_OR_GREATER
        [Obsolete("Formatter-based serialization is obsolete", DiagnosticId = "SYSLIB0051")]
#endif
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            if (!string.IsNullOrEmpty(InvalidIssuer))
                info.AddValue(_InvalidIssuerKey, InvalidIssuer);
        }
    }
}
