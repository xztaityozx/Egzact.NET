namespace Egzact.Share;

/// <summary>
/// Global options for all commands
/// </summary>
/// <param name="Fs">Field separator</param>
/// <param name="Eor">End of record</param>
/// <param name="Eos">End of set. Set means, all results generated from single line, in this manual.</param>
public record GlobalOptions(string Fs = " ", string Eor = "\n", string Eos = "\n")
{
    /// <summary>
    /// Input field separator
    /// </summary>
    public string Ifs { get; init; } = Fs;
    
    /// <summary>
    /// Output field separator
    /// </summary>
    public string Ofs { get; init; } = Fs;
}