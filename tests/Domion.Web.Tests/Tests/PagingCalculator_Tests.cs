using System;
using System.Collections.Generic;
using Domion.Web.Helpers;
using FluentAssertions;
using Xunit;

namespace Domion.Web.Tests.Tests
{
    [Trait("Type", "Unit")]
    public class PagingCalculator_Tests
    {
        [Theory, MemberData(nameof(PagingData))]
        public void Create_AssignsValidValues_WhenValidItemCount(
            int testCase,
            int totalItemCount,
            int? page,
            int? pageSize,
            bool expectedOutOfRange,
            int expectedPage,
            int expectedSkip,
            int expectedPageCount,
            int expectedPageSize,
            Dictionary<string, object> expectedValues)
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
            calculator.PagingValues.Should().BeEquivalentTo(expectedValues);
        }

        [Fact]
        public void Create_Fails_WhenInvalidItemCount()
        {
            // Arrange ---------------------------

            // Act -------------------------------

            Action action = () => new PagingCalculator(-1, 1, 10);

            // Assert ----------------------------

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        public static IEnumerable<object[]> PagingData => new[]
        {
            new object[] {1, 0, null, null, false, 1, 0, 0, 10, GetPagingValues(), },
            new object[] {2, 0, 1, 10, false, 1, 0, 0, 10, GetPagingValues(p: 1), },
            new object[] {3, 0, 2, 10, true, 1, 0, 0, 10, GetPagingValues(p: 1), },
            new object[] {4, 10, 1, 10, false, 1, 0, 1, 10, GetPagingValues(p: 1), },
            new object[] {5, 10, 2, 10, true, 1, 0, 1, 10, GetPagingValues(p: 1), },
            new object[] {6, 15, 1, 10, false, 1, 0, 2, 10, GetPagingValues(p: 1), },
            new object[] {7, 15, 3, 10, true, 2, 10, 2, 10, GetPagingValues(p: 2), },
            new object[] {8, 15, -1, 10, true, 1, 0, 2, 10, GetPagingValues(p: 1), },
            new object[] {9, 15, 1, -10, true, 1, 0, 2, 10, GetPagingValues(p: 1), },
            new object[] {10, 15, 1, 0, true, 1, 0, 2, 10, GetPagingValues(p: 1), },
            new object[] {11, 15, 1, 5, false, 1, 0, 3, 5, GetPagingValues(p: 1, ps: 5), },
            new object[] {12, 15, 2, 5, false, 2, 5, 3, 5, GetPagingValues(p: 2, ps: 5), },
            new object[] {13, 15, 4, 5, true, 3, 10, 3, 5, GetPagingValues(p: 3, ps: 5), },
            new object[] {14, 0, null, 5, false, 1, 0, 0, 5, GetPagingValues(ps: 5), },
        };

        private static Dictionary<string, object> GetPagingValues(int? p = null, int? ps = null)
        {
            var values = new Dictionary<string, object>();

            if (p.HasValue)
            {
                values.Add("p", p.Value);
            }

            if (ps.HasValue)
            {
                values.Add("ps", ps.Value);
            }

            return values;
        }
    }
}
