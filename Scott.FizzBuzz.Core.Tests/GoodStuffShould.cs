using FluentAssertions;
using LanguageExt.UnitTesting;

namespace Scott.FizzBuzz.Core.Tests;

public class GoodStuffShould

{
    [Theory]
    [ClassData(typeof(TestLeftData))]
    public void ReturnExpectedLeftValue(int value, string expectedResult)
    {
        //Arrange
        //Act
        var result = LanguageExtExample.LanguageExtFizzBuzz(value);
        
        //Assert
        result.ShouldBeLeft(x => x.Should().Be(expectedResult));
    }
    
    [Theory]
    [ClassData(typeof(TestRightData))]
    public void ReturnExpectedRightValue(int value, int expectedResult)
    {
        //Arrange
        //Act
        var result = LanguageExtExample.LanguageExtFizzBuzz(value);
        
        //Assert
        result.ShouldBeRight(x => x.Should().Be(expectedResult));
    }
    
    public class TestLeftData : TheoryData<int, string>
    {
        public TestLeftData()
        {
            Add(3, "Fizz");
            Add(5, "Buzz");
            Add(15, "NoDependencyFizzBuzz");
        }
    }
    
    public class TestRightData : TheoryData<int, int>
    {
        public TestRightData()
        {
            Add(1, 1);
            Add(2, 2);
            Add(17, 17);
            Add(94, 94);
        }
    }
}