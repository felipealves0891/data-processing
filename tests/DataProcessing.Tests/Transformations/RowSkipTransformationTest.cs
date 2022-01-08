using DataProcessing.Transformations;
using Xunit;

namespace DataProcessing.Tests.Transformations
{
    public class RowSkipTransformationTest
    {
        [Fact]
        public void SkipTest()
        {
            //Arrange
            string[][] lines = new string[][]
            {
                new string[] { "Test" },
                new string[] { "Test1" },
                new string[] { "Test2" }
            };

            string[][] expected = new string[][]
            {
                null,
                new string[] { "Test1" },
                new string[] { "Test2" }
            };

            string[][] results = new string[3][];

            //Act
            RowSkipTransformation skip = new RowSkipTransformation(1);
            results[0] = skip.Transform(lines[0]);
            results[1] = skip.Transform(lines[1]);
            results[2] = skip.Transform(lines[2]);

            //Assert
            Assert.Equal(expected[0], results[0]);
            Assert.Equal(expected[1], results[1]);
            Assert.Equal(expected[2], results[2]);
        }
    }

}