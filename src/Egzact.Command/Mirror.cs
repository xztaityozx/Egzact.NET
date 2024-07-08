using Egzact.Shared;

namespace Egzact.Command;

public class Mirror : IEgzactMultipleResultCommand
{
    /// <summary>
    /// 入力された文字列を反転させる
    /// </summary>
    /// <param name="inputRecord"></param>
    /// <returns></returns>
    public IReadOnlyList<IEnumerable<string>> Execute(IReadOnlyList<string> inputRecord)
    {
        return [inputRecord.Reverse()];
    }
}