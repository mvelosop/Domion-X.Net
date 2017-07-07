using Domion.WebApp.Helpers;
using FluentAssertions;
using System;
using Xunit;

namespace Domion.WebApp.Tests
{
    [Trait("Type", "Unit")]
    public class PagingCalculator_Tests
    {
        [Theory]
        [InlineData(1, 0, null, null, false, 1, 0, 0, 10)]
        [InlineData(2, 0, 1, 10, false, 1, 0, 0, 10)]
        [InlineData(3, 0, 2, 10, true, 1, 0, 0, 10)]
        [InlineData(4, 10, 1, 10, false, 1, 0, 1, 10)]
        [InlineData(5, 10, 2, 10, true, 1, 0, 1, 10)]
        [InlineData(6, 15, 1, 10, false, 1, 0, 2, 10)]
        [InlineData(7, 15, 3, 10, true, 2, 10, 2, 10)]
        [InlineData(8, 15, -1, 10, true, 1, 0, 2, 10)]
        public void Create_AssignsValidValues_WhenValidItemCount(
            int testCase,
            int totalItemCount,
            int? page,
            int? pageSize,
            bool expectedOutOfRange,
            int expectedPage,
            int expectedSkip,
            int expectedPageCount,
            int expectedPageSize)
        {
            // Arrange ---------------------------

            // Act -------------------------------

            var calculator = new PagingCalculator(totalItemCount, page, pageSize);

            // Assert ----------------------------

            calculator.ItemCount.Should().Be(totalItemCount);
            calculator.OutOfRange.Should().Be(expectedOutOfRange);
            calculator.Page.Should().Be(expectedPage);
            calculator.PageCount.Should().Be(expectedPageCount);
            calculator.PageSize.Should().Be(expectedPageSize);
            calculator.Skip.Should().Be(expectedSkip);
            calculator.Take.Should().Be(expectedPageSize);
        }

        [Fact]
        public void Create_Fails_WhenInvalidItemCount()
        {
            // Arrange ---------------------------

            // Act -------------------------------

            Action action = () => new PagingCalculator(-1, 1, 10);

            // Assert ----------------------------

            action.ShouldThrow<ArgumentOutOfRangeException>();
        }
    }
}
