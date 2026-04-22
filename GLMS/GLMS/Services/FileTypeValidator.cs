namespace GLMS.Services;

// section 4 rule from brief: block restricted file types (allow only pdf)
public class FileTypeValidator
{
    public void EnsurePdfOnly(string fileName)
    {
        // basic null/empty check first
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name is required.", nameof(fileName));

        // pull extension and compare ignoring case (PDF / pdf / Pdf all pass)
        var ext = Path.GetExtension(fileName);
        if (!string.Equals(ext, ".pdf", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Invalid file type. Only .pdf files are allowed.", nameof(fileName));
    }
}

// References (accessed 22 Apr 2026):
// - .NET Path.GetExtension docs: https://learn.microsoft.com/en-us/dotnet/api/system.io.path.getextension
// - StringComparison enum docs: https://learn.microsoft.com/en-us/dotnet/api/system.stringcomparison
