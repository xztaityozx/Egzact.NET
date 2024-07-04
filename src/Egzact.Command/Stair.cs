using Egzact.Shared;

namespace Egzact.Command;

public class Stair : IEgzactMultipleResultCommand
{

    private readonly Direction _direction;
    
    public Stair(Direction direction)
    {
        if(direction != Direction.Left && direction != Direction.Right)
            throw new ArgumentException($"Unknown direction {direction}", nameof(direction));
        _direction = direction;
    }
    
    /// <summary>
    /// Stairは、入力されたレコードのサブセットを返す。出力順が階段状になっていることから、stairと呼ばれてる
    /// stairlは、左から右に、stairrは右から左に出力する
    /// ex:
    /// > echo A B C D | stairl
    /// A
    /// A B
    /// A B C
    /// A B C D
    ///
    /// > yes A B C D | head -n3 | stairl eos=@@@
    /// A
    /// A B
    /// A B C
    /// A B C D
    /// @@@
    /// A
    /// A B
    /// A B C
    /// A B C D
    /// @@@
    /// A
    /// A B
    /// A B C
    /// A B C D
    /// </summary>
    /// <param name="inputRecord"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IReadOnlyList<IEnumerable<string>> Execute(IReadOnlyList<string> inputRecord)
    {
        var span = inputRecord.ToArray().AsSpan();
        var result = new List<IEnumerable<string>>();
        for (var i = 0; i < span.Length; i++)
        {
            result.Add(_direction == Direction.Left
                ? span[..(i + 1)].ToArray()
                : span[(span.Length - i - 1)..].ToArray());
        }
        
        return result;
    }
}