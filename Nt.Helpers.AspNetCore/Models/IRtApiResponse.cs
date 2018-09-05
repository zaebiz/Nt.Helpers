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

        /// <summary>
        /// заполнить поля Response на основе исключения
        /// </summary>        
        void AddError(Exception exception);

        /// <summary>
        /// заполнить полe Error в ответе сервера
        /// </summary>
        void AddError(string message, string code = null);

        /// <summary>
        /// Добавить Warning к ответу
        /// </summary>
        void AddWarning(string message, string code = null);
        //void AddError(string errorMessage, Exception errorException);
    }
}