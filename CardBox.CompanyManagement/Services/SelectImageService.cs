using System.Windows.Forms;

namespace CardBox.CompanyManagement.Services;

internal class SelectImageService : ISelectImageService
{
    private readonly OpenFileDialog _dialog;

    public SelectImageService()
    {
        _dialog = new OpenFileDialog
        {
            InitialDirectory = @"%userprofile%\Pictures",
            Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                     "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                     "Portable Network Graphic (*.png)|*.png",
            RestoreDirectory = true
        };
    }

    public string FileName => _dialog.FileName;

    public DialogResult ShowDialog() => _dialog.ShowDialog();
}