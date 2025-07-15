using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum ProductVariationStatus
    {
        Active = 1, // đang bán
        Inactive = 2, // ngừng bán
        OutOfStock = 3, // hết hàng
    }
}
