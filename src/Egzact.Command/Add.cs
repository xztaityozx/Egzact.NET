using System.Diagnostics;
using Egzact.Shared;

namespace Egzact.Command;

public record Add
{
    public Direction Direction { get; init; }
    private readonly string _element;
    private const string ErrorMessage = "addコマンドの引数はLeftかRightのみです";

    public Add(in Direction direction, in string element)
    {
        if (direction != Direction.Left && direction != Direction.Right)
        {
            throw new ArgumentOutOfRangeException(nameof(direction), ErrorMessage);
        }

        Direction = direction;
        _element = element;
    }


    /// <summary>
    /// 右端または左端に要素を加える
    /// </summary>
    /// <param name="line"></param>
    /// <exception cref="UnknownDirectionException">DirectionがLeftでもRightでもないときに投げられる</exception>
    /// <returns></returns>
    public string Execute(in string line) =>

        Direction switch
        {
            Direction.Left => $"{_element}{line}",
            Direction.Right => $"{line}{_element}",
            _ => throw new UnknownDirectionException(ErrorMessage)
        };
}