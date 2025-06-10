using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using Ternary3.Operators.Operations;
using Xunit;

namespace Ternary3.Tests.Operators.Operations
{
    public class BinaryOperationBuilderTests
    {
        [Theory]
        [InlineData(false, false, false, 
            false, false, false, 
            false, false, false,
            0, 0, 0,
            0, 0, 0, 1)]
        [InlineData(true, true, true, 
            true, true, true, 
            true, true, true,
            3, 3, 3,
            3, 3, 3, 1)]
        [InlineData(true, true, true, 
            false, false, false, 
            false, false, false,
            3, 0, 0,
            1, 1, 1, 1)]
        [InlineData(false, false, false, 
            true, true, true, 
            false, false, false,
            0, 3, 0,
            1, 1, 1, 1)]
        [InlineData(false, false, false, 
            false, false, false, 
            true, true, true,
            0, 0, 3,
            1, 1, 1, 1)]
        [InlineData(true, false, false, 
            false, true, false, 
            false, false, true,
            1, 1, 1,
            1, 1, 1, 1)]
        [InlineData(true, false, true, 
            false, true, false, 
            true, false, true,
            2, 1, 2,
            2, 1, 2, 5)]
        [InlineData(true, true, false, 
            false, false, false, 
            false, false, false,
            2, 0, 0,
            0, 0, 0, 1)]
        public void GetExpressions_ReturnsCorrectNumberOfExpressions(
            bool v00, bool v01, bool v02,
            bool v10, bool v11, bool v12,
            bool v20, bool v21, bool v22,
            int fc0, int fc1, int fc2,
            int sc0, int sc1, int sc2,
            int expectedCount)
        {
            var table = new Ternary3.Operators.BinaryTritOperator();
            var builder = new BinaryOperationBuilder<uint>(table);
            var matrix = new[,]
            {
                { v00, v01, v02 },
                { v10, v11, v12 },
                { v20, v21, v22 }
            };
            var firstCounts = new[] { fc0, fc1, fc2 };
            var secondCounts = new[] { sc0, sc1, sc2 };
            
            var result = builder.GetExpressions(matrix, firstCounts, secondCounts).ToList();
            
            result.Should().HaveCount(expectedCount);
        }
    }
}
