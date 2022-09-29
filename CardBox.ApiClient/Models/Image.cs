namespace CardBox.ApiClient.Models;

public class Image
{
    public Image(Uri? uri, string? base64)
    {
        Uri = uri;
        Base64 = base64;
    }

    public Uri? Uri { get; set; }
    public string? Base64 { get; set; }
}