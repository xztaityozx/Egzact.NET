using Egzact.Shared;

namespace Egzact.Command;

public class Comb : IEgzactCommand
{
    private readonly int _numberOfColumns;
    
    public Comb(int numberOfColumns)
    {
        if(numberOfColumns < 1) throw new ArgumentOutOfRangeException(nameof(numberOfColumns), "combコマンドの引数は1以上である必要があります。");
        _numberOfColumns = numberOfColumns;
    }

    /// <summary>
    /// すべての組み合わせを返す
    /// </summary>
    /// <param name="inputRecord"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IReadOnlyList<IEnumerable<string>> Execute(IReadOnlyList<string> inputRecord)
    {
        var length = Math.Min(_numberOfColumns, inputRecord.Count);
        if (length == 0) return [];
        return GetKCombinations(inputRecord.Select((v, i) => new ValueAndIndex(v, i)).ToList(), length)
            .Select(list => list.ToArray())
            .OrderBy(list => list[^1].Index)
            .Select(list => list.Select(v => v.Value))
            .ToList();
    }

    private static IEnumerable<IEnumerable<ValueAndIndex>> GetKCombinations(IReadOnlyList<ValueAndIndex> list, int k)
    {
        if(k == 1) return list.Select(t => new[] { t });
        return GetKCombinations(list, k - 1)
            .SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0),
                (t1, t2) => t1.Concat(new[] { t2 }));
    }

    private struct ValueAndIndex(string value, int index) : IComparable<ValueAndIndex>
    {
        public readonly string Value = value;
        public readonly int Index = index;
        
        public int CompareTo(ValueAndIndex other)
        {
            var stringCompareResult = string.Compare(Value, other.Value, StringComparison.Ordinal);
            return stringCompareResult != 0 ? stringCompareResult : Index.CompareTo(other.Index);
        }
    }
}