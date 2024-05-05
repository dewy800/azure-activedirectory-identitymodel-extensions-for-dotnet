// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    internal class MessageDetails
    {
        string _message;

        public MessageDetails(ReadOnlyMemory<char> messageId, params object[] parameters)
        {
            MessageId = messageId;
            Parameters = parameters;
        }

        public string Message
        {
            get
            {
                _message ??= LogHelper.FormatInvariant(MessageId.ToString(), Parameters);
                return _message;
            }
        }

        public ReadOnlyMemory<char> MessageId { get; }

        public object[] Parameters { get; }
    }
}
