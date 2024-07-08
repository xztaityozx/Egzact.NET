using Egzact.Shared;

namespace Egzact.Command;

public class Cycle : IEgzactMultipleResultCommand
{
    /// <summary>
    /// 入力レコードを回転させます
    /// ex:
    /// > echo 1 2 3 4 | cycle
    /// 1 2 3 4
    /// 2 3 4 1
    /// 3 4 1 2
    /// 4 1 2 3
    /// </summary>
    /// <param name="inputRecord"></param>
    /// <returns></returns>
    public IReadOnlyList<IEnumerable<string>> Execute(IReadOnlyList<string> inputRecord)
    {
        var q = new Queue<string>(inputRecord);
        var result = new List<IEnumerable<string>>(inputRecord.Count);
        for (var i = 0; i < inputRecord.Count; i++)
        {
            result.Add(q.ToArray());
            q.Enqueue(q.Dequeue());
        }

        return result;
    }
}