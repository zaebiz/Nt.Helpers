using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Nt.Helpers.AspNetCore.Extensions;

namespace Nt.Helpers.AspNetCore.Models
{
    public class RtApiErrorResponse : IRtApiResponse
    {
        public HttpStatusCode ResponseCode { get; set; } = HttpStatusCode.BadRequest;
        public bool IsValid => ResponseCode == HttpStatusCode.OK;
        public bool IsWarning => WarningMessage.Any();

        public RtEventDetails Error { get; } = new RtEventDetails();
        public List<RtEventDetails> Warnings { get; } = new List<RtEventDetails>();

        [Obsolete]
        public List<string> ErrorMessage { get; set; } = new List<string>();
        [Obsolete]
        public List<string> WarningMessage { get; set; } = new List<string>();
        
        public string LogEventId { get; set; }

        public RtApiErrorResponse()
        { }

        public RtApiErrorResponse(Exception exception)
        {
            AddError(exception);
        }

        public RtApiErrorResponse(string errMsg)
        {
            AddError(errMsg);
        }

        public RtApiErrorResponse(List<string> errMsgList)
        {
            ErrorMessage.AddRange(errMsgList);
        }

        public void AddError(Exception exception)
        {
            var message = exception.TryLast().Message;

            ErrorMessage.Add(message);
            Error.Code = exception.GetType().Name.Split('.').Last();
            Error.Message = message;

            if (exception.Data.Keys.Count > 0)
            {
                if (exception.Data["code"] != null)
                {
                    Error.Code = exception.Data["code"].ToString();
                }
                if (exception.Data["message"] != null)
                {
                    Error.Message = exception.Data["message"].ToString();
                }
                if (exception.Data["target"] != null)
                {
                    Error.Target = exception.Data["target"].ToString();
                }
                if (exception.Data["logEventId"] != null)
                {
                    Error.LogEventId = exception.Data["logEventId"].ToString();
                }
            }
            
        }

        public void AddError(string message, string code = null)
        {
            ErrorMessage.Add(message);

            Error.Code = code ?? "application error";
            Error.Message = message;
        }

        public void AddWarning(string message, string code = null)
        {
            WarningMessage.Add(message);

            Warnings.Add(new RtEventDetails()
            {
                Code = code ?? "warning",
                Message = message
            });
            
        }
    }
}
