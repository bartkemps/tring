using Tring.Numbers;
using Xunit;

namespace Tring.Tests.Numbers
{
    public class TritArray27Tests
    {
        [Fact]
        public void SetAndGetTritValues_WorksCorrectly()
        {
            var array = new TritArray27();
            // Set all to Zero, then set some to Positive and Negative
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = Trit.Zero;
            }
            array[0] = Trit.Positive;
            array[1] = Trit.Negative;
            array[2] = Trit.Positive;
            array[26] = Trit.Negative;

            Assert.Equal(Trit.Positive, array[0]);
            Assert.Equal(Trit.Negative, array[1]);
            Assert.Equal(Trit.Positive, array[2]);
            Assert.Equal(Trit.Negative, array[26]);
            // All others should be Zero
            for (int i = 3; i < 26; i++)
            {
                Assert.Equal(Trit.Zero, array[i]);
            }
        }
    }
}

