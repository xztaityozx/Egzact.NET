using System.Text;

namespace Egzact.IO.Test;

public class OutputStreamTest
{
    [Fact]
    public async Task Test_WriteLineEosAsync()
    {
        var buffer = new byte[Environment.NewLine.Length];
        using var ms = new MemoryStream(buffer);
        await using (var os = new OutputStream(ms, " ", "", ""))
        {
            await os.WriteLineEosAsync();
        }

        var s = Encoding.UTF8.GetString(buffer);
        Assert.Equal(Environment.NewLine, s);
    }

    [Fact]
    public async Task Test_WriteEosAsync()
    {
        const string eos = "@@@";
        var buffer = new byte[eos.Length];
        using var ms = new MemoryStream(buffer);
        await using (var os = new OutputStream(ms, " ", "", eos))
        {
            await os.WriteEosAsync();
        }

        var s = Encoding.UTF8.GetString(buffer);
        Assert.Equal(eos, s);
    }


    private record TestWriteSetAsyncTestCase(
        TestWriteSetAsyncInput Input,
        string Expected
    );

    public record TestWriteSetAsyncInput(
        string Name,
        IReadOnlyList<IEnumerable<string>> Set,
        bool AllowWriteEorAtEndOfRecord,
        string EndOfRecord,
        string FieldSeparator
    )
    {
        public override string ToString() => Name;
    }

    public static IEnumerable<object[]> Test_WriteEos_TestCases()
    {
        var testCase = new[]
        {
            new TestWriteSetAsyncTestCase(
                new TestWriteSetAsyncInput(
                    "Empty",
                    [],
                    true,
                    "",
                    ""
                ),
                ""
            ),
            new TestWriteSetAsyncTestCase(
                new TestWriteSetAsyncInput(
                    "[[A,B], [C,D]] to be A B\nC D\n",
                    [["A", "B"], ["C", "D"]],
                    true,
                    "\n",
                    " "
                ),
                "A B\nC D\n"
            ),
            new TestWriteSetAsyncTestCase(
                new TestWriteSetAsyncInput(
                    "[[A,B], [C,D]] to be A///B---C///D@@@",
                    [["A", "B"], ["C", "D"]],
                    false,
                    "---",
                    "///"
                ),
                $"A///B---C///D{Environment.NewLine}"
            ),
            new TestWriteSetAsyncTestCase(
                new TestWriteSetAsyncInput(
                    "[[A,B], [C,D]] to be A///B---C///D---",
                    [["A", "B"], ["C", "D"]],
                    true,
                    "---",
                    "///"
                ),
                "A///B---C///D---"
            )
        };

        return testCase.Select(tt => new object[] { tt.Input, tt.Expected });
    }

    [Theory]
    [MemberData(nameof(Test_WriteEos_TestCases))]
    public async Task Test_WriteSetAsync(TestWriteSetAsyncInput input, string expected)
    {
        using var ms = new MemoryStream();
        await using (var os = new OutputStream(ms, input.FieldSeparator, input.EndOfRecord, ""))
        {
            await os.WriteSetAsync(input.Set, input.AllowWriteEorAtEndOfRecord);
        }

        Assert.Equal(expected, Encoding.UTF8.GetString(ms.ToArray()));
    }
}