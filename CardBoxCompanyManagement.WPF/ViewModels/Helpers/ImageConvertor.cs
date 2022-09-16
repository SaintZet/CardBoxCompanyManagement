using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace CardBoxCompanyManagement.ViewModels;

public static class ImageConvertor
{
    public static string ConvertToJsonBase64(BitmapImage image)
    {
        string imageUri = image.UriSource.ToString();
        string imageExt = imageUri.Substring(imageUri.Length - 3);

        return $"data:image/{imageExt};base64," + Base64ImageRepresentation(image);
    }

    private static string Base64ImageRepresentation(BitmapImage image)
    {
        byte[] imageBytes = ImageBitmapToBytes(image);
        return Convert.ToBase64String(imageBytes);
    }

    private static byte[] ImageBitmapToBytes(BitmapImage imageBitmap)
    {
        PngBitmapEncoder encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(imageBitmap));

        using (MemoryStream stream = new())
        {
            encoder.Save(stream);
            return stream.ToArray();
        }
    }
}