// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.Abstractions;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Contains information so that logs can be written when needed.
    /// </summary>
    internal class LogDetails
    {
        /// <summary>
        /// Creates an instance of <see cref="LogDetails"/>
        /// </summary>
        public LogDetails(MessageDetails messageDetails, EventLogLevel eventLogLevel)
        {
            EventLogLevel = eventLogLevel;
            MessageDetails = messageDetails;
        }

        public EventLogLevel EventLogLevel { get; }

        public MessageDetails MessageDetails { get; }
    }
}
