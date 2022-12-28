using CardBox.ApiClient.Contracts;
using CardBox.ApiClient.Models;
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
    private readonly ICategoriesService _categories;
    private readonly ISelectImageService _selectImageService;
    private Company? _company;
    private Category _selectedCategory;
    private bool _idHasError;

    public CRUDCompanyViewModel(ICategoriesService categories, ISelectImageService selectImageService)
    {
        _categories = categories;
        _selectImageService = selectImageService;
        Categories = categories.GetCategories();
        _selectedCategory = Categories[0];
    }

    public string CompanyID
    {
        get => _company!.ID;
        set
        {
            _company!.ID = value;
            OnPropertyChanged();
        }
    }
    public Company Company
    {
        get => _company!;
        private set
        {
            _company = value;
            OnPropertyChanged();
        }
    }
    public Uri ImageUri
    {
        get
        {
            if (Company.Image is null)
            {
                Company.Image = new Image();
            }

            return Company.Image!.Uri ?? new Uri("about:blank");
        }
        set
        {
            Company.Image!.Uri = value;
            OnPropertyChanged();
        }
    }
    public Category SelectedCategory
    {
        get => _selectedCategory!;
        set
        {
            _selectedCategory = value;
            Company.Category = Categories.First(c => c.Number == _selectedCategory.Number);
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

    public ICommand ButtonIsEnabledCommand => new RelayCommand(execute: CloseWindow, canExecute: _ => !CompanyPropertiesIsNullOrWhiteSpace() && !_idHasError);

    public string Error => "Bulstat invalid!";

    public string this[string columnName] => Validate(columnName);

    public CRUDCompanyViewModel Load(Company company, CRUDOperation operation)
    {
        _company = company;
        SelectedCategory = company.Category ?? _categories.GetCategories().FirstOrDefault()!; ;

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
                _idHasError = !EIKValidator.Validate(CompanyID);
                return _idHasError ? "Error" : string.Empty;

            default:
                return string.Empty;
        }
    }

    private bool CompanyPropertiesIsNullOrWhiteSpace()
    {
        return string.IsNullOrWhiteSpace(Company.Name) ||
               string.IsNullOrWhiteSpace(Company.ID) ||
               string.IsNullOrWhiteSpace(Company.Summary) ||
               string.IsNullOrWhiteSpace(Company.Image?.Uri?.ToString());
    }

    private void BrowseImage(object _)
    {
        if (_selectImageService.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        try
        {
            Uri uri = new(_selectImageService.FileName, UriKind.Absolute);
            Company.Image!.Base64 = ImageConvertor.ConvertToJsonBase64(new BitmapImage(uri));
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