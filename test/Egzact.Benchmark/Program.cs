using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<SubsetBenchmark>();

public class SubsetBenchmark
{
    private readonly IReadOnlyList<string> _inputRecord = ["A", "B", "C", "D"];

    [Benchmark]
    public void BenchmarkGenerateSubsetWithTuple()
    {
        var subset = new List<(string, int)>();
        var result = new List<IReadOnlyList<(string, int)>>();
        GenerateSubsetWithTuple(_inputRecord, 0, subset, result);
    }

    private static void GenerateSubsetWithTuple(
        IReadOnlyList<string> inputRecord,
        int start,
        IList<(string, int)> subset,
        ICollection<IReadOnlyList<(string, int)>> result)
    {
        if (subset.Count > 0)
        {
            result.Add(new List<(string, int)>(subset));
        }

        for (var i = start; i < inputRecord.Count; i++)
        {
            subset.Add((inputRecord[i], i));
            GenerateSubsetWithTuple(inputRecord, i + 1, subset, result);
            subset.RemoveAt(subset.Count - 1);
        }
    }

    [Benchmark]
    public void BenchmarkGenerateSubsetWithReadOnlyMemberStruct()
    {
        var subset = new List<ValueAndIndexReadOnlyMember>();
        var result = new List<IReadOnlyList<ValueAndIndexReadOnlyMember>>();
        GenerateSubsetWithReadOnlyStruct(_inputRecord, 0, subset, result);
    }

    private static void GenerateSubsetWithReadOnlyStruct(
        IReadOnlyList<string> inputRecord,
        int start,
        IList<ValueAndIndexReadOnlyMember> subset,
        ICollection<IReadOnlyList<ValueAndIndexReadOnlyMember>> result)
    {
        if (subset.Count > 0)
        {
            result.Add(new List<ValueAndIndexReadOnlyMember>(subset));
        }

        for (var i = start; i < inputRecord.Count; i++)
        {
            subset.Add(new ValueAndIndexReadOnlyMember(inputRecord[i], i));
            GenerateSubsetWithReadOnlyStruct(inputRecord, i + 1, subset, result);
            subset.RemoveAt(subset.Count - 1);
        }
    }

    [Benchmark]
    public void BenchmarkGenerateSubsetWithStruct()
    {
        var subset = new List<ValueAndIndex>();
        var result = new List<IReadOnlyList<ValueAndIndex>>();
        GenerateSubsetWithStruct(_inputRecord, 0, subset, result);
    }

    private static void GenerateSubsetWithStruct(
        IReadOnlyList<string> inputRecord,
        in int start,
        IList<ValueAndIndex> subset,
        ICollection<IReadOnlyList<ValueAndIndex>> result)
    {
        if (subset.Count > 0)
        {
            result.Add(new List<ValueAndIndex>(subset));
        }

        for (var i = start; i < inputRecord.Count; i++)
        {
            subset.Add(new ValueAndIndex(inputRecord[i], i));
            GenerateSubsetWithStruct(inputRecord, i + 1, subset, result);
            subset.RemoveAt(subset.Count - 1);
        }
    }

    [Benchmark]
    public void BenchmarkGenerateSubsetWithReadOnlyStruct()
    {
        var subset = new List<ValueAndIndexReadOnly>();
        var result = new List<IReadOnlyList<ValueAndIndexReadOnly>>();
        GenerateSubsetWithReadOnlyStruct(_inputRecord, 0, subset, result);
    }

    private static void GenerateSubsetWithReadOnlyStruct(
        IReadOnlyList<string> inputRecord,
        int start,
        IList<ValueAndIndexReadOnly> subset,
        ICollection<IReadOnlyList<ValueAndIndexReadOnly>> result)
    {
        if (subset.Count > 0)
        {
            result.Add(new List<ValueAndIndexReadOnly>(subset));
        }

        for (var i = start; i < inputRecord.Count; i++)
        {
            subset.Add(new ValueAndIndexReadOnly(inputRecord[i], i));
            GenerateSubsetWithReadOnlyStruct(inputRecord, i + 1, subset, result);
            subset.RemoveAt(subset.Count - 1);
        }
    }

    private struct ValueAndIndexReadOnlyMember(string value, int index)
    {
        public readonly string Value = value;
        public readonly int Index = index;
    }

    private struct ValueAndIndex(string value, int index)
    {
        public string Value = value;
        public int Index = index;
    }

    private readonly struct ValueAndIndexReadOnly(string value, int index)
    {
        public readonly string Value = value;
        public readonly int Index = index;
    }
}