namespace Egzact.Command.Test;

public class DuplicateTest
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Test_Constructor_Throws_ArgumentOutOfRangeException(int n)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Duplicate(n));
    }
    
    public record TestExecuteTestCase(
        string Name,
        IReadOnlyList<string> InputRecord,
        int NumberOfRows,
        IReadOnlyList<IEnumerable<string>> Expected)
    {
        public override string ToString() => Name;
    }

    public static IEnumerable<object[]> TestExecuteTestCases()
    {
        var testCases = new[]
        {
            new TestExecuteTestCase("Empty", [], 1, [[]]),
            new TestExecuteTestCase("dupl 1, 1 to be [[1]]", ["1"], 1, [["1"]]),
            new TestExecuteTestCase("dupl 2, 1 to be [[1], [1]]", ["1"], 2, [["1"], ["1"]]),
            new TestExecuteTestCase("README example", ["A", "B", "C", "D"], 3, [
                ["A", "B", "C", "D"],
                ["A", "B", "C", "D"],
                ["A", "B", "C", "D"],
            ])
        };

        return testCases.Select(tt => new object[] { tt });
    }
    
    [Theory]
    [MemberData(nameof(TestExecuteTestCases))]
    public void Test_Execute(TestExecuteTestCase testCase)
    {
        var duplicate = new Duplicate(testCase.NumberOfRows);
        var actual = duplicate.Execute(testCase.InputRecord);
        Assert.Equal(testCase.Expected, actual);
    }
}