using Egzact.Shared;

namespace Egzact.Command;

public class Duplicate : IEgzactCommand
{
    private readonly int _numberOfRows;

    public Duplicate(int n)
    {
        if(n < 1) throw new ArgumentOutOfRangeException(nameof(n), "duplicateコマンドの引数は1以上である必要があります。");
        _numberOfRows = n;
    }
    
    /// <summary>
    /// 入力レコードを複製して返す
    /// </summary>
    /// <param name="inputRecord"></param>
    /// <returns></returns>
    public IReadOnlyList<IEnumerable<string>> Execute(IReadOnlyList<string> inputRecord)
    {
        return Enumerable.Repeat(inputRecord, _numberOfRows).ToList();
    }
}