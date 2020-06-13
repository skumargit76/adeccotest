using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromotionEngine
{
    class Program
    {
        static List<Product> myProd;
        static void Main(string[] args)
        {
            //Product detail can be load from Database.
            myProd = LoadProductDetails();
            //Testing starts
            IDictionary<string, int> order;
            //Senario A     
            order = new Dictionary<string, int>();
            order.Add("A", 1);
            order.Add("B", 1);
            order.Add("C", 1);
            var senarioA = CalculateOrderValue(order);

            //Senario B        
            order = new Dictionary<string, int>();
            order.Add("A", 5);
            order.Add("B", 5);
            order.Add("C", 1);
            var senarioB = CalculateOrderValue(order);
            //Senario C        
            order = new Dictionary<string, int>();
            order.Add("A", 3);
            order.Add("B", 5);
            order.Add("C", 1);
            order.Add("D", 1);
            var senarioC = CalculateOrderValue(order);

            //Senario D      
            order = new Dictionary<string, int>();
            order.Add("A", 3);//130
            order.Add("B", 5);//45 +45 +30
            order.Add("C", 3);//
            order.Add("D", 1);//30 + 2 * 20 = 320
            var senarioD = CalculateOrderValue(order);

            //Senario E     
            order = new Dictionary<string, int>();
            order.Add("A", 3);//130
            order.Add("B", 5);//45 +45 +30
            order.Add("C", 1);//
            order.Add("D", 3);//30 + 2 * 15 = 310
            var senarioE = CalculateOrderValue(order);

        }
        #region "Product detail"
        static List<Product> LoadProductDetails()
        {
            Product prod1;
            List<Product> ProdList = new List<Product>();
            // Add A
            prod1 = new Product();
            prod1.ProductSKUID = "A";
            prod1.ProductPrice = 50.00;
            prod1.ProductPromoType = "3A";
            prod1.ProductPromoUnit = 3;
            prod1.ProductPromoPrice = 130;
            //
            ProdList.Add(prod1);

            // Add B
            prod1 = new Product();
            prod1.ProductSKUID = "B";
            prod1.ProductPrice = 30.00;
            prod1.ProductPromoType = "2B";
            prod1.ProductPromoUnit = 2;
            prod1.ProductPromoPrice = 45;
            //
            ProdList.Add(prod1);
            // Add C
            prod1 = new Product();
            prod1.ProductSKUID = "C";
            prod1.ProductPrice = 20.00;
            prod1.ProductPromoType = "CD";
            prod1.ProductPromoUnit = 0;
            prod1.ProductPromoPrice = 30;
            //
            ProdList.Add(prod1);
            // Add D
            prod1 = new Product();
            prod1.ProductSKUID = "D";
            prod1.ProductPrice = 15;
            prod1.ProductPromoType = "CD";
            prod1.ProductPromoUnit = 0;
            prod1.ProductPromoPrice = 30;
            //
            ProdList.Add(prod1);
            //
            return ProdList;
        }
        #endregion
        #region "Calculation"
        static double CalculateOrderValue(IDictionary<string, int> MyOrder)
        {
            double TotalPrice = 0.0;

            foreach (var item in MyOrder)
            {
                if (item.Key == "A")
                {
                    var qty = item.Value;
                    var AValue = CalculateOrderValueForA(qty);
                    TotalPrice += AValue;
                }
                if (item.Key == "B")
                {
                    var qty = item.Value;
                    var BValue = CalculateOrderValueForB(qty);
                    TotalPrice += BValue;
                }
                if (item.Key == "C")
                {
                    myProd[2].ProductQty = item.Value;
                }
                if (item.Key == "D")
                {
                    myProd[3].ProductQty = item.Value;
                }

            }
            var CDValue = CalculateOrderValueForCD();
            TotalPrice += CDValue;
            return TotalPrice;
        }
        static double CalculateOrderValueForA(int qty)
        {
            double ValueForA = 0;
            if (qty == 0)
                return 0;
            if (qty < myProd[0].ProductPromoUnit)
                return qty * myProd[0].ProductPrice;
            if (qty >= myProd[0].ProductPromoUnit)
            {
                var qty1 = qty / myProd[0].ProductPromoUnit;
                var qty2 = qty % myProd[0].ProductPromoUnit;
                ValueForA = qty1 * myProd[0].ProductPromoPrice;
                ValueForA += qty2 * myProd[0].ProductPrice;
            }

            //return
            return ValueForA;
        }
        static double CalculateOrderValueForB(int qty)
        {
            double ValueForB = 0;
            if (qty == 0)
                return 0;
            if (qty < myProd[1].ProductPromoUnit)
                return qty * myProd[1].ProductPrice;
            if (qty >= myProd[1].ProductPromoUnit)
            {
                var qty1 = qty / myProd[1].ProductPromoUnit;
                var qty2 = qty % myProd[1].ProductPromoUnit;
                ValueForB = qty1 * myProd[1].ProductPromoPrice;
                ValueForB += qty2 * myProd[1].ProductPrice;
            }

            //return
            return ValueForB;
        }
        static double CalculateOrderValueForCD()
        {
            double ValueForCD = 0;
            int C_qty = myProd[2].ProductQty;
            int D_qty = myProd[3].ProductQty;
            //both not ordered
            if (C_qty == 0 && D_qty == 0)
                return 0;
            //only C ordered and D null
            if (C_qty > 0 && D_qty == 0)
                return C_qty * myProd[2].ProductPrice;
            //only D ordered and C null
            if (C_qty == 0 && D_qty > 0)
                return D_qty * myProd[3].ProductPrice;
            //if both ordered
            if (C_qty > 0 && D_qty > 0)
            {
                //if both have same qty
                if (C_qty == D_qty)
                    return C_qty * myProd[2].ProductPromoPrice;
                else
                {
                    //var sumCD = C_qty + D_qty;
                    //var div1 = sumCD / 2;
                    //var div2 = sumCD % 2;
                    if (C_qty > D_qty)//If C qty is more than D
                    {
                        var qty1 = C_qty - D_qty;
                        ValueForCD += D_qty * myProd[2].ProductPromoPrice;
                        ValueForCD += qty1 * myProd[2].ProductPrice;
                    }
                    else
                    {
                        var qty1 = D_qty - C_qty;
                        ValueForCD += C_qty * myProd[3].ProductPromoPrice;
                        ValueForCD += qty1 * myProd[3].ProductPrice;
                    }

                }

            }


            //return
            return ValueForCD;
        }
        #endregion

    }
}
