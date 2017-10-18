using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarryPotterShoppingCart
{
    public class ShoppingCart
    {
        public decimal Calculate(Dictionary<string, int> books)
        {
            var result = 0m;
            var price = 100m;
            var currentQuantity = books.Values.Max();

            do
            {
                var count = books.Count(p => p.Value >= currentQuantity);
                var currentPrice = price * count * Discount(count);
                result += currentPrice;

                currentQuantity--;
            } while (currentQuantity > 0);         

            return result;
        }


        private decimal Discount(int count)
        {
            var result = 1m;
            switch (count)
            {
                case 2:
                    result -= 0.05m;
                    break;
                case 3:
                    result -= 0.1m;
                    break;
                case 4:
                    result -= 0.2m;
                    break;
                case 5:
                    result -= 0.25m;
                    break;
            }

            return result;
        }
    }
}
