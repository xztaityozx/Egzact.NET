using Egzact.IO;

namespace Egzact.Command;

/// <summary>
/// egzact conv/conv each コマンドの実装クラス。isEach=true の場合はconv eachとしてふるまう
/// </summary>
public class Conv
{
    private readonly int _numberOfColumns;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="numberOfColumns">返還後のカラム数</param>
    /// <exception cref="ArgumentOutOfRangeException">0以下の数値を numberOfColumns に与えたとき投げられる</exception>
    public Conv(int numberOfColumns)
    {
        if(numberOfColumns <= 0) throw new ArgumentOutOfRangeException(nameof(numberOfColumns), "conv の引数は1以上である必要があります");
        _numberOfColumns = numberOfColumns;
    }
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
    public (IReadOnlyList<IEnumerable<string>>? set, IReadOnlyList<string> remain) Execute(IEnumerable<string> inputRecord)
    {
        var r = inputRecord.ToArray();
        if (r.Length < _numberOfColumns) return (null, r);
        
        var span = r.AsSpan();
        var set = new List<IEnumerable<string>>();

        var i = 0;
        for(; i + _numberOfColumns <= span.Length; i++)
        {
            set.Add(span.Slice(i, _numberOfColumns).ToArray());
        }
        
        return (set, span[i..].ToArray());
    }
}