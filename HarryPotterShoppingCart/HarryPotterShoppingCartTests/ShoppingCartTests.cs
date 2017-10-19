using HarryPotterShoppingCart;
using Xunit;
using System.Collections.Generic;
using HarryPotterShoppingCart.Enums;

namespace HarryPotterShoppingCart.Tests
{
    public class ShoppingCartTests
    {
        [Theory]
        [MemberData(nameof(CalculateDatas))]
        public void CalculateTest(Dictionary<string, int> books, decimal expected)
        {
            // Arrange
            var actual = 0m;
            var shoppingCart = new ShoppingCart();

            // Act
            actual = shoppingCart.Calculate(books);

            // Arrange
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> CalculateDatas
        {
            get
            {
                return new[]
                {
                    //// 買一本書
                    new object[]
                    {
                        new Dictionary<string, int>() { { "1", 1 } },
                        100m
                    },
                    //// 買兩本不同的書，會有5%的折扣
                    new object[]
                    {
                        new Dictionary<string, int>() { { "1", 1 }, { "2", 1 } },
                        190m
                    },
                    //// 買三本不同的書，會有10%的折扣
                    new object[]
                    {
                        new Dictionary<string, int>() { { "1", 1 }, { "2", 1 }, { "3", 1 } },
                        270m
                    },
                    //// 買四本不同的書，會有20%的折扣
                    new object[]
                    {
                        new Dictionary<string, int>() { { "1", 1 }, { "2", 1 }, { "3", 1 }, { "4", 1 } },
                        320m
                    },
                    //// 買五本不同的書，會有25%的折扣
                    new object[]
                    {
                        new Dictionary<string, int>() { { "1", 1 }, { "2", 1 }, { "3", 1 }, { "4", 1 }, { "5", 1 } },
                        375m
                    },
                    //// 買了四本書，其中三本不同，第四本是重複的，那麼那三本將享有10%的折扣
                    new object[]
                    {
                        new Dictionary<string, int>() { { "1", 1 }, { "2", 1 }, { "3", 2 } },
                        370m
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(ShoppingCartDatas))]
        public void CalculateShoppingCartTest(Dictionary<string, int> books, MemberType memberType, DelieveryType delieveryType, decimal expected)
        {
            // Arrange
            var actual = 0m;
            var shoppingCart = new ShoppingCart();

            // Act
            actual = shoppingCart.CalculateShoppingCart(books, memberType, delieveryType);

            // Arrange
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> ShoppingCartDatas
        {
            get
            {
                return new[]
                {
                    //// VIP買1本書，使用黑貓須外加運費100，共200元。
                    new object[]
                    {
                        new Dictionary<string, int>() { { "1", 1 } },
                        MemberType.VIP,
                        DelieveryType.BlackCat,
                        200m
                    },
                    //// VIP買兩本不同的書各三本，會有5%的折扣，加上VIP滿500打八折，使用黑貓須外加運費100，共556元。
                    new object[]
                    {
                        new Dictionary<string, int>() { { "1", 3 }, { "2", 3 } },
                         MemberType.VIP,
                        DelieveryType.BlackCat,
                        556m
                    },
                    //// 一般會員買五本不同的書各三本，會有25%的折扣，加上一般會員滿千打八五折，使用黑貓須外加運費100，共1056.25元。
                    new object[]
                    {
                        new Dictionary<string, int>() { { "1", 3 }, { "2", 3 }, { "3", 3 }, { "4", 3 }, { "5", 3 } },
                        MemberType.Normal,
                        DelieveryType.BlackCat,
                        1056.25m
                    },
                    //// 一般會員買了四本書，其中三本不同，第四本是重複的，那麼那三本將享有10%的折扣，使用黑貓須外加運費100，共470元。
                    new object[]
                    {
                        new Dictionary<string, int>() { { "1", 1 }, { "2", 1 }, { "3", 2 } },
                        MemberType.Normal,
                        DelieveryType.BlackCat,
                        470m
                    },
                    //// VIP買1本書，使用郵局須外加運費50，共150元。
                    new object[]
                    {
                        new Dictionary<string, int>() { { "1", 1 } },
                        MemberType.VIP,
                        DelieveryType.Mail,
                        150m
                    },
                    //// 一般會員買了四本書，其中三本不同，第四本是重複的，那麼那三本將享有10%的折扣，使用郵局須外加運費50，共420元。
                    new object[]
                    {
                        new Dictionary<string, int>() { { "1", 1 }, { "2", 1 }, { "3", 2 } },
                        MemberType.Normal,
                        DelieveryType.Mail,
                        420m
                    },
                };
            }
        }

        [Theory]
        //// 每一集最多買 5 本，總共 30 本。
        [InlineData(5, 30, true)]
        //// 每一集最多買 6 本，總共 30 本。
        [InlineData(6, 10, false)]
        //// 每一集最多買 1 本，總共 31 本。
        [InlineData(1, 31, false)]
        public void CheckTest(int maxCount, int totalCount, bool expected)
        {
            try
            {
                // Arrange
                var actual = true;
                var shoppingCart = new ShoppingCart();

                // Act
                actual = shoppingCart.Check(maxCount, totalCount);

                // Arrange
                Assert.Equal(expected, actual);
            }
            catch
            {
                // Arrange
                Assert.False(expected);
            }
        }

        [Theory]
        //// VIP 滿 500 打八折。
        [InlineData(500, MemberType.VIP, 5, 400)]
        //// VIP 滿 300 不打折。
        [InlineData(300, MemberType.VIP, 3, 300)]
        //// 一般會員滿 1000 元且超過 3 件，打八五折。
        [InlineData(1000, MemberType.Normal, 4, 850)]
        //// 一般會員滿 1000 元但未超過 3 件，不打折。
        [InlineData(1000, MemberType.Normal, 3, 1000)]
        //// 一般會員未滿 1000 元但超過 3 件，不打折。
        [InlineData(500, MemberType.Normal, 4, 500)]
        //// 一般會員未滿 1000 元也未超過 3 件，不打折。
        [InlineData(200, MemberType.Normal, 2, 200)]
        public void MemberDiscountTest(decimal totalPrice, MemberType type, int count, decimal expected)
        {
            // Arrange
            var actual = 0m;
            var shoppingCart = new ShoppingCart();

            // Act
            actual = shoppingCart.MemberDiscount(totalPrice, type, count);

            // Arrange
            Assert.Equal(expected, actual);
        }

        [Theory]
        //// 5 件以上只能用黑貓，運費 100 元。
        [InlineData(5, DelieveryType.BlackCat, 100, true)]
        //// 5 件以上只能用黑貓，不能使用郵局。
        [InlineData(6, DelieveryType.Mail, 0, false)]
        //// 5 件以下可以用郵局，運費 50 元。
        [InlineData(1, DelieveryType.Mail, 50, true)]
        //// 5 件以下可以用黑貓，運費 100 元。
        [InlineData(1, DelieveryType.BlackCat, 100, true)]
        public void DelieveryTest(int totalCount, DelieveryType type, decimal expected, bool expectedResult)
        {
            try
            {
                // Arrange
                var actual = 0m;
                var shoppingCart = new ShoppingCart();

                // Act
                actual = shoppingCart.Delievery(totalCount, type);

                // Arrange
                Assert.Equal(expected, actual);
            }
            catch
            {
                // Arrange
                Assert.False(expectedResult);
            }
        }
    }
}