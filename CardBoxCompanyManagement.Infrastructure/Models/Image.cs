namespace CardBoxCompanyManagement.Infrastructure;

public class Image
{
    public Image(Uri? uri, string? Base64)
    {
        Uri = uri;
        this.Base64 = Base64;
    }

    public Uri? Uri { get; set; }
    public string? Base64 { get; set; }
}