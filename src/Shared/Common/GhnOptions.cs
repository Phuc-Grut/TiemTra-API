using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Common
{
    public class GhnOptions
    {
        public string BaseUrl { get; set; } = "";
        public string Token { get; set; } = "";
        public string ShopId { get; set; } = "";
        public int FromDistrictId { get; set; }
        public string FromWardCode { get; set; } = "";
    }
}
