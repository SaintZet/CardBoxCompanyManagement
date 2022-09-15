using CardBoxCompanyManagement.Infrastructure;
using CardBoxCompanyManagement.ViewModels.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CardBoxCompanyManagement.ViewModels;

internal class CRUDCompanyViewModel : BaseViewModel, IDataErrorInfo
{
    private Company? company;
    private Category selectedCategory;
    private bool idHasError;

    public CRUDCompanyViewModel(ICategoriesRepository categories)
    {
        Categories = categories.Categories;
        selectedCategory = Categories[0];
    }

    public string CompanyID
    {
        get => company!.ID;
        set
        {
            company!.ID = value;
            OnPropertyChanged();
        }
    }

    public Company Company
    {
        get => company!;
        private set
        {
            company = value;
            OnPropertyChanged();
        }
    }

    public Uri ImageUri
    {
        get => Company.Image.Uri ?? new Uri("about:blank");
        set
        {
            Company.Image.Uri = value;
            OnPropertyChanged();
        }
    }

    public Category SelectedCategory
    {
        get => selectedCategory!;
        set
        {
            selectedCategory = value;
            Company.Category = Categories.First(c => c.Number == selectedCategory.Number);
            OnPropertyChanged();
        }
    }

    public List<Category> Categories { get; private set; }
    public bool IsEnabledBrowseImage { get; private set; } = true;
    public bool IsEnabledCategory { get; private set; } = true;
    public bool ReadOnlyID { get; private set; }
    public bool ReadOnlyName { get; private set; }
    public bool ReadOnlySummary { get; private set; }

    public ICommand BrowseImageCommand => new RelayCommand(execute: BrowseImage, canExecute: _ => IsEnabledBrowseImage);

    public ICommand ButtonIsEnabledCommand => new RelayCommand(execute: (obj) => ((Window)obj).DialogResult = true, canExecute: _ => !CompanyPropertiesIsNullOrWhiteSpace() && !idHasError);

    public string Error => "Bulstat invalid!";

    public string this[string columnName] => Validate(columnName);

    public CRUDCompanyViewModel Load(Company company, CRUDOperation operation)
    {
        this.company = company;
        SelectedCategory = company.Category;

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

    private string Validate(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(CompanyID):
                idHasError = !EIKValidator.Validate(CompanyID);
                return idHasError ? "Error" : string.Empty;
        }

        return string.Empty;
    }

    private bool CompanyPropertiesIsNullOrWhiteSpace()
    {
        return string.IsNullOrWhiteSpace(Company.Name) ||
               string.IsNullOrWhiteSpace(Company.ID) ||
               string.IsNullOrWhiteSpace(Company.Summary) ||
               string.IsNullOrWhiteSpace(Company.Image.Uri?.ToString());
    }

    //TODO: Create interface (for tests)
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
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}