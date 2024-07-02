namespace Egzact.Command.Test;

public class SubListTest
{
    public record TestExecuteTestCase(
        string Name,
        IReadOnlyList<string> InputRecord,
        IReadOnlyList<IEnumerable<string>> Expected
    )
    {
        public override string ToString() => Name;
    }

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
                "A B to be A, A B, B",
                ["A", "B"],
                [["A"], ["A", "B"], ["B"]]
            ),
            new TestExecuteTestCase(
                "A B C D to be A, A B, B, A B C, B C, C, A B C D, B C D, C D, D",
                ["A", "B", "C", "D"],
                [
                    ["A"], 
                    ["A", "B"], ["B"], 
                    ["A", "B", "C"], ["B", "C"], ["C"], 
                    ["A", "B", "C", "D"], ["B", "C", "D"], ["C", "D"], ["D"]
                ]
            )
        };

        return testCases.Select(tt => new object[] { tt });
    }
    
    [Theory]
    [MemberData(nameof(TestExecuteTestCases))]
    public void TestExecute(TestExecuteTestCase testCase)
    {
        var subList = new SubList();
        var actual = subList.Execute(testCase.InputRecord);
        Assert.Equal(testCase.Expected, actual);
    }
}