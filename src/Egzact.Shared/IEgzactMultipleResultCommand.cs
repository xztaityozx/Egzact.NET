namespace Egzact.Shared;

public interface IEgzactMultipleResultCommand
{
    IReadOnlyList<IEnumerable<string>> Execute(IReadOnlyList<string> inputRecord);
}