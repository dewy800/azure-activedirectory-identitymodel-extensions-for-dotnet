// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Contains information so that logs can be written when needed.
    /// </summary>
    internal class ExceptionDetails
    {
        /// <summary>
        /// Creates an instance of <see cref="ExceptionDetails"/>
        /// </summary>
        public ExceptionDetails(MessageDetails messageDetails, Type exceptionType, StackFrame stackFrame)
        {
            ExceptionType = exceptionType;
            MessageDetails = messageDetails;
            StackFrames.Add(stackFrame);
        }

        /// <summary>
        /// Creates an instance of <see cref="ExceptionDetails"/>
        /// </summary>
        public ExceptionDetails(MessageDetails messageDetails, Type exceptionType, StackFrame stackFrame, Exception innerException)
        {
            ExceptionType = exceptionType;
            InnerException = innerException;
            MessageDetails = messageDetails;
            StackFrames.Add(stackFrame);
        }

        public Exception GetException()
        {
            if (InnerException != null)
                return Activator.CreateInstance(ExceptionType, MessageDetails.Message, InnerException) as Exception;

            return Activator.CreateInstance(ExceptionType, MessageDetails.Message) as Exception;
        }

        public Type ExceptionType { get; }

        public Exception InnerException { get; }

        public MessageDetails MessageDetails { get; }

        public StackTrace StackTrace
        {
            get
            {
#if NET8_0_OR_GREATER
                return new StackTrace(StackFrames);
#else
                return new StackTrace(StackFrames[0]);
#endif
            }
        }

        public IList<StackFrame> StackFrames { get; } = [];
    }
}
