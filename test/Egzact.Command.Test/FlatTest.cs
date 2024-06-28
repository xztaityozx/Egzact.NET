namespace Egzact.Command.Test;

public class FlatTest
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Test_Flat_Constructor_Throws_ArgumentOutOfRangeException(int n)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Flat(n));
    }

    public record TestExecuteTestCase(
        string Name,
        IReadOnlyList<string> InputRecord,
        int NumberOfColumns,
        IReadOnlyList<IEnumerable<string>>? ExpectedSet,
        IReadOnlyList<string> ExpectedRemain
    )
    {
        public override string ToString() => Name;
    }

    public static IEnumerable<object[]> TestExecuteTestCases()
    {
        var testCases = new[]
        {
            new TestExecuteTestCase(
                "a b c d to be 1 column data",
                ["a", "b", "c", "d"],
                1,
                [["a"], ["b"], ["c"], ["d"]],
                []
            ),
            new TestExecuteTestCase(
                "a b c d to be 2 column data",
                ["a", "b", "c", "d"],
                2,
                [["a", "b"], ["c", "d"]],
                []
            ),
            new TestExecuteTestCase(
                "a b c d to be 3 column data",
                ["a", "b", "c", "d"],
                3,
                [["a", "b", "c"]],
                ["d"]
            ),
            new TestExecuteTestCase(
                "a b c d to be 4 column data",
                ["a", "b", "c", "d"],
                4,
                [["a", "b", "c", "d"]],
                []
            ),
            new TestExecuteTestCase(
                "a b c d to be 5 column data",
                ["a", "b", "c", "d"],
                5,
                null,
                ["a", "b", "c", "d"]
            ),
        };

        return testCases.Select(tt => new object[]
            { tt });
    }

    [Theory]
    [MemberData(nameof(TestExecuteTestCases))]
    public void Test_Execute(
        TestExecuteTestCase testCase
    )
    {
        var flat = new Flat(testCase.NumberOfColumns);
        var (set, remain) = flat.Execute(testCase.InputRecord);
        Assert.Equal(testCase.ExpectedSet, set);
        Assert.Equal(testCase.ExpectedRemain, remain);
    }
}