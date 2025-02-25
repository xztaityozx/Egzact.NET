﻿namespace Egzact.IO;

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
    /// <param name="allowWriteEorAtEndOfRecord">各レコードの最後にeorを書いていいかどうか</param>
    public async Task WriteSetAsync(IReadOnlyList<IEnumerable<string>> set, bool allowWriteEorAtEndOfRecord)
    {
        var length = set.Count;
        for (var i = 0; i < length; i++)
        {
            var record = set[i];
            await _streamWriter.WriteAsync(string.Join(_fieldSeparator, record));
            if (i < length - 1)
                await _streamWriter.WriteAsync(_endOfRecord);
            else
            {
                if (allowWriteEorAtEndOfRecord)
                    await _streamWriter.WriteAsync(_endOfRecord);
                else
                    await _streamWriter.WriteAsync(Environment.NewLine);
            }
        }
    }

    public async Task WriteEosAsync() => await _streamWriter.WriteAsync(_endOfSet);

    public async Task WriteLineEosAsync() => await _streamWriter.WriteLineAsync(_endOfSet);

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