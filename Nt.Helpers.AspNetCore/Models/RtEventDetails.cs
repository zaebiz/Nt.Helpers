using System;
using System.Collections.Generic;
using System.Text;

namespace Nt.Helpers.AspNetCore.Models
{
    public class RtEventDetails
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Target { get; set; }
        public string LogEventId { get; set; }
    }
}
