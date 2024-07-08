namespace Egzact.Shared;

public interface IEgzactCommand
{
    IReadOnlyList<IEnumerable<string>> Execute(IReadOnlyList<string> inputRecord);
}