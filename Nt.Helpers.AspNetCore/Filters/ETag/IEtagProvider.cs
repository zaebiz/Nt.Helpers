using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nt.Helpers.AspNetCore.Filters.ETag
{
    public interface IEtagProvider
    {
        Task<string> GetResourceEtag(string resourse);
    }
}
