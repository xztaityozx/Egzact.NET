using Egzact.Shared;

namespace Egzact.Command.Test;

public class StairTest
{
    [Theory]
    [InlineData(2)]
    public void Test_StairConstructor_Throws_ArgumentException(int direction)
    {
        Assert.Throws<ArgumentException>(() => new Stair((Direction)direction));
    }

    public record TestExecuteTestCase(
        string Name,
        IReadOnlyList<string> InputRecord,
        Direction Direction,
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
                "A B C D to be [A], [A B], [A B C], [A B C D]",
                ["A", "B", "C", "D"],
                Direction.Left,
                [["A"], ["A", "B"], ["A", "B", "C"], ["A", "B", "C", "D"]]
            ),
            new TestExecuteTestCase(
                "A B C D to be [D], [C D], [B C D], [A B C D]",
                ["A", "B", "C", "D"],
                Direction.Right,
                [["D"], ["C", "D"], ["B", "C", "D"], ["A", "B", "C", "D"]]
            ),
            new TestExecuteTestCase(
                "Left: A to be [A]",
                ["A"],
                Direction.Left,
                [["A"]]
            ),
            new TestExecuteTestCase(
                "Right: A to be [A]",
                ["A"],
                Direction.Right,
                [["A"]]
            ),
            new TestExecuteTestCase(
                "Empty",
                [],
                Direction.Left,
                []
            )
        };

        return testCases.Select(tt => new object[] { tt });
    }

    [Theory]
    [MemberData(nameof(TestExecuteTestCases))]
    public void TestExecute(TestExecuteTestCase tt)
    {
        var stair = new Stair(tt.Direction);
        var actual = stair.Execute(tt.InputRecord);
        Assert.Equal(tt.Expected, actual);
    }
}