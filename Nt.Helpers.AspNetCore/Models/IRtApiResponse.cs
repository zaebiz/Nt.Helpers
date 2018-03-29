using System;
using System.Collections.Generic;
using System.Net;

namespace Nt.Helpers.AspNetCore.Models
{
    public interface IRtApiResponse
    {
        HttpStatusCode ResponseCode { get; set; }
        bool IsValid { get; }
        bool IsWarning { get; }

        List<string> ErrorMessage { get; set; }
        List<string> WarningMessage { get; set; }

        string LogEventId { get; set; }

        void AddError(Exception errorException);
        void AddError(string errorMessage);
        //void AddError(string errorMessage, Exception errorException);
    }
}