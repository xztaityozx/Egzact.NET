namespace Egzact.Command;

public class Slit
{
    private readonly int _numberOfSlits;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="numberOfSlits">何行に分割するか</param>
    /// <exception cref="ArgumentOutOfRangeException">numberOfSlitsが0以下のときに投げられる</exception>
    public Slit(int numberOfSlits)
    {
        if (numberOfSlits <= 0)
            throw new ArgumentOutOfRangeException(nameof(numberOfSlits), "slitコマンドの引数は1以上である必要があります。");

        _numberOfSlits = numberOfSlits;
    }

    /// <summary>
    /// Slitは入力を指定された行数に分割する
    /// ex:
    /// > seq 7 | slit 3
    /// 1 2 3
    /// 4 5
    /// 6 7
    ///
    /// > seq 7 | slit 4
    /// 1 2
    /// 3 4
    /// 5 6
    /// 7
    ///
    /// > yes 1 2 3 4 | head -n3 | slit each eos=@@@ 3
    /// 1 2
    /// 3
    /// 4
    /// @@@
    /// 1 2
    /// 3
    /// 4
    /// @@@
    /// 1 2
    /// 3
    /// 4
    /// </summary>
    /// <param name="inputRecord">入力レコード</param>
    /// <returns></returns>
    public IReadOnlyList<IEnumerable<string>> Execute(IReadOnlyList<string> inputRecord)
    {
        var length = inputRecord.Count;
        // 入力をすべて分解しても行数が足りない場合は1行1フィールドにして最大限満たすようにする
        if (length < _numberOfSlits) return inputRecord.Select(str => new[] { str }).ToList();

        var set = new List<IEnumerable<string>>(_numberOfSlits);
        var quotient = length / _numberOfSlits;
        var remainder = length % _numberOfSlits;

        var startIndex = 0;
        for (var i = 0; i < _numberOfSlits; i++)
        {
            var size = quotient + (i < remainder ? 1 : 0);
            // Spanにした方がアクセスが速そうな気がする
            set.Add(inputRecord.Skip(startIndex).Take(size).ToList());
            startIndex += size;
        }

        return set;
    }
}