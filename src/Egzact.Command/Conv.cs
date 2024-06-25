using Egzact.IO;

namespace Egzact.Command;

/// <summary>
/// egzact conv/conv each コマンドの実装クラス。isEach=true の場合はconv eachとしてふるまう
/// </summary>
/// <param name="numberOfColumns">何個のカラムに変換するか</param>
public class Conv( int numberOfColumns)
{
    /// <summary>
    /// 入力ストリームからレコードを読み込み、指定された列数に変換して出力ストリームに書き込む
    /// ex:
    /// > seq 5 | egzact conv 2
    /// 1 2
    /// 2 3
    /// 3 4
    /// 4 5
    /// isEach が true の場合、入力ストリームから読み込んだ各行を個別のセットとして扱う
    /// ex:
    /// > cat a
    /// A B C D
    /// E F G H
    /// > egzact each conv eos=@@@ 2
    /// A B
    /// B C
    /// C D
    /// @@@
    /// E F
    /// F G
    /// G H
    /// <param name="inputRecord">入力レコード</param>
    /// </summary>
    public (IReadOnlyList<IEnumerable<string>>? set, IEnumerable<string> remain) Execute(IEnumerable<string> inputRecord)
    {
        var r = inputRecord.ToArray();
        if (r.Length < numberOfColumns) return (null, r);
        
        var span = r.AsSpan();
        var set = new List<IEnumerable<string>>();

        var i = 0;
        for(; i + numberOfColumns <= span.Length; i++)
        {
            set.Add(span.Slice(i, numberOfColumns).ToArray());
        }
        
        return (set, span[i..].ToArray());
    }
}