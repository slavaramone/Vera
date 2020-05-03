using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Security
{
    public interface IClaimsAccessor
    {
        bool TryGetValue(string type, out string value);
    }
}
