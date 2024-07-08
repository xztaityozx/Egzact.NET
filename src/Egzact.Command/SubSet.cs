using Egzact.Shared;

namespace Egzact.Command;

public class SubSet : IEgzactCommand
{
    /// <summary>
    /// inputRecordのサブセットを返す
    /// ex:
    /// > echo A B C D | subset
    /// A
    /// B
    /// C
    /// D
    /// A B
    /// A C
    /// B C
    /// A D
    /// B D
    /// C D
    /// A B C
    /// A B D
    /// A C D
    /// B C D
    /// A B C D
    /// </summary>
    /// <param name="inputRecord"></param>
    /// <returns></returns>
    public IReadOnlyList<IEnumerable<string>> Execute(IReadOnlyList<string> inputRecord)
    {
        var length = inputRecord.Count;
        var result = new List<IReadOnlyList<Vi>>((int)Math.Pow(2, length));
        var subset = new List<Vi>();

        GenerateSubSets(
            inputRecord,
            0,
            subset,
            result
        );
        
        return result.OrderBy(t => t.Count).ThenBy(t => t[^1].I)
            .Select(t => t.Select(x => x.V)).ToList();
    }
    
    private static void GenerateSubSets(
        IReadOnlyList<string> inputRecord,
        int start,
        IList<Vi> subset,
        ICollection<IReadOnlyList<Vi>> result)
    {
        if (subset.Count > 0)
        {
            result.Add(new List<Vi>(subset));
        }

        for (var i = start; i < inputRecord.Count; i++)
        {
            subset.Add(new Vi(inputRecord[i], i));
            GenerateSubSets(inputRecord, i + 1, subset, result);
            subset.RemoveAt(subset.Count - 1);
        }
    }

    private struct Vi(string v, int i)
    {
        public readonly string V = v;
        public readonly int I = i;
    }
}