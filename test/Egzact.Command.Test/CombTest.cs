namespace Egzact.Command.Test;

public class CombTest
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Test_Constructor_Throws_ArgumentOutOfRangeException(int numberOfColumns)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Comb(numberOfColumns));
    }

    public record TestExecuteTestCase(
        string Name,
        IReadOnlyList<string> InputRecord,
        int NumberOfColumns,
        IReadOnlyList<IEnumerable<string>> Expected)
    {
        public override string ToString() => Name;
    }

    public static IEnumerable<object[]> TestExecuteTestCases()
    {
        var testCases = new[]
        {
            new TestExecuteTestCase("Empty", [], 1, []),
            new TestExecuteTestCase("Single", ["1"], 1, [["1"]]),
            new TestExecuteTestCase("comb 1, 1 2 to be [[1],[2]]", ["1", "2"], 1, [["1"], ["2"]]),
            new TestExecuteTestCase("comb 2, 1 2 to be [[1, 2]]", ["1", "2"], 2, [["1", "2"]]),
            new TestExecuteTestCase("comb 3, 1 2 to be [[1, 2]]", ["1", "2"], 3, [["1", "2"]]),
            new TestExecuteTestCase("comb 1, 1 2 3 to be [[1],[2],[3]]", ["1", "2", "3"], 1, [["1"], ["2"], ["3"]]),
            new TestExecuteTestCase("comb 2, 1 2 3 to be [[1, 2],[1, 3],[2, 3]]", ["1", "2", "3"], 2,
                [["1", "2"], ["1", "3"], ["2", "3"]]),
            new TestExecuteTestCase("comb 3, 1 2 3 to be [[1, 2, 3]]", ["1", "2", "3"], 3, [["1", "2", "3"]]),
            new TestExecuteTestCase("README example", ["A", "B", "C", "D"], 2, [
                ["A", "B"],
                ["A", "C"],
                ["B", "C"],
                ["A", "D"],
                ["B", "D"],
                ["C", "D"]
            ])
        };

        return testCases.Select(tt => new object[] { tt });
    }
    
    [Theory]
    [MemberData(nameof(TestExecuteTestCases))]
    public void Test_Execute(TestExecuteTestCase testCase)
    {
        var comb = new Comb(testCase.NumberOfColumns);
        var actual = comb.Execute(testCase.InputRecord);
        Assert.Equal(testCase.Expected, actual);
    }
}