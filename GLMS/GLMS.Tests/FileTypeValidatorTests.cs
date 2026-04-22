using GLMS.Services;

namespace GLMS.Tests;

public class FileTypeValidatorTests
{
    // these should pass because they are all pdf names (different casing/patterns)
    [Theory]
    [InlineData("agreement.pdf")]
    [InlineData("AGREEMENT.PDF")]
    [InlineData("signed.contract.v1.pdf")]
    public void EnsurePdfOnly_DoesNotThrow_ForPdfFiles(string fileName)
    {
        var validator = new FileTypeValidator();
        var ex = Record.Exception(() => validator.EnsurePdfOnly(fileName));
        Assert.Null(ex);
    }

    // these should fail because they are restricted for this brief rule
    [Theory]
    [InlineData("virus.exe")]
    [InlineData("image.png")]
    [InlineData("notes.txt")]
    public void EnsurePdfOnly_Throws_ForNonPdfFiles(string fileName)
    {
        var validator = new FileTypeValidator();
        Assert.Throws<ArgumentException>(() => validator.EnsurePdfOnly(fileName));
    }

    // empty name is also invalid input
    [Fact]
    public void EnsurePdfOnly_Throws_ForEmptyFileName()
    {
        var validator = new FileTypeValidator();
        Assert.Throws<ArgumentException>(() => validator.EnsurePdfOnly(string.Empty));
    }
}

// References (accessed 22 Apr 2026):
// - xUnit theories and inline data: https://xunit.net/docs/getting-started/v2/getting-started
// - Record.Exception docs: https://api.xunit.net/v3/3.0.0/v3.3.0.0-Xunit.Record.Exception.html
