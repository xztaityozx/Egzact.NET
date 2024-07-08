namespace Egzact.Shared;

public enum Direction
{
    Left = 0, Right = 1
}

public class UnknownDirectionException(string message) : Exception(message);