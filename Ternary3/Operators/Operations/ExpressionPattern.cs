using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Ternary3.Operators.Operations
{
    /// <summary>
    /// Represents a pattern in a 3x3 boolean matrix that can be optimized to a specific expression
    /// </summary>
    internal class ExpressionPattern
    {
        /// <summary>
        /// Function that checks if the pattern matches the matrix
        /// </summary>
        public Func<bool[,], int[], int[], bool> Matches { get; }
        
        /// <summary>
        /// Function that creates the optimized expression for this pattern
        /// </summary>
        public Func<bool[,], Expression[], Expression[], Expression> CreateExpression { get; }
        
        /// <summary>
        /// Function that marks cells as handled when this pattern is applied
        /// </summary>
        public Action<bool[,], bool[,]> MarkHandled { get; }

        public ExpressionPattern(
            Func<bool[,], int[], int[], bool> matches,
            Func<bool[,], Expression[], Expression[], Expression> createExpression,
            Action<bool[,], bool[,]> markHandled)
        {
            Matches = matches ?? throw new ArgumentNullException(nameof(matches));
            CreateExpression = createExpression ?? throw new ArgumentNullException(nameof(createExpression));
            MarkHandled = markHandled ?? throw new ArgumentNullException(nameof(markHandled));
        }
        
        /// <summary>
        /// Gets a list of standard patterns for expression optimization
        /// </summary>
        public static List<ExpressionPattern> GetStandardPatterns()
        {
            var patterns = new List<ExpressionPattern>();
            
            // Pattern 1: Empty matrix (all false)
            patterns.Add(new ExpressionPattern(
                // Matches
                (matrix, firstCounts, secondCounts) => 
                    firstCounts[0] == 0 && firstCounts[1] == 0 && firstCounts[2] == 0,
                // Create Expression
                (matrix, expr1, expr2) => Expression.Constant(0),
                // Mark Handled
                (matrix, handled) => { } // Nothing to mark
            ));
            
            // Pattern 2: Full matrix (all true)
            patterns.Add(new ExpressionPattern(
                // Matches
                (matrix, firstCounts, secondCounts) => 
                    firstCounts[0] == 3 && firstCounts[1] == 3 && firstCounts[2] == 3,
                // Create Expression - Call the type-specific CreateTrueConstant on BinaryOperationBuilder
                (matrix, expr1, expr2) => Expression.Constant(1),
                // Mark Handled
                (matrix, handled) => {
                    for (int i = 0; i < 3; i++)
                        for (int j = 0; j < 3; j++)
                            handled[i, j] = true;
                }
            ));
            
            // Pattern 3: Complete row
            for (int row = 0; row < 3; row++)
            {
                int capturedRow = row; // Capture for lambda
                patterns.Add(new ExpressionPattern(
                    // Matches
                    (matrix, firstCounts, secondCounts) => firstCounts[capturedRow] == 3,
                    // Create Expression
                    (matrix, expr1, expr2) => expr1[capturedRow],
                    // Mark Handled
                    (matrix, handled) => {
                        for (int j = 0; j < 3; j++)
                            handled[capturedRow, j] = true;
                    }
                ));
            }
            
            // Pattern 4: Complete column
            for (int col = 0; col < 3; col++)
            {
                int capturedCol = col; // Capture for lambda
                patterns.Add(new ExpressionPattern(
                    // Matches
                    (matrix, firstCounts, secondCounts) => secondCounts[capturedCol] == 3,
                    // Create Expression
                    (matrix, expr1, expr2) => expr2[capturedCol],
                    // Mark Handled
                    (matrix, handled) => {
                        for (int i = 0; i < 3; i++)
                            handled[i, capturedCol] = true;
                    }
                ));
            }
            
            // Pattern 5: Row with exactly 2 true values (exclude handled)
            for (int row = 0; row < 3; row++)
            {
                int capturedRow = row; // Capture for lambda
                patterns.Add(new ExpressionPattern(
                    // Matches
                    (matrix, firstCounts, secondCounts) => {
                        if (firstCounts[capturedRow] != 2) return false;
                        
                        // Find missing column
                        var missingCol = 0;
                        while (missingCol < 3 && matrix[capturedRow, missingCol]) missingCol++;
                        if (missingCol == 3) return false; // Should never happen if count is 2
                        
                        // Check if at least one of the cells is not already handled
                        for (int j = 0; j < 3; j++)
                            if (j != missingCol && matrix[capturedRow, j]) 
                                return true;
                        
                        return false;
                    },
                    // Create Expression
                    (matrix, expr1, expr2) => {
                        // Find missing column
                        var missingCol = 0;
                        while (missingCol < 3 && matrix[capturedRow, missingCol]) missingCol++;
                        
                        return Expression.And(
                            expr1[capturedRow],
                            Expression.Not(expr2[missingCol])
                        );
                    },
                    // Mark Handled
                    (matrix, handled) => {
                        // Find missing column
                        var missingCol = 0;
                        while (missingCol < 3 && matrix[capturedRow, missingCol]) missingCol++;
                        
                        for (int j = 0; j < 3; j++)
                            if (j != missingCol)
                                handled[capturedRow, j] = true;
                    }
                ));
            }
            
            // Pattern 6: Column with exactly 2 true values (exclude handled)
            for (int col = 0; col < 3; col++)
            {
                int capturedCol = col; // Capture for lambda
                patterns.Add(new ExpressionPattern(
                    // Matches
                    (matrix, firstCounts, secondCounts) => {
                        if (secondCounts[capturedCol] != 2) return false;
                        
                        // Find missing row
                        var missingRow = 0;
                        while (missingRow < 3 && matrix[missingRow, capturedCol]) missingRow++;
                        if (missingRow == 3) return false; // Should never happen if count is 2
                        
                        // Check if at least one of the cells is not already handled
                        for (int i = 0; i < 3; i++)
                            if (i != missingRow && matrix[i, capturedCol]) 
                                return true;
                        
                        return false;
                    },
                    // Create Expression
                    (matrix, expr1, expr2) => {
                        // Find missing row
                        var missingRow = 0;
                        while (missingRow < 3 && matrix[missingRow, capturedCol]) missingRow++;
                        
                        return Expression.And(
                            Expression.Not(expr1[missingRow]),
                            expr2[capturedCol]
                        );
                    },
                    // Mark Handled
                    (matrix, handled) => {
                        // Find missing row
                        var missingRow = 0;
                        while (missingRow < 3 && matrix[missingRow, capturedCol]) missingRow++;
                        
                        for (int i = 0; i < 3; i++)
                            if (i != missingRow)
                                handled[i, capturedCol] = true;
                    }
                ));
            }
            
            // Pattern 7: Checkerboard pattern (like in your example)
            patterns.Add(new ExpressionPattern(
                // Matches
                (matrix, firstCounts, secondCounts) => {
                    // Check for the pattern where diagonal and anti-diagonal cells are true
                    // and everything else is false
                    bool diagonalPattern = 
                        matrix[0, 0] && matrix[1, 1] && matrix[2, 2] && 
                        !matrix[0, 1] && !matrix[0, 2] && 
                        !matrix[1, 0] && !matrix[1, 2] && 
                        !matrix[2, 0] && !matrix[2, 1];
                        
                    bool antiDiagonalPattern =
                        matrix[0, 2] && matrix[1, 1] && matrix[2, 0] && 
                        !matrix[0, 0] && !matrix[0, 1] && 
                        !matrix[1, 0] && !matrix[1, 2] && 
                        !matrix[2, 1] && !matrix[2, 2];
                    
                    // Checkerboard pattern (your example)
                    bool checkerboardPattern = 
                        matrix[0, 0] && !matrix[0, 1] && matrix[0, 2] &&
                        !matrix[1, 0] && matrix[1, 1] && !matrix[1, 2] &&
                        matrix[2, 0] && !matrix[2, 1] && matrix[2, 2];
                        
                    return diagonalPattern || antiDiagonalPattern || checkerboardPattern;
                },
                // Create Expression
                (matrix, expr1, expr2) => {
                    // Diagonal pattern
                    if (matrix[0, 0] && matrix[1, 1] && matrix[2, 2] && 
                        !matrix[0, 1] && !matrix[0, 2] && 
                        !matrix[1, 0] && !matrix[1, 2] && 
                        !matrix[2, 0] && !matrix[2, 1])
                    {
                        // (negative1 && negative2) || (zero1 && zero2) || (positive1 && positive2)
                        // This can be simplified to: (first == second)
                        return Expression.Or(
                            Expression.Or(
                                Expression.And(expr1[0], expr2[0]),
                                Expression.And(expr1[1], expr2[1])
                            ),
                            Expression.And(expr1[2], expr2[2])
                        );
                    }
                    
                    // Anti-diagonal pattern
                    if (matrix[0, 2] && matrix[1, 1] && matrix[2, 0] && 
                        !matrix[0, 0] && !matrix[0, 1] && 
                        !matrix[1, 0] && !matrix[1, 2] && 
                        !matrix[2, 1] && !matrix[2, 2])
                    {
                        // (negative1 && positive2) || (zero1 && zero2) || (positive1 && negative2)
                        // This can be simplified based on your specific logic
                        return Expression.Or(
                            Expression.Or(
                                Expression.And(expr1[0], expr2[2]),
                                Expression.And(expr1[1], expr2[1])
                            ),
                            Expression.And(expr1[2], expr2[0])
                        );
                    }
                    
                    // Checkerboard pattern (your example)
                    if (matrix[0, 0] && !matrix[0, 1] && matrix[0, 2] &&
                        !matrix[1, 0] && matrix[1, 1] && !matrix[1, 2] &&
                        matrix[2, 0] && !matrix[2, 1] && matrix[2, 2])
                    {
                        // (not zero1 && not zero2) || (zero1 && zero2)
                        return Expression.Or(
                            Expression.And(
                                Expression.Not(expr1[1]), 
                                Expression.Not(expr2[1])
                            ),
                            Expression.And(expr1[1], expr2[1])
                        );
                    }
                    
                    // Should never get here if match function is correct
                    throw new InvalidOperationException("Pattern matched but no expression created");
                },
                // Mark Handled
                (matrix, handled) => {
                    // Mark all cells as handled for this special pattern
                    for (int i = 0; i < 3; i++)
                        for (int j = 0; j < 3; j++)
                            if (matrix[i, j])
                                handled[i, j] = true;
                }
            ));
            
            // Add more patterns as needed...
            
            return patterns;
        }
    }
}
