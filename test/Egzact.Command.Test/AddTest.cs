using Egzact.Shared;

namespace Egzact.Command.Test;

public class AddTest
{
    [Theory]
    [InlineData((Direction)3)]
    public void Add_InvalidDirection_ThrowsException(Direction direction)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Add(direction, "element"));
    }

    public record TestCase(
        string Name,
        Direction Direction,
        string Element,
        string Line,
        string Expected,
        Type? Throws)
    {
        public override string ToString() => Name;
    }

    public static IEnumerable<object[]> TestCases()
    {
        var testCases = new[]
        {
            new TestCase("add to right to [] to be [1]", Direction.Right, "1", "", "1", null),
            new TestCase("add to left to [] to be [1]", Direction.Left, "1", "", "1", null),
            new TestCase("add to right to [1] to be [1, 2]", Direction.Right, "2", "1", "12",null),
            new TestCase("add to left to [1] to be [2, 1]", Direction.Left, "2", "1", "21",null),
            new TestCase("throws UnknownDirectionException", (Direction)3, "1", "", "",
                typeof(UnknownDirectionException)),
            new TestCase("README example(addl)", Direction.Left, "ABC", "abc", "ABCabc", null),
            new TestCase("README example(addr)", Direction.Right, "ABC", "abc", "abcABC", null),
        };

        return testCases.Select(x => new object[] { x });

    }
    
    [Theory]
    [MemberData(nameof(TestCases))]
    public void Add_Execute(TestCase testCase)
    {
        if (testCase.Throws is not null)
        {
            var add = new Add(Direction.Left, "");
            var target = add with { Direction = testCase.Direction };
            Assert.Throws(testCase.Throws, () => target.Execute(testCase.Line));
        }
        else
        {
            Assert.Equal(testCase.Expected, new Add(testCase.Direction, testCase.Element).Execute(testCase.Line));
        }
    }
}