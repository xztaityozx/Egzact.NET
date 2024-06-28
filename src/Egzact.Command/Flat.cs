namespace Egzact.Command;

public class Flat
{
    private readonly int _numberOfColumns;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="numberOfColumns">返還後のカラム数</param>
    /// <exception cref="ArgumentOutOfRangeException">numberOfColumnsに0以下の値を設定すると投げられます</exception>
    public Flat(int? numberOfColumns)
    {
        if (numberOfColumns <= 0)
            throw new ArgumentOutOfRangeException(nameof(numberOfColumns), "flatコマンドの引数は1以上である必要があります。");
        _numberOfColumns = numberOfColumns ?? -1;
    }
    
    /// <summary>
    /// N列のデータに変換する
    /// ex:
    /// > seq 5 | flat 3
    /// 1 2 3
    /// 4 5
    ///
    /// ex:
    /// > seq 5 | flat 1
    /// 1
    /// 2
    /// 3
    /// 4
    /// 5
    ///
    /// ex:
    /// > yes a b c d | head -n2 | flat each eos=@@@ 3
    /// a b c
    /// d
    /// @@@
    /// a b c
    /// d
    /// </summary>
    /// <param name="inputRecord"></param>
    /// <returns></returns>
    public (IReadOnlyList<IEnumerable<string>>? set, IReadOnlyList<string> remain) Execute(
        IReadOnlyList<string> inputRecord)
    {
        // カラム数が -1 のときは1行にまとめるので全部remain
        if (_numberOfColumns == -1) return ([], inputRecord);
        // 入力がカラム数を満たさない場合は次にまわす
        if (inputRecord.Count < _numberOfColumns) return (null, inputRecord);

        var chunk = inputRecord.Chunk(_numberOfColumns).ToList();
        var length = chunk.Count;
        if (chunk[length-1].Length < _numberOfColumns)
        {
            return (new List<IEnumerable<string>>(chunk.Take(length - 1)), chunk[length-1]);
        }

        return (chunk, []);
    }
}