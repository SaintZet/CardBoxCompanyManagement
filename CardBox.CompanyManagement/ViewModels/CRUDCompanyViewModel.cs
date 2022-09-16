using CardBox.ApiClient.Models;
using CardBox.ApiClient.Services;
using CardBox.CompanyManagement.Services;
using CardBox.CompanyManagement.ViewModels.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CardBox.CompanyManagement.ViewModels;

internal class CRUDCompanyViewModel : BaseViewModel, IDataErrorInfo
{
    private readonly ICategoriesService categories;
    private readonly ISelectImageService selectImageService;
    private Company? company;
    private Category selectedCategory;
    private bool idHasError;

    public CRUDCompanyViewModel(ICategoriesService categories, ISelectImageService selectImageService)
    {
        this.categories = categories;
        this.selectImageService = selectImageService;
        Categories = categories.GetCategories();
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

    public ICommand ButtonIsEnabledCommand => new RelayCommand(execute: CloseWindow, canExecute: _ => !CompanyPropertiesIsNullOrWhiteSpace() && !idHasError);

    public string Error => "Bulstat invalid!";

    public string this[string columnName] => Validate(columnName);

    public CRUDCompanyViewModel Load(Company company, CRUDOperation operation)
    {
        this.company = company;
        SelectedCategory = company.Category ?? categories.GetCategories().FirstOrDefault()!; ;

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

    private void BrowseImage(object _)
    {
        if (selectImageService.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        try
        {
            Uri uri = new(selectImageService.FileName, UriKind.Absolute);
            Company.Image.Base64 = ImageConvertor.ConvertToJsonBase64(new BitmapImage(uri));
            ImageUri = uri;
        }
        catch (Exception ex)
        {
            System.Windows.Forms.MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void CloseWindow(object obj)
    {
        ((Window)obj).DialogResult = true;
    }
}