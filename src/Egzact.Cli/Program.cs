﻿using System.Diagnostics.CodeAnalysis;
using Cocona;
using Egzact.Command;
using Egzact.IO;

var app = CoconaApp.Create();

app.AddSubCommand("conv",
    x =>
    {
        x.AddCommand(async (GlobalOptions globalOptions, [Option] bool each, [Argument] int numberOfColumns) =>
        {
            using var dummyStdin = new MemoryStream(Console.InputEncoding.GetBytes("""
                a b c d
                a b c d
                a b c d
                a b c d
                a b c d
                """));
            var conv = new Conv(numberOfColumns);
            // using var stdin = new StreamReader(Console.OpenStandardInput());
            using var stdin = new StreamReader(dummyStdin);
            await using var stdout = globalOptions.CreateOutputStream(Console.OpenStandardOutput());
            IReadOnlyList<IEnumerable<string>>? prevSet = null;

            IReadOnlyList<string> prevRemain = [];
            while (await stdin.ReadLineAsync() is { } line)
            {
                var inputRecord =
                    line.TrimEnd().Split(globalOptions.InputFieldSeparator ?? globalOptions.FieldSeparator);
                var (set, remain) =
                    conv.Execute(each ? inputRecord : prevRemain.Concat(inputRecord));

                prevRemain = remain;
                if (set is null && !each)
                {
                    continue;
                }

                set ??= new[] { remain };

                if (prevSet is not null)
                {
                    await stdout.WriteSetAsync(prevSet, !each);
                    if(each && globalOptions.EndOfSet != Environment.NewLine) await stdout.WriteLineEosAsync();
                }

                prevSet = set;
            }

            if (prevSet is not null)
            {
                await stdout.WriteSetAsync(prevSet, !each);
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
    [Option("fs", Description = "Field separator")]
    [HasDefaultValue]
    public string FieldSeparator { get; set; } = " ";

    // fsが指定されたとき、ifsもofsも指定されていない場合はfsを使う。
    // これをプロパティのgetter, setter で書くと
    // { get => _ifs ?? FieldSeparator; set => _ifs = value; }
    // みたいになるが、どこかのタイミングで FieldSeparator が " " になるのを確認している。なんでこうなるのかは深掘りしてない
    // とりあえず、ifs, ofs が指定されていないことが表明できればいいので Nullable で表現している
    [Option("ifs", Description = "Input field separator")]
    [HasDefaultValue]
    public string? InputFieldSeparator { get; set; } = null;

    [Option("ofs", Description = "Output field separator")]
    [HasDefaultValue]
    public string? OutputFieldSeparator { get; set; } = null;

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
                -1 => value,
                0 => Environment.NewLine,
                _ => $"{value[..(index - 1)]}"
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
        return new OutputStream(stream, OutputFieldSeparator ?? FieldSeparator, EndOfRecord, EndOfSet);
    }
}