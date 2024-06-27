namespace Egzact.Command.Test;

public class ConvTest
{
    [Theory]
    [MemberData(nameof(TestCases))]
    public void Test_Execute(IEnumerable<string> input, int numberOfColumns, Expected expected)
    {
        var conv = new Conv(numberOfColumns);
        var (set, remain) = conv.Execute(input);
        Assert.Equal(expected.Set, set);
        Assert.Equal(expected.Remain, remain);
    }

    public record Expected(IReadOnlyList<IEnumerable<string>>? Set, IReadOnlyList<string> Remain);

    private record TestCase(
        IEnumerable<string> Input,
        int NumberOfColumns,
        Expected Expected);

    public static IEnumerable<object[]> TestCases()
    {
        var testCases = new[]
        {
            new TestCase(
                ["a", "b", "c", "d"],
                1,
                new Expected(
                    [["a"], ["b"], ["c"], ["d"]],
                    []
                )
            ),
            new TestCase(
                ["a", "b", "c", "d"],
                2,
                new Expected(
                    [["a", "b"], ["b", "c"], ["c", "d"]],
                    ["d"]
                )
            ),
            new TestCase(
                ["a", "b", "c", "d"],
                3,
                new Expected(
                    [["a", "b", "c"], ["b", "c", "d"]],
                    ["c", "d"]
                )
            ),
            new TestCase(
                ["a", "b", "c", "d"],
                4,
                new Expected(
                    [["a", "b", "c", "d"]],
                    ["b", "c", "d"]
                )
            ),
            new TestCase(
                ["a", "b", "c", "d"],
                5,
                new Expected(
                    null, // 5カラムにできないのでrecordが作られない
                    ["a", "b", "c", "d"]
                )
            ),
        };

        return testCases.Select(x => new object[] { x.Input, x.NumberOfColumns, x.Expected });
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Test_Conv_Constructor_Throws_ArgumentOutOfRangeException(int n)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Conv(n));
    }
}