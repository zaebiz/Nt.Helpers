using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Nt.Helpers.AspNetCore.Models
{
    public class RtApiErrorResponse : IRtApiResponse
    {
        public HttpStatusCode ResponseCode { get; set; } = HttpStatusCode.BadRequest;
        public bool IsValid => ResponseCode == HttpStatusCode.OK;
        public bool IsWarning => WarningMessage.Any();

        public List<string> ErrorMessage { get; set; } = new List<string>();
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
            AddError(exception.Message);
        }

        public void AddError(string errorMessage)
        {
            ErrorMessage.Add(errorMessage);
        }
    }
}
