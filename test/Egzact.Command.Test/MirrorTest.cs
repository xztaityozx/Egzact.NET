namespace Egzact.Command.Test;

public class MirrorTest
{
    public record TestCase(string Name, IReadOnlyList<string> InputRecord, IReadOnlyList<IEnumerable<string>> Expected)
    {
        public override string ToString() => Name;
    }
    
    public static IEnumerable<object[]> TestCases()
    {
        var testCases = new[]
        {
            new TestCase("Empty", [], [[]]),
            new TestCase("1 to be [1]", ["1"], [["1"]]),
            new TestCase("1 2 to be [2, 1]", ["1", "2"], [["2", "1"]]),
            new TestCase("1 2 3 to be [3, 2, 1]", ["1", "2", "3"], [["3", "2", "1"]]),
            new TestCase("README example", ["A", "B", "C", "D"], [["D", "C", "B", "A"]]),
        };

        return testCases.Select(x => new object[] { x });
    }
    
    [Theory]
    [MemberData(nameof(TestCases))]
    public void Execute(TestCase testCase)
    {
        var mirror = new Mirror();
        Assert.Equal(testCase.Expected, mirror.Execute(testCase.InputRecord));
    }
}