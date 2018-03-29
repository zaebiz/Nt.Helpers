using System.Collections.Generic;
using System.Net;

namespace Nt.Helpers.AspNetCore.Models
{
    public class RtApiResponse<TData> : RtApiErrorResponse
    {
        public TData Data { get; set; }


        public RtApiResponse(TData data)
        {
            ResponseCode = HttpStatusCode.OK;
            Data = data;
            WarningMessage = new List<string>();
        }

    }
}
