namespace Egzact.Share.Tests;

using Share;

public class GlobalOptionsTest
{
    [Fact]
    public void Test_Default()
    {
        var opt = new GlobalOptions();
        Assert.Equal(" ", opt.Ifs);
        Assert.Equal(" ", opt.Ofs);
        Assert.Equal(" ", opt.Fs);
        Assert.Equal("\n", opt.Eor);
        Assert.Equal("\n", opt.Eos);
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
    
    [Fact]
    public void Test_IfsとOfsに値を設定したときはそれが設定されているべき()
    {
        const string fs = ",";
        const string ifs = "ifs";
        const string ofs = "ofs";
        var opt = new GlobalOptions(fs)
        {
            Ifs = ifs, Ofs = ofs
        };
        Assert.Equal(ifs, opt.Ifs);
        Assert.Equal(ofs, opt.Ofs);
        Assert.Equal(fs, opt.Fs);
    }
}