namespace Egzact.IO;

public class OutputStream : IDisposable, IAsyncDisposable
{
    private readonly string _endOfSet;
    private readonly string _endOfRecord;
    private readonly string _fieldSeparator;
    private readonly StreamWriter _streamWriter;

    public OutputStream(Stream stream, string fieldSeparator, string endOfRecord, string endOfSet)
    {
        _streamWriter = new StreamWriter(stream);
        _streamWriter.AutoFlush = true;

        _endOfRecord = endOfRecord;
        _endOfSet = endOfSet;
        _fieldSeparator = fieldSeparator;
    }

    /// <summary>
    /// setを出力する
    /// </summary>
    /// <param name="set">出力したいset</param>
    /// <param name="isLastSet">最後のsetかどうか。最後かどうかでeosの出力条件が変わる</param>
    public async Task WriteSetAsync(IReadOnlyList<IEnumerable<string>> set, bool isLastSet)
    {
        var length = set.Count;
        for(var i = 0; i < length; i++)
        {
            var record = set[i];
            await _streamWriter.WriteAsync(string.Join(_fieldSeparator, record));
            if (i < length - 1)
            {
                await _streamWriter.WriteAsync(_endOfRecord);
            }
        }

        // 最終行の場合はeosではなく改行を出力する。本家がそうなってる
        if (!isLastSet) await _streamWriter.WriteLineAsync(_endOfSet);
        else await _streamWriter.WriteAsync(Environment.NewLine);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _streamWriter.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await _streamWriter.DisposeAsync();
    }
}