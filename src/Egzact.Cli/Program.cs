using System.Diagnostics.CodeAnalysis;
using Cocona;
using Egzact.Command;
using Egzact.IO;

var app = CoconaApp.Create();

using var dummyStdin = new MemoryStream(Console.InputEncoding.GetBytes(@"""
a b c d
a b c d
a b c d
a b c d
a b c d
"""));

app.AddSubCommand("conv",
    x =>
    {
        x.AddCommand(async (GlobalOptions globalOptions, [Option] bool each, [Argument] int numberOfColumns) =>
        {
            var conv = new Conv(numberOfColumns);
            // using var stdin = new StreamReader(Console.OpenStandardInput());
            using var stdin = new StreamReader(dummyStdin);
            await using var stdout = globalOptions.CreateOutputStream(Console.OpenStandardOutput());
            IReadOnlyList<IEnumerable<string>>? prevSet = null;

            string[] prevRemain = [];
            while (await stdin.ReadLineAsync() is { } line)
            {
                var inputRecord = line.Split(globalOptions.InputFieldSeparator);
                var (set, remain) =
                    conv.Execute(each ? inputRecord : prevRemain.Concat(inputRecord));

                if (set is null)
                {
                    // eachな場合、setが作られなかったときはremainをsetとして扱う
                    if (each)
                    {
                        set = [remain];
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    prevRemain = remain.ToArray();
                }
                
                if (prevSet is not null)
                {
                    await stdout.WriteSetAsync(prevSet, false);
                }

                prevSet = set;
            }

            if (prevSet is not null)
            {
                await stdout.WriteSetAsync(prevSet, true);
            }
        });
    });

app.Run();

/// <summary>
/// すべてのサブコマンドで利用しているオプションをまとめたクラス
/// </summary>
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class GlobalOptions : ICommandParameterSet
{
    private string _fs = " ";

    [Option("fs", Description = "Field separator")]
    [HasDefaultValue]
    public string FieldSeparator
    {
        get => _fs;
        set
        {
            _fs = value;
            _ifs = value;
            _ofs = value;
            
            Console.WriteLine($"fs: {value} ifs: {InputFieldSeparator} ofs: {OutputFieldSeparator}");
        }
    }

    private string? _ifs, _ofs;

    [Option("ifs", Description = "Input field separator")]
    [HasDefaultValue]
    public string InputFieldSeparator
    {
        get => _ifs ?? FieldSeparator;
        set => _ifs = value;
    }

    [Option("ofs", Description = "Output field separator")]
    [HasDefaultValue]
    public string OutputFieldSeparator
    {
        get => _ofs ?? FieldSeparator;
        set => _ofs = value;
    }

    private string _endOfSet = Environment.NewLine;

    [Option("eos", Description = "End of set")]
    [HasDefaultValue]
    public string EndOfSet
    {
        get => _endOfSet;
        set
        {
            // eosはSetごとに1行で出力する。eosに改行込みの文字列が渡されたとしても、最初の改行文字の前までしか使わない
            // ex: eos=@@@ の場合、@@@ がセットの最後に出力される
            // ex: eos=a\nb\n の場合、a がセットの最後に出力される
            // eosは必ず行頭から始まる。つまりeosの最初の文字は改行文字である
            var index = value.IndexOf(Environment.NewLine, StringComparison.Ordinal);
            _endOfSet = index switch
            {
                -1 => $"{Environment.NewLine}{value}",
                0 => value,
                _ => $"{Environment.NewLine}{value[..(index - 1)]}"
            };
        }
    }

    [Option("eor", Description = "end of record")]
    [HasDefaultValue]
    public string EndOfRecord { get; set; } = Environment.NewLine;

    /// <summary>
    /// 出力ストリームを作って返す
    /// </summary>
    /// <param name="stream">書き込み先になるストリーム。大体の場合stdout</param>
    /// <returns></returns>
    public OutputStream CreateOutputStream(Stream stream)
    {
        return new OutputStream(stream, OutputFieldSeparator, EndOfRecord, EndOfSet);
    }
}