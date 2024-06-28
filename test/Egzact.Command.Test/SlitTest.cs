namespace Egzact.Command.Test;

public class SlitTest
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Test_Slit_Constructor_Throws_ArgumentOutOfRangeException(int numberOfSlits)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Slit(numberOfSlits));
    }

    public record TestExecuteTestCase(
        string Name,
        IReadOnlyList<string> InputRecord,
        int NumberOfSlits,
        IReadOnlyList<IEnumerable<string>> Expected)
    {
        public override string ToString() => Name;
    }
    
    public static IEnumerable<object[]> TestExecuteTestCases()
    {
        var testCases = new[]
            {
                new TestExecuteTestCase(
                "1 2 3 4 5 6 7 to be [1, 2, 3], [4, 5], [6, 7]",
                ["1", "2", "3", "4", "5", "6", "7"],
                3,
                [["1", "2", "3"], ["4", "5"], ["6", "7"]]
                ),
                new TestExecuteTestCase(
                    "1 2 3 4 5 6 7 to be [1, 2], [3, 4], [5, 6], [7]",
                    ["1", "2", "3", "4", "5", "6", "7"],
                    4,
                    [["1", "2"], ["3", "4"], ["5", "6"], ["7"]]
                ),
                new TestExecuteTestCase(
                    "1 2 3 4 5 6 7 to be [1], [2], [3], [4], [5], [6], [7]",
                    ["1", "2", "3", "4", "5", "6", "7"],
                    7,
                    [["1"], ["2"], ["3"], ["4"], ["5"], ["6"], ["7"]]
                ),
                new TestExecuteTestCase(
                    "1 2 3 4 5 6 7 to be [1], [2], [3], [4], [5], [6], [7]",
                    ["1", "2", "3", "4", "5", "6", "7"],
                    8,
                    [["1"], ["2"], ["3"], ["4"], ["5"], ["6"], ["7"]]
                ),
                new TestExecuteTestCase(
                    "1 2 3 4 5 6 7 to be [1], [2], [3], [4], [5], [6], [7]",
                    ["1", "2", "3", "4", "5", "6", "7"],
                    2,
                    [["1", "2", "3", "4"], ["5", "6", "7"]]
                ),
                new TestExecuteTestCase(
                    "1 2 3 4 5 6 7 to be [1], [2], [3], [4], [5], [6], [7]",
                    ["1", "2", "3", "4", "5", "6", "7"],
                    1,
                    [["1", "2", "3", "4", "5", "6", "7"]]
                ),
            };

        return testCases.Select(tt => new object[] { tt });
    }

    [Theory]
    [MemberData(nameof(TestExecuteTestCases))]
    public void Test_Execute(TestExecuteTestCase tt)
    {
        var slit = new Slit(tt.NumberOfSlits);
        var actual = slit.Execute(tt.InputRecord);
        Assert.Equal(tt.Expected, actual);
    }
}