namespace Egzact.Command.Test;

public class SubSetTest
{
    public record TestExecuteTestCase(
        string Name,
        IReadOnlyList<string> InputRecord,
        IReadOnlyList<IEnumerable<string>> Expected);

    public static IEnumerable<object[]> TestExecuteTestCases()
    {
        var testCases = new[]
        {
            new TestExecuteTestCase(
                "Empty",
                [],
                []
            ),
            new TestExecuteTestCase(
                "A to be A",
                ["A"],
                [["A"]]
            ),
            new TestExecuteTestCase(
                "A B to be A, B, A B",
                ["A", "B"],
                [["A"], ["B"], ["A", "B"]]
            ),
            new TestExecuteTestCase(
                "A B C D to be A, B, C, D, A B, A C, B C, A D, B D, C D, A B C, A B D, A C D, B C D, A B C D",
                ["A", "B", "C", "D"],
                [
                    ["A"], ["B"], ["C"], ["D"],
                    ["A", "B"], ["A", "C"], ["B", "C"], ["A", "D"], ["B", "D"], ["C", "D"],
                    ["A", "B", "C"], ["A", "B", "D"], ["A", "C", "D"], ["B", "C", "D"],
                    ["A", "B", "C", "D"]
                ]
            )
        };
        
        return testCases.Select(tt => new object[] { tt });
    }
    
    [Theory]
    [MemberData(nameof(TestExecuteTestCases))]
    public void TestExecute(TestExecuteTestCase testCase)
    {
        var subset = new SubSet();
        var actual = subset.Execute(testCase.InputRecord);
        Assert.Equal(testCase.Expected, actual);
    }
}