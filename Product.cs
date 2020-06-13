using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromotionEngine
{
    interface IProduct
    {
        string ProductSKUID { get; set; }
        string ProductName { get; set; }
        double ProductPrice { get; set; }
    }
    class Product : IProduct
    {
        public string ProductSKUID { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public string ProductPromoType { get; set; }
        public int ProductPromoUnit { get; set; }// like 3, 2...
        public double ProductPromoPrice { get; set; }// like 130   
        public double ProductDiscount { get; set; }// like 130 
        public int ProductQty { get; set; }// like 3, 2...

    }
}
