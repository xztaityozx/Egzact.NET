using Egzact.Shared;

namespace Egzact.Command;

public class SubList : IEgzactMultipleResultCommand
{
    /// <summary>
    /// inputRecordのサブリストを返す
    /// ex:
    /// > echo A B C D | sublist
    /// A
    /// A B
    /// B
    /// A B C
    /// B C
    /// C
    /// A B C D
    /// B C D
    /// C D
    /// D
    /// </summary>
    /// <param name="inputRecord"></param>
    /// <returns></returns>
    public IReadOnlyList<IEnumerable<string>> Execute(IReadOnlyList<string> inputRecord)
    {
        var stair = new Stair(Direction.Right);
        var result = new List<IEnumerable<string>>();
        var length = inputRecord.Count;
        
        
        for (var i = 1; i <= length; i++)
        {
            var set = stair.Execute(inputRecord.Take(i).ToList());
            result.AddRange(set.Reverse());
        }

        return result;
    }
}