using CardBoxCompanyManagement.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CardBoxCompanyManagement.ViewModels;

public enum CRUDOperation
{
    Add,
    Delete,
    Edit
}

internal class CRUDCompanyViewModel : INotifyPropertyChanged
{
    private Company? company;
    private int selectedCategory;

    public CRUDCompanyViewModel(ICategoriesRepository categories)
    {
        Categories = categories.Categories;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public Company Company
    {
        get => company!;
        set
        {
            company = value;
            OnPropertyChanged(nameof(Company));
        }
    }

    public Uri ImageUri
    {
        get
        {
            return Company.Image.Uri;
        }
        set
        {
            Company.Image.Uri = value;
            OnPropertyChanged(nameof(ImageUri));
        }
    }

    public int SelectedCategory
    {
        get => selectedCategory!;
        set
        {
            selectedCategory = value;
            Company.Category = Categories.First(c => c.Number == selectedCategory);
            OnPropertyChanged(nameof(SelectedCategory));
        }
    }

    public List<Category> Categories { get; }

    public bool IsEnabledBrowseImage { get; private set; } = true;
    public bool IsEnabledCategory { get; private set; } = true;
    public bool ReadOnlyID { get; private set; }
    public bool ReadOnlyName { get; private set; }
    public bool ReadOnlySummary { get; private set; }

    public ICommand BrowseImageCommand => new RelayCommand(execute: BrowseImage, canExecute: _ => IsEnabledBrowseImage);

    //TODO: Add execute if all textbox not null
    public ICommand CompleteOperationCommand => new RelayCommand(execute: CompleteOperation, _ => true);

    public CRUDCompanyViewModel Load(Company company, CRUDOperation operation)
    {
        this.company = company;
        SelectedCategory = company.Category!.Number;

        switch (operation)
        {
            case CRUDOperation.Add:
                break;

            case CRUDOperation.Delete:
                IsEnabledBrowseImage = false;
                IsEnabledCategory = false;
                ReadOnlyID = true;
                ReadOnlyName = true;
                ReadOnlySummary = true;
                break;

            case CRUDOperation.Edit:
                ReadOnlyID = true;
                break;

            default:
                throw new NotImplementedException();
        }
        return this;
    }

    public void OnPropertyChanged(string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    private void BrowseImage(object _)
    {
        OpenFileDialog dlg = new OpenFileDialog
        {
            InitialDirectory = @"%userprofile%\Pictures",
            Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
         "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
         "Portable Network Graphic (*.png)|*.png",
            RestoreDirectory = true
        };

        if (dlg.ShowDialog() == DialogResult.OK)
        {
            try
            {
                Uri uri = new(dlg.FileName, UriKind.Absolute);
                Company.Image.Base64 = ImageConvertor.ConvertToJsonBase64(new BitmapImage(uri));
                ImageUri = uri;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void CompleteOperation(object _)
    {
        var x = System.Windows.Interop.ComponentDispatcher.IsThreadModal;
    }
}