using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Nt.Helpers.AspNetCore.Models;

namespace Nt.Helpers.AspNetCore.Extensions
{    

    public static class ControllerEx
    {
        static readonly JsonSerializerSettings _defaults = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.None,
            //Converters = new List<JsonConverter> { new DateTimeGlobalConverter() },
            NullValueHandling = NullValueHandling.Ignore,
            // todo NT: Олег просил вернуть пустые массивы
            //ContractResolver = new ContractResolver(true, true)
        };

        public static IActionResult ApiOk<TData>(this Controller controller, TData data)
        {
            return controller.Json(new RtApiResponse<TData>(data), _defaults);
        }

        public static IActionResult ApiError(this Controller controller, string error)
        {
            var response = new RtApiErrorResponse(error);
            return controller.Json(response, _defaults);
        }

        public static IActionResult ApiError(this Controller controller, Exception ex)
        {
            var response = new RtApiErrorResponse(ex);
            return controller.Json(response, _defaults);
        }

        public static IActionResult ApiError(this Controller controller, ModelStateDictionary modelState)
        {
            var errList = GetModelStateErrors(modelState);
            var response = new RtApiErrorResponse(errList);

            return controller.Json(response, _defaults); ;
        }

        /// <summary>
        /// Получить все сообщения об ошибках из ModelState
        /// </summary>
        static List<string> GetModelStateErrors(ModelStateDictionary modelState)
        {
            return modelState
                .SelectMany(x => x.Value.Errors)
                //.Where(y => y.Count > 0)
                .Select(err =>
                {
                    string errorString = "No error message extracted";

                    if (!string.IsNullOrWhiteSpace(err.ErrorMessage))
                        errorString = err.ErrorMessage;
                    else if (!string.IsNullOrWhiteSpace(err.Exception.Message))
                        errorString = err.Exception.Message;

                    return errorString;
                })
                .ToList();
        }
    }
}