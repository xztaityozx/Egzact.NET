namespace Egzact.Command.Test;

public class ObrevTest
{
    public record TestCase(string Name, IReadOnlyList<string> InputRecord, IReadOnlyList<IEnumerable<string>> Expected);
    
    public static IEnumerable<object[]> TestCases()
    {
        var testCases = new[]
        {
            new TestCase("Empty", [], [[],[]]),
            new TestCase("1 to be [[1], [1]]", ["1"], [["1"], ["1"]]),
            new TestCase("1 2 to be [[1,2], [2,1]]", ["1", "2"], [["1", "2"], ["2", "1"]]),
            new TestCase("README example", ["A", "B", "C", "D"], [
                ["A", "B", "C", "D"],
                ["D", "C", "B", "A"]
            ])
        };

        return testCases.Select(tt => new object[] { tt });
    }
    
    [Theory]
    [MemberData(nameof(TestCases))]
    public void Test_Execute(TestCase testCase)
    {
        var obrev = new Obrev();
        var actual = obrev.Execute(testCase.InputRecord);
        Assert.Equal(testCase.Expected, actual);
    }
}