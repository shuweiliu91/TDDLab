using System;
using System.Collections.Generic;
using System.Linq;
using HarryPotterShoppingCart.Enums;

namespace HarryPotterShoppingCart
{
    public class ShoppingCart
    {
        /// <summary>
        /// 檢查購物數量並計算總價格：購物車價格 * 會員折扣 + 運費
        /// </summary>
        /// <param name="books">購物車商品(哈利波特們)</param>
        /// <param name="memberType">會員類型</param>
        /// <param name="delieveryType">運送類型</param>
        /// <returns>總價格：購物車價格 * 會員折扣 + 運費</returns>
        public decimal CalculateShoppingCart(Dictionary<string, int> books, MemberType memberType, DelieveryType delieveryType)
        {
            decimal totalPrice = 0m;
            decimal delieveryPrice = 0m;

            int maxCount = books.Values.Max();
            int totalCount = books.Values.Sum();

            //// 檢查 功能
            //// 每一集最多買 5 本，總共不能超過 30 本。
            Check(maxCount, totalCount);

            //// 計算購物車價格
            totalPrice = Calculate(books);

            //// 折扣 功能
            //// A.VIP 滿 500 打八折。
            //// B.一般會員滿 1000 元且超過 3 件，打八五折。
            totalPrice = MemberDiscount(totalPrice, memberType, totalCount);

            //// 物流 功能
            //// A. 5 件以上只能用黑貓，運費 100 元。
            //// B. 5 件以下可以用黑貓或郵局，郵局運費 50 元。
            delieveryPrice = Delievery(totalCount, delieveryType);

            //// 金流 功能
            //// 寫交易記錄到檔案。
            Log(books, memberType, delieveryType);

            return totalPrice + delieveryPrice;
        }

        /// <summary>
        /// 計算購物車價格
        /// </summary>
        /// <param name="books">購物車商品(哈利波特們)</param>
        /// <returns>實際價格</returns>
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

        /// <summary>
        /// 計算折扣
        /// </summary>
        /// <param name="count">購買種類</param>
        /// <returns>折扣</returns>
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

        /// <summary>
        /// 檢查購物車
        /// </summary>
        /// <param name="maxCount">最多一集買幾本</param>
        /// <param name="totalCount">購買數量</param>
        /// <returns>檢查結果</returns>
        public bool Check(int maxCount, int totalCount)
        {
            if (maxCount > 5)
                throw new Exception("每一集最多買5本。");

            if (totalCount > 30)
                throw new Exception("總共不能超過30本。");

            return true;
        }

        /// <summary>
        /// 計算會員折扣
        /// </summary>
        /// <param name="totalPrice">本次購物金額</param>
        /// <param name="type">會員類型</param>
        /// <param name="count">購買數量</param>
        /// <returns>折扣後總價</returns>
        public decimal MemberDiscount(decimal totalPrice, MemberType type, int count)
        {
            if (type == MemberType.VIP && totalPrice >= 500)
            {
                totalPrice *= 0.8m;
            }
            else if (type == MemberType.Normal && totalPrice >= 1000 && count > 3)
            {
                totalPrice *= 0.85m;
            }

            return totalPrice;
        }

        /// <summary>
        /// 物流檢查
        /// </summary>
        /// <param name="totalCount">購買數量</param>
        /// <param name="type">運送類型</param>
        /// <returns>運費</returns>
        public decimal Delievery(int totalCount, DelieveryType type)
        {
            if (totalCount > 5 && type == DelieveryType.Mail)
                throw new Exception("5件以上只能用黑貓。");

            decimal delieveryPrice = 0m;
            if (type == DelieveryType.Mail)
            {
                delieveryPrice = 50m;
            }
            else if (type == DelieveryType.BlackCat)
            {
                delieveryPrice = 100m;
            }

            return delieveryPrice;
        }

        /// <summary>
        /// 寫交易記錄到檔案
        /// </summary>
        /// <param name="books">購物車商品(哈利波特們)</param>
        /// <param name="memberType">會員類型</param>
        /// <param name="delieveryType">運送類型</param>
        public void Log(Dictionary<string, int> books, MemberType memberType, DelieveryType delieveryType)
        {

        }
    }
}
