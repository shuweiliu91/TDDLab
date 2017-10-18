using Xunit;
using HarryPotterShoppingCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

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
    }
}