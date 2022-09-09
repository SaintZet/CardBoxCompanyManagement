﻿using CardBoxCompanyManagement.Infrastructure;
using CardBoxCompanyManagement.ViewModels.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

internal class CRUDCompanyViewModel : INotifyPropertyChanged, IDataErrorInfo
{
    private Company? company;
    private int selectedCategory;
    private bool idHasError;

    public CRUDCompanyViewModel(ICategoriesRepository categories)
    {
        Categories = categories.Categories;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public string ID
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
        get => Company.Image.Uri;
        set
        {
            Company.Image.Uri = value;
            OnPropertyChanged();
        }
    }

    public int SelectedCategory
    {
        get => selectedCategory!;
        set
        {
            selectedCategory = value;
            Company.Category = Categories.First(c => c.Number == selectedCategory);
            OnPropertyChanged();
        }
    }

    public List<Category> Categories { get; }

    public bool IsEnabledBrowseImage { get; private set; } = true;
    public bool IsEnabledCategory { get; private set; } = true;
    public bool ReadOnlyID { get; private set; }
    public bool ReadOnlyName { get; private set; }
    public bool ReadOnlySummary { get; private set; }

    public ICommand BrowseImageCommand => new RelayCommand(execute: BrowseImage, canExecute: _ => IsEnabledBrowseImage);

    public ICommand ButtonIsEnabledCommand => new RelayCommand(execute: _ => DoNothing(), canExecute: _ => !CompanyPropertiesIsNullOrWhiteSpace() && !idHasError);

    public string Error => "Bulstat invalid!";

    public string this[string columnName]
    {
        get
        {
            return Validate(columnName);
        }
    }

    public CRUDCompanyViewModel Load(Company company, CRUDOperation operation)
    {
        this.company = company;
        SelectedCategory = company.Category.Number;

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

    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    private string Validate(string propertyName)
    {
        switch (propertyName)
        {
            case nameof(ID):
                idHasError = !EIKValidator.Validate(ID);
                return idHasError ? "Error" : string.Empty;
        }

        return string.Empty;
    }

    private void DoNothing()
    {
    }

    private bool CompanyPropertiesIsNullOrWhiteSpace()
    {
        return string.IsNullOrWhiteSpace(Company.Name) |
               string.IsNullOrWhiteSpace(Company.ID) |
               string.IsNullOrWhiteSpace(Company.Summary) |
               string.IsNullOrWhiteSpace(Company.Image.Uri?.ToString());
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
}