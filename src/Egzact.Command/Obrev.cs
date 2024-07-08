using Egzact.Shared;

namespace Egzact.Command;

public class Obrev : IEgzactCommand
{
    /// <summary>
    /// 入力レコードとその逆順を返す
    /// ex:
    /// > echo a b c d | obrev
    /// a b c d
    /// d c b a
    /// </summary>
    /// <param name="inputRecord"></param>
    /// <returns></returns>
    public IReadOnlyList<IEnumerable<string>> Execute(IReadOnlyList<string> inputRecord) =>
    [
        inputRecord,
        inputRecord.Reverse()
    ];
}