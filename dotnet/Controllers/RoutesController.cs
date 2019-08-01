﻿namespace Affirm.Controllers
{
    using Affirm.Models;
    using Affirm.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;

    public class RoutesController : Controller
    {
        private readonly IAffirmPaymentService _affirmPaymentService;

        public RoutesController(IAffirmPaymentService affirmPaymentService)
        {
            this._affirmPaymentService = affirmPaymentService ?? throw new ArgumentNullException(nameof(affirmPaymentService));
        }

        /// <summary>
        /// https://{{providerApiEndpoint}}/payments
        /// Creates a new payment and/or initiates the payment flow.
        /// </summary>
        /// <param name="createPaymentRequest"></param>
        /// <returns></returns>
        public async Task<IActionResult> CreatePaymentAsync()
        {
            string publicKey = HttpContext.Request.Headers[AffirmConstants.PublicKeyHeader];
            var bodyAsText = await new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            CreatePaymentRequest createPaymentRequest = JsonConvert.DeserializeObject<CreatePaymentRequest>(bodyAsText);
            var paymentResponse = await this._affirmPaymentService.CreatePaymentAsync(createPaymentRequest, publicKey);

            Response.Headers.Add("Cache-Control", "private");

            return Json(paymentResponse);
        }

        /// <summary>
        /// https://{{providerApiEndpoint}}/payments/{{paymentId}}/cancellations
        /// </summary>
        /// <param name="paymentId">VTEX payment ID from this payment</param>
        /// <param name="cancelPaymentRequest"></param>
        /// <returns></returns>
        public async Task<IActionResult> CancelPaymentAsync(string paymentId)
        {
            string privateKey = HttpContext.Request.Headers[AffirmConstants.PrivateKeyHeader];
            string publicKey = HttpContext.Request.Headers[AffirmConstants.PublicKeyHeader];
            bool isLive = Boolean.Parse(HttpContext.Request.Headers[AffirmConstants.IsProduction]);

            var bodyAsText = await new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            CancelPaymentRequest cancelPaymentRequest = JsonConvert.DeserializeObject<CancelPaymentRequest>(bodyAsText);

            if (string.IsNullOrWhiteSpace(privateKey) || string.IsNullOrWhiteSpace(publicKey))
            {
                return BadRequest();
            }
            else
            {
                var cancelResponse = await this._affirmPaymentService.CancelPaymentAsync(cancelPaymentRequest, publicKey, privateKey, isLive);

                return Json(cancelResponse);
            }
        }

        /// <summary>
        /// https://{{providerApiEndpoint}}/payments/{{paymentId}}/settlements
        /// </summary>
        /// <param name="paymentId">VTEX payment ID from this payment</param>
        /// <param name="capturePaymentRequest"></param>
        /// <returns></returns>
        public async Task<IActionResult> CapturePaymentAsync(string paymentId)
        {
            string privateKey = HttpContext.Request.Headers[AffirmConstants.PrivateKeyHeader];
            string publicKey = HttpContext.Request.Headers[AffirmConstants.PublicKeyHeader];
            bool isLive = Boolean.Parse(HttpContext.Request.Headers[AffirmConstants.IsProduction]);

            var bodyAsText = await new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            CapturePaymentRequest capturePaymentRequest = JsonConvert.DeserializeObject<CapturePaymentRequest>(bodyAsText);

            if (string.IsNullOrWhiteSpace(privateKey) || string.IsNullOrWhiteSpace(publicKey))
            {
                return BadRequest();
            }
            else
            {
                var captureResponse = await this._affirmPaymentService.CapturePaymentAsync(capturePaymentRequest, publicKey, privateKey, isLive);

                return Json(captureResponse);
            }
        }

        /// <summary>
        /// https://{{providerApiEndpoint}}/payments/{{paymentId}}/refunds
        /// </summary>
        /// <param name="paymentId">VTEX payment ID from this payment</param>
        /// <param name="refundPaymentRequest"></param>
        /// <returns></returns>
        public async Task<IActionResult> RefundPaymentAsync(string paymentId)
        {
            string privateKey = HttpContext.Request.Headers[AffirmConstants.PrivateKeyHeader];
            string publicKey = HttpContext.Request.Headers[AffirmConstants.PublicKeyHeader];
            bool isLive = Boolean.Parse(HttpContext.Request.Headers[AffirmConstants.IsProduction]);

            var bodyAsText = await new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            RefundPaymentRequest refundPaymentRequest = JsonConvert.DeserializeObject<RefundPaymentRequest>(bodyAsText);

            if (string.IsNullOrWhiteSpace(privateKey) || string.IsNullOrWhiteSpace(publicKey))
            {
                return BadRequest();
            }
            else
            {
                var refundResponse = await this._affirmPaymentService.RefundPaymentAsync(refundPaymentRequest, publicKey, privateKey, isLive);

                return Json(refundResponse);
            }
        }

        /// <summary>
        /// Retrieve stored payment request
        /// </summary>
        /// <param name="paymentIdentifier">Payment GUID</param>
        /// <returns></returns>
        public async Task<IActionResult> GetPaymentRequestAsync(string paymentIdentifier)
        {
            var paymentRequest = await this._affirmPaymentService.GetCreatePaymentRequestAsync(paymentIdentifier);

            Response.Headers.Add("Cache-Control", "private");

            return Json(paymentRequest);
        }

        /// <summary>
        /// After completing the checkout flow and receiving the checkout token, authorize the charge.
        /// Authorizing generates a charge ID that you’ll use to reference the charge moving forward.
        /// You must authorize a charge to fully create it. A charge is not visible in the Read response,
        /// nor in the merchant dashboard until you authorize it.
        /// </summary>
        /// <param name="paymentIdentifier">Payment GUID</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IActionResult> AuthorizeAsync(string paymentIdentifier, string token, string callbackUrl, int orderTotal)
        {
            string privateKey = HttpContext.Request.Headers[AffirmConstants.PrivateKeyHeader];
            string publicKey = HttpContext.Request.Headers[AffirmConstants.PublicKeyHeader];
            bool isLive = Boolean.Parse(HttpContext.Request.Headers[AffirmConstants.IsProduction]);

            if (string.IsNullOrWhiteSpace(privateKey) || string.IsNullOrWhiteSpace(publicKey))
            {
                return BadRequest();
            }
            else
            {
                var paymentRequest = await this._affirmPaymentService.AuthorizeAsync(paymentIdentifier, token, publicKey, privateKey, isLive, callbackUrl, orderTotal);
                Response.Headers.Add("Cache-Control", "private");

                return Json(paymentRequest);
            }
        }

        /// <summary>
        /// Read the charge information, current charge status, and checkout data
        /// </summary>
        /// <param name="paymentIdentifier">Payment GUID</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IActionResult> ReadChargeAsync(string paymentIdentifier)
        {
            string privateKey = HttpContext.Request.Headers[AffirmConstants.PrivateKeyHeader];
            string publicKey = HttpContext.Request.Headers[AffirmConstants.PublicKeyHeader];
            bool isLive = Boolean.Parse(HttpContext.Request.Headers[AffirmConstants.IsProduction]);

            if (string.IsNullOrWhiteSpace(privateKey) || string.IsNullOrWhiteSpace(publicKey))
            {
                return BadRequest();
            }
            else
            {
                var paymentRequest = await this._affirmPaymentService.ReadChargeAsync(paymentIdentifier, publicKey, privateKey, isLive);
                Response.Headers.Add("Cache-Control", "private");

                return Json(paymentRequest);
            }
        }

        public async Task<IActionResult> InboundAsync(string actiontype)
        {
            Console.WriteLine($"InboundAsync action = {actiontype}");

            string responseCode = string.Empty;
            string responseMessage = string.Empty;
            string responseStatusCode = string.Empty;
            string responseBody = string.Empty;

            string privateKey = HttpContext.Request.Headers[AffirmConstants.PrivateKeyHeader];
            string publicKey = HttpContext.Request.Headers[AffirmConstants.PublicKeyHeader];
            bool isLive = true;
            Boolean.TryParse(HttpContext.Request.Headers[AffirmConstants.IsProduction], out isLive);
            var bodyAsText = await new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            InboundRequest inboundRequest = JsonConvert.DeserializeObject<InboundRequest>(bodyAsText);
            dynamic inboundRequestBody = null;
            try
            {
                inboundRequestBody = JsonConvert.DeserializeObject(inboundRequest.requestData.body);
            }
            catch(Exception ex)
            {
                responseMessage = ex.Message;
            }

            string paymentId = inboundRequest.paymentId;
            string requestId = inboundRequest.requestId;

            if(inboundRequestBody == null)
            {
                responseStatusCode = StatusCodes.Status400BadRequest.ToString();
            }
            else if (string.IsNullOrWhiteSpace(privateKey) || string.IsNullOrWhiteSpace(publicKey))
            {
                responseStatusCode = StatusCodes.Status400BadRequest.ToString();
                responseMessage = "Missing keys.";
            }
            else
            {
                switch(actiontype)
                {
                    case AffirmConstants.Inbound.ActionAuthorize:
                        string token = inboundRequestBody.token;
                        string callbackUrl = inboundRequestBody.callbackUrl;
                        int amount = inboundRequestBody.orderTotal;
                        if (string.IsNullOrEmpty(paymentId) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(callbackUrl))
                        {
                            responseStatusCode = StatusCodes.Status400BadRequest.ToString();
                            responseMessage = "Missing parameters.";
                        }
                        else
                        {
                            var paymentRequest = await this._affirmPaymentService.AuthorizeAsync(paymentId, token, publicKey, privateKey, isLive, callbackUrl, amount);
                            Response.Headers.Add("Cache-Control", "private");

                            responseBody = JsonConvert.SerializeObject(paymentRequest);
                            responseStatusCode = StatusCodes.Status200OK.ToString();
                        }

                        break;
                    default:
                        responseStatusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                        responseMessage = $"Action '{actiontype}' is not supported.";
                        break;
                }
            }

            InboundResponse response = new InboundResponse
            {
                code = responseCode,
                message = responseMessage,
                paymentId = paymentId,
                requestId = requestId,
                responseData = new ResponseData
                {
                    body = responseBody,
                    statusCode = responseStatusCode
                }
            };

            return Json(response);
        }

        public async Task<IActionResult> GetAppSettings()
        {
            VtexSettings paymentRequest = await this._affirmPaymentService.GetSettingsAsync();

            return Json(paymentRequest);
        }

        public string PrintHeaders()
        {
            string headers = "--->>> Headers <<<---\n";
            foreach (var header in HttpContext.Request.Headers)
            {
                headers += $"{header.Key}: {header.Value}\n";
            }
            return headers;
        }

        public string PrintHeadersPriv()
        {
            return PrintHeaders();
        }

        public JsonResult PaymentMethods()
        {
            PaymentMethods methods = new PaymentMethods();
            methods.paymentMethods = new System.Collections.Generic.List<string>();
            methods.paymentMethods.Add("Affirm");
            methods.paymentMethods.Add("Promissories");

            Response.Headers.Add("Cache-Control", "private");

            return Json(methods);
        }
    }
}
