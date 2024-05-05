// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Json.Tests;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Validation.Tests
{
    public class IssuerValidationResultTests
    {
        [Theory, MemberData(nameof(IssuerValdationResultsTestCases), DisableDiscoveryEnumeration = true)]
        public async Task IssuerValidatorAsyncTests(IssuerValidationResultsTheoryData theoryData)
        {
            CompareContext context = TestUtilities.WriteHeader($"{this}.IssuerValidatorAsyncTests", theoryData);

            try
            {
                IssuerValidationResult issuerValidationResult = await Validators.ValidateIssuerAsync(
                    theoryData.Issuer,
                    theoryData.SecurityToken,
                    theoryData.ValidationParameters,
                    theoryData.Configuration).ConfigureAwait(false);

                theoryData.ExpectedException.ProcessException(issuerValidationResult.Exception, context);
                IdentityComparer.AreIssuerValidationResultsEqual(
                    issuerValidationResult,
                    theoryData.IssuerValidationResult,
                    "runResult",
                    "theoryData",
                    "at Microsoft.IdentityModel.Tokens.",
                    context);
            }
            catch (SecurityTokenInvalidIssuerException ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<IssuerValidationResultsTheoryData> IssuerValdationResultsTestCases
        {
            get
            {
                TheoryData<IssuerValidationResultsTheoryData> theoryData = new();

                string validIssuer = Guid.NewGuid().ToString();
                string issClaim = Guid.NewGuid().ToString();
                theoryData.Add(new IssuerValidationResultsTheoryData("Invalid_Issuer")
                {
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX10205:"),
                    Issuer = issClaim,
                    IssuerValidationResult = new IssuerValidationResult
                    {
                        ExceptionDetails = new ExceptionDetails(
                            new MessageDetails(
                                LogMessages.IDX10205.AsMemory(),
                                LogHelper.MarkAsNonPII(issClaim),
                                LogHelper.MarkAsNonPII(validIssuer),
                                LogHelper.MarkAsNonPII(Utility.SerializeAsSingleCommaDelimitedString(null)),
                                LogHelper.MarkAsNonPII(null)),
                            typeof(SecurityTokenInvalidIssuerException),
                            new StackFrame(true)),
                        Issuer = issClaim,
                        IsValid = false,
                        ValidationFailureType = ValidationFailureType.IssuerValidationFailed
                    },
                    IsValid = false,
                    SecurityToken = JsonUtilities.CreateUnsignedJsonWebToken(JwtRegisteredClaimNames.Iss, issClaim),
                    ValidationParameters = new TokenValidationParameters { ValidIssuer = validIssuer }
                });

                theoryData.Add(new IssuerValidationResultsTheoryData("NULL_Issuer")
                {
                    ExpectedException = ExpectedException.SecurityTokenInvalidIssuerException("IDX10211:"),
                    IssuerValidationResult = new IssuerValidationResult
                    {
                        ExceptionDetails = new ExceptionDetails(
                            new MessageDetails(
                                LogMessages.IDX10211.AsMemory(),
                                LogHelper.MarkAsNonPII(issClaim),
                                LogHelper.MarkAsNonPII(validIssuer),
                                LogHelper.MarkAsNonPII(Utility.SerializeAsSingleCommaDelimitedString(null)),
                                LogHelper.MarkAsNonPII(null)),
                            typeof(SecurityTokenInvalidIssuerException),
                            new StackFrame(true)),
                        IsValid = false,
                        ValidationFailureType = ValidationFailureType.NullArgument
                    },
                    IsValid = false,
                    SecurityToken = JsonUtilities.CreateUnsignedJsonWebToken(JwtRegisteredClaimNames.Iss, issClaim),
                    ValidationParameters = new TokenValidationParameters(),
                });

                return theoryData;
            }
        }
    }

    public class IssuerValidationResultsTheoryData : TheoryDataBase
    {
        public IssuerValidationResultsTheoryData(string testId) : base(testId)
        {
        }

        public BaseConfiguration Configuration { get; set; }

        public string Issuer { get; set; }

        public IssuerValidationResult IssuerValidationResult { get; set; }

        public bool IsValid { get; set; }

        public SecurityToken SecurityToken { get; set; }

        public TokenValidationParameters ValidationParameters { get; set; }

        public ValidationFailureType ValidationFailureType { get; set; }
    }
}
