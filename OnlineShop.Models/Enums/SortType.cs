using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Models.Enums
{
    public enum SortType
    {
        Default = 0,
        OrderByPriceDesc = 1,
        OrderByPriceAsc = 2,
        GetNewest = 3,
        GetMostSold = 4,
    }
}
