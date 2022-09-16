using System.Windows.Forms;

namespace CardBox.CompanyManagement.Services;

internal interface ISelectImageService
{
    string FileName { get; }

    public DialogResult ShowDialog();
}