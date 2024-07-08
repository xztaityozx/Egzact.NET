namespace Egzact.Command.Test;

public class CycleTest
{
    public record TestExecuteTestCase(
        string Name,
        IReadOnlyList<string> InputRecord,
        IReadOnlyList<IEnumerable<string>> Expected);

    public static IEnumerable<object[]> TestExecuteTestCases()
    {
        var testCases = new[]
        {
            new TestExecuteTestCase("Empty", [], []),
            new TestExecuteTestCase("Single", ["1"], [["1"]]),
            new TestExecuteTestCase("Double", ["1", "2"], [["1", "2"], ["2", "1"]]),
            new TestExecuteTestCase("Triple", ["1", "2", "3"], [["1", "2", "3"], ["2", "3", "1"], ["3", "1", "2"]]),
            new TestExecuteTestCase(
                "README example",
                ["A", "B", "C", "D", "E"],
                [
                    ["A", "B", "C", "D", "E"],
                    ["B", "C", "D", "E", "A"],
                    ["C", "D", "E", "A", "B"],
                    ["D", "E", "A", "B", "C"],
                    ["E", "A", "B", "C", "D"],
                ]),
        };

        return testCases.Select(tt => new object[] { tt });
    }

    [Theory]
    [MemberData(nameof(TestExecuteTestCases))]
    public void Test_Execute(TestExecuteTestCase testCase)
    {
        var cycle = new Cycle();
        var actual = cycle.Execute(testCase.InputRecord);
        Assert.Equal(testCase.Expected, actual);
    }
}