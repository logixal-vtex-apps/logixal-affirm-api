﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Affirm.Services
{
    public class AffirmConstants
    {
        //public const string Sandbox = "https://sandbox.affirm.com/api/v2/";
        //public const string Live = " https://api.affirm.com/api/v2/";
        //public const string Sandbox = "https://sandbox.affirm.com/api/v1/";
        //public const string Live = " https://api.affirm.com/api/v1/";

        public const string SuccessResponseCode = "authorized";
        public const string TokenUsed = "checkout-token-used";

        public const string PrivateKeyHeader = "X-PROVIDER-API-AppToken";
        public const string PublicKeyHeader = "X-PROVIDER-API-AppKey";
        public const string IsProduction = "X-Vtex-Workspace-Is-Production";

        public const string PaymentFlowAppName = "vtex.affirm-payment";
        public const string RedirectUrl = "/affirm-payment";
        public const string Authorize = "authorize";
        public const string Read = "read";
        public const string Capture = "capture";
        public const string Void = "void";
        public const string Refund = "refund";
        public const string Update = "update";

        public const string AffirmUrlStub = "affirm.com";
        public const string AffirmApiVersion = "api/v1";
        public const string Charges = "charges";
        public const string Transactions = "transactions";

        // https://sandbox.katapult.com/api/v3/application/funding/
        public const string KatapultUrlStub = "katapult.com";
        public const string KatapultApiVersion = "api/v3";
        public const string Application = "application";
        public const string Funding = "funding";

        public const string KatapultIdPrefix = "LS-";

        public const string AlreadyCaptured = "already_captured";

        public const int MinimumDelayToCancel = 3600;

        public class Inbound
        {
            public const string ActionAuthorize = "auth";
            public const string AcceptedResponse = "[accepted]";
        }

        public class Vtex
        {
            public const string Approved = "approved";
            public const string Denied = "denied";
            public const string Undefined = "undefined";
        }
    }
}
