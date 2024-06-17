namespace Egzact.Share.Tests;

using Share;

public class GlobalOptionsTest
{
    [Fact]
    public void Test_Fsのデフォルト値はスペースであるべき()
    {
        var opt = new GlobalOptions();
        Assert.Equal(" ", opt.Ifs);
        Assert.Equal(" ", opt.Ofs);
        Assert.Equal(" ", opt.Fs);
    }

    [Fact]
    public void Test_Fsに値を設定したときはその値がIfsとOfsに設定されているべき()
    {
        const string fs = ",";
        var opt = new GlobalOptions(fs);
        Assert.Equal(fs, opt.Ifs);
        Assert.Equal(fs, opt.Ofs);
        Assert.Equal(fs, opt.Fs);
    }
}